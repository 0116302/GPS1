using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class CatDefaultProgressingState : CatProgressingState {

	public float roomPadding = 2.0f;

	IDictionary<Room, int> roomVisitCounter = new Dictionary<Room, int> ();

	struct DoorOption {
		public Door door;
		public Room destination;

		public DoorOption (Door door, Room destination) {
			this.door = door;
			this.destination = destination;
		}
	}

	Door _target;
	Vector3 _targetPosition;

	Coroutine enteringStaircase = null;
	Coroutine retargeting = null;

	public CatDefaultProgressingState (Cat cat) : base(cat) {

	}

	public override void Start () {
		
	}

	public override void Update () {
		if (_target != null) {

			if (cat.transform.position == _targetPosition) {
				
				if (_target.isStaircase) {
					// If staircase position has been reached, enter it
					if (enteringStaircase == null)
						enteringStaircase = cat.StartCoroutine (EnterStaircaseCoroutine ());

				} else {
					
					if (GetVisitCount (_currentRoom) <= 1) {
						// First time in this room, explore it
						ToExploringState ();

					} else {
						// Been here before, just move on
						DetermineTarget ();
					}

				}

			} else {
				
				if (enteringStaircase == null) {
					// Walk towards target
					cat.transform.position = Vector3.MoveTowards (cat.transform.position, _targetPosition, cat.walkingSpeed * Time.deltaTime);

					// Flip cat to face the right direction and play the moving animation
					FaceTarget ();
					cat.animator.SetFloat ("movementSpeed", cat.walkingSpeed);

					// If target door is no longer open, find a new target
					if (!_target.isOpen && retargeting == null) {
						retargeting = cat.StartCoroutine (RetargetCoroutine ());

						cat.animator.SetFloat ("movementSpeed", 0.0f);
					}
				}
			}

		} else if (retargeting == null) {
			DetermineTarget ();

			cat.animator.SetFloat ("movementSpeed", 0.0f);
		}
	}

	void FaceTarget () {
		Vector3 scale = cat.transform.localScale;

		float deltaX = _targetPosition.x - cat.transform.position.x;
		if (deltaX > 0) {
			scale.x = Mathf.Abs (scale.x);

		} else if (deltaX < 0) {
			scale.x = -(Mathf.Abs (scale.x));
		}

		cat.transform.localScale = scale;
	}

	IEnumerator EnterStaircaseCoroutine () {
		float originalZ = cat.transform.position.z;
		Vector3 scale = cat.transform.localScale;

		// Align
		cat.transform.position = _targetPosition;
		scale.x = -scale.x;
		cat.transform.localScale = scale;

		// Walk in
		while (cat.transform.position != _target.teleportPosition.position) {
			cat.transform.position = Vector3.MoveTowards (cat.transform.position, _target.teleportPosition.position, cat.walkingSpeed * 2.0f * Time.deltaTime);
			yield return new WaitForEndOfFrame ();
		}

		// Teleport
		cat.transform.position = _target.destination.teleportPosition.position;
		scale.x = -scale.x;
		cat.transform.localScale = scale;

		// Walk out
		Vector3 newPos = _target.destination.teleportPosition.position;
		newPos.z = originalZ;
		while (cat.transform.position != newPos) {
			cat.transform.position = Vector3.MoveTowards (cat.transform.position, newPos, cat.walkingSpeed * 2.0f * Time.deltaTime);
			yield return new WaitForEndOfFrame ();
		}

		enteringStaircase = null;

		if (GetVisitCount (_currentRoom) <= 1) {
			// First time in this room, explore it
			ToExploringState ();

		} else {
			// Been here before, just move on
			DetermineTarget ();
		}
	}

	IEnumerator RetargetCoroutine () {
		yield return new WaitForSeconds (1.0f);
		_target = null;
		yield return new WaitForSeconds (2.0f);
		retargeting = null;
		DetermineTarget ();
	}

	void DetermineTarget () {
		if (_currentRoom == null) return;

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
		options = options.OrderBy (x => GetVisitCount (x.destination)).ThenBy (x => Random.Range (int.MinValue, int.MaxValue)).ToList ();
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

			_targetPosition.z = cat.transform.position.z;
		}
	}

	int GetVisitCount (Room room) {
		int visitCount;
		roomVisitCounter.TryGetValue (room, out visitCount);
		return visitCount;
	}

	void IncrementVisitCount (Room room) {
		int visitCount;
		roomVisitCounter.TryGetValue (room, out visitCount);
		roomVisitCounter [room] = visitCount + 1;
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

	public override void OnTriggerStay (Collider other) {
		if (other.CompareTag("Room") && _currentRoom == null) {
			Room room = other.GetComponent<Room> ();
			_currentRoom = room;

			if (enteringStaircase == null && retargeting == null) {
				retargeting = cat.StartCoroutine (RetargetCoroutine ());
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

	public override void ToExploringState () {
		_target = null;
		cat.currentState = cat.exploringState;
	}
}
