using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class CatDefaultProgressingState : CatProgressingState {

	public float roomPadding = 1.0f;

	struct RoomStatus {
		public int visitCount;
		public bool isDeadEnd;

		public RoomStatus (int visitCount = 0, bool isDeadEnd = false) {
			this.visitCount = visitCount;
			this.isDeadEnd = isDeadEnd;
		}
	}

	IDictionary<Room, RoomStatus> roomStatus = new Dictionary<Room, RoomStatus> ();

	struct DoorOption {
		public Door door;
		public Room destination;

		public DoorOption (Door door, Room destination) {
			this.door = door;
			this.destination = destination;
		}
	}

	bool _isEnteringStaircase = false;
	bool _isRetargeting = false;

	Coroutine enteringStaircaseCoroutine = null;
	Coroutine retargetingCoroutine = null;

	public CatDefaultProgressingState (Cat cat) : base(cat) {

	}

	public override void Start () {
		
	}

	public override void Update () {
		if (_target == null && !_isEnteringStaircase && !_isRetargeting) {
			// The cat does not have a target and isn't currently retargeting

			DetermineTarget ();
			cat.controller.LockZ ();
			cat.controller.movementSpeed = cat.walkingSpeed;
			if (_target != null) cat.controller.StartMoving ();

		}
	}

	public override void OnTargetReached () {
		// The cat has reached its target

		if (_target != null && !_isEnteringStaircase) {
			Debug.Log ("Target reached!");
			// The cat has a valid target

			if (_target.isStaircase) {
				// Target is a staircase

				enteringStaircaseCoroutine = cat.StartCoroutine (EnteringStaircaseCoroutine ());
				Debug.Log ("Entering staircase!");

			} else {
				// Target is a room

				if (GetRoomStatus (_currentRoom).visitCount <= 1) {
					Debug.Log ("Exploring room for the first time!");
					// First time in this room, explore it
					ToExploringState ();

				} else {
					Debug.Log ("Been here before!");
					// Been here before, just move on
					DetermineTarget ();
					cat.controller.LockZ ();
					cat.controller.movementSpeed = cat.walkingSpeed;
					if (_target != null) cat.controller.StartMoving ();
				}

			}

		}

	}

	void DetermineTarget () {
		if (_currentRoom == null) return;

		Debug.Log ("Determining next target!");

		List<DoorOption> options = new List<DoorOption> ();

		// Check the left door
		if (_currentRoom.leftDoor != null && _currentRoom.leftDoor.isOpen) {
			options.Add (new DoorOption (_currentRoom.leftDoor, _currentRoom.roomLeft));
		}

		// Check the right door
		if (_currentRoom.rightDoor != null && _currentRoom.rightDoor.isOpen) {
			options.Add (new DoorOption (_currentRoom.rightDoor, _currentRoom.roomRight));
		}

		// Check all the staircase doors
		foreach (Door door in _currentRoom.staircases) {
			if (door.isOpen && door.destination != null && door.destination.room != null) {
				options.Add (new DoorOption (door, door.destination.room));
			}
		}

		// Determine the best option
		options = options.OrderBy (x => GetRoomStatus (x.destination).isDeadEnd).ThenBy (x => GetRoomStatus (x.destination).visitCount).ThenBy (x => Random.Range (int.MinValue, int.MaxValue)).ToList ();
		if (options.Count == 0) {
			// No valid paths
			_target = null;

		} else {
			// Set the new target
			_target = options [0].door;

			if (_target.isStaircase) {
				_targetPosition = _target.teleportPosition.position;

			} else {
				_targetPosition = _target.transform.position;
				_targetPosition.x += (options[0].destination.transform.position.x - _targetPosition.x > 0.0f) ? roomPadding : -roomPadding;
			}

			_targetPosition.z = cat.zPosition;

			cat.controller.SetTarget (_targetPosition);
		}
	}

	RoomStatus GetRoomStatus (Room room) {
		RoomStatus status = new RoomStatus ();
		roomStatus.TryGetValue (room, out status);
		return status;
	}

	void IncrementVisitCount (Room room) {
		RoomStatus status = new RoomStatus ();
		roomStatus.TryGetValue (room, out status);

		int validPaths = 0;

		if (_currentRoom.leftDoor != null && _currentRoom.roomLeft != null && !GetRoomStatus (_currentRoom.roomLeft).isDeadEnd) {
			validPaths++;
		}

		if (_currentRoom.rightDoor != null && _currentRoom.roomRight != null && !GetRoomStatus (_currentRoom.roomRight).isDeadEnd) {
			validPaths++;
		}

		foreach (Door door in _currentRoom.staircases) {
			if (door.destination != null && door.destination.room != null && !GetRoomStatus (door.destination.room).isDeadEnd) {
				validPaths++;
			}
		}

		if (validPaths <= 1) {
			Debug.Log ("This room is a dead end!");
			status.isDeadEnd = true;
		}
		
		status.visitCount++;
		roomStatus [room] = status;
	}

	public override void OnTriggerEnter (Collider other) {
		if (other.CompareTag("Room")) {
			Room room = other.GetComponent<Room> ();
			if (room != _currentRoom) {
				_currentRoom = room;

				// Count visit
				IncrementVisitCount (_currentRoom);
			}
		}
	}

	public override void OnCollisionEnter (Collision collision) {
		if (collision.gameObject.CompareTag ("Door") && !_isRetargeting) {
			
			Vector3 normal = collision.contacts[0].normal;
			if ((normal.x < 0 && cat.transform.localScale.x > 0) || (normal.x > 0 && cat.transform.localScale.x < 0) || (normal.z < 0)) {
				// Find a new target if current destination is suddenly blocked by a door
				retargetingCoroutine = cat.StartCoroutine (RetargetCoroutine (true));
			}

		}
	}

	public override void OnTriggerStay (Collider other) {
		if (other.CompareTag("Room") && _currentRoom == null) {
			Room room = other.GetComponent<Room> ();
			_currentRoom = room;

			if (!_isEnteringStaircase && !_isRetargeting) {
				retargetingCoroutine = cat.StartCoroutine (RetargetCoroutine ());
			}
		}
	}

	public override void OnTriggerExit (Collider other) {
		if (other.CompareTag("Room")) {
			Room room = other.GetComponent<Room> ();
			if (room == _currentRoom) {
				_currentRoom = null;
			}
		}
	}

	IEnumerator EnteringStaircaseCoroutine () {
		if (_isEnteringStaircase) yield break;
		_isEnteringStaircase = true;

		// Flip
		Vector3 scale = cat.transform.localScale;
		scale.x = -scale.x;
		cat.transform.localScale = scale;

		// Walk in
		cat.controller.LockX ();
		_targetPosition = _target.teleportPosition.position;
		cat.controller.SetTarget (_targetPosition);
		cat.controller.movementSpeed = cat.enteringStaircaseSpeed;
		cat.controller.StartMoving ();

		while (!cat.controller.reachedTarget) {
			yield return null;
		}

		cat.controller.StopMoving ();

		// Fade out
		yield return cat.StartCoroutine (cat.Fade (0.0f, 0.5f));

		// Teleport
		cat.transform.position = _target.destination.teleportPosition.position;

		// Flip
		scale.x = -scale.x;
		cat.transform.localScale = scale;

		// Fade in
		yield return cat.StartCoroutine (cat.Fade (1.0f, 0.5f));

		// Walk out
		_targetPosition = _target.destination.teleportPosition.position;
		_targetPosition.z = cat.zPosition;
		cat.controller.SetTarget (_targetPosition);
		cat.controller.movementSpeed = cat.enteringStaircaseSpeed;
		cat.controller.StartMoving ();

		while (!cat.controller.reachedTarget) {
			yield return null;
		}

		cat.controller.StopMoving ();
		cat.controller.LockZ ();
		cat.controller.movementSpeed = cat.walkingSpeed;

		// Determine next target / state
		if (GetRoomStatus (_currentRoom).visitCount <= 1) {
			// First time in this room, explore it
			ToExploringState ();

		} else {
			// Been here before, just move on
			DetermineTarget ();
			if (_target != null) cat.controller.StartMoving ();
		}

		_isEnteringStaircase = false;
	}

	IEnumerator RetargetCoroutine (bool tryToOpenDoor = false) {
		if (_isRetargeting) yield break;
		_isRetargeting = true;

		if (_isEnteringStaircase) {
			cat.StopCoroutine (enteringStaircaseCoroutine);
			_isEnteringStaircase = false;
		}

		// Stop
		yield return new WaitForSeconds (0.5f);
		_target = null;
		cat.controller.StopMoving ();

		if (tryToOpenDoor) {
			cat.animator.SetBool ("tryingToOpenDoor", true);
			yield return new WaitForSeconds (2.0f);
			cat.animator.SetBool ("tryingToOpenDoor", false);

		} else {
			yield return new WaitForSeconds (2.0f);
		}

		// Return to original Z position
		if (cat.transform.position.z != cat.zPosition) {

			Vector3 scale = cat.transform.localScale;
			scale.x = -scale.x;
			cat.transform.localScale = scale;

			cat.controller.LockX ();
			_targetPosition = cat.transform.position;
			_targetPosition.z = cat.zPosition;
			cat.controller.SetTarget (_targetPosition);
			cat.controller.movementSpeed = cat.enteringStaircaseSpeed;
			cat.controller.StartMoving ();

			while (!cat.controller.reachedTarget) {
				yield return null;
			}

		}

		// Retarget and move
		DetermineTarget ();
		cat.controller.LockZ ();
		cat.controller.movementSpeed = cat.walkingSpeed;
		if (_target != null) cat.controller.StartMoving ();

		_isRetargeting = false;
	}

	public void StopCoroutines () {

		if (_isEnteringStaircase) {
			cat.StopCoroutine (enteringStaircaseCoroutine);
			_isEnteringStaircase = false;
		}

		if (_isRetargeting) {
			cat.StopCoroutine (retargetingCoroutine);
			_isRetargeting = false;
		}

	}

	public override void ToExploringState () {
		Debug.Log ("Entered exploring state!");

		_target = null;
		StopCoroutines ();

		cat.currentState = cat.exploringState;
	}

	public override void ToLuredState () {
		Debug.Log ("Entered lured state!");

		_target = null;
		StopCoroutines ();
		
		cat.currentState = cat.luredState;
	}
}
