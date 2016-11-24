using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class CatBossProgressingState : CatProgressingState {

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

	bool _isBreakingDoor = false;
	bool _isEnteringStaircase = false;
	bool _isRetargeting = false;

	Coroutine enteringStaircaseCoroutine = null;
	Coroutine retargetingCoroutine = null;

	public CatBossProgressingState (Cat cat) : base(cat) {

	}

	public override void Update () {
		if (_target == null && cat.currentRoom != null && !_isEnteringStaircase && !_isRetargeting) {
			// The cat does not have a target and isn't currently retargeting (probably just transitioned from another state)

			// Evaluate the room
			EvaluateRoom (cat.currentRoom);

			// Determine a target and move towards it
			retargetingCoroutine = cat.StartCoroutine (RetargetCoroutine ());

		}
	}

	public override void OnTargetReached () {
		// The cat has reached its target

		if (_target != null && !_isEnteringStaircase && !_isRetargeting) {
			// The cat has a valid target

			if (_target.isStaircase) {
				// Target is a staircase

				enteringStaircaseCoroutine = cat.StartCoroutine (EnteringStaircaseCoroutine ());

			} else {
				// Target is a regular room door

				// Evaluate the room
				EvaluateRoom (cat.currentRoom);

				if (GetRoomStatus (cat.currentRoom).visitCount <= 1) {
					// First time in this room, explore it
					ToExploringState ();

				} else {
					// Been here before, just move on
					retargetingCoroutine = cat.StartCoroutine (RetargetCoroutine ());
				}

			}

		}

	}

	void DetermineTarget () {
		if (cat.currentRoom == null) return;

		List<DoorOption> options = new List<DoorOption> ();

		// Check the left door
		if (cat.currentRoom.leftDoor != null) {
			options.Add (new DoorOption (cat.currentRoom.leftDoor, cat.currentRoom.roomLeft));
		}

		// Check the right door
		if (cat.currentRoom.rightDoor != null) {
			options.Add (new DoorOption (cat.currentRoom.rightDoor, cat.currentRoom.roomRight));
		}

		// Check all the staircase doors
		foreach (Door door in cat.currentRoom.staircases) {
			if (door.destination != null && door.destination.room != null) {
				options.Add (new DoorOption (door, door.destination.room));
			}
		}

		// Determine the best option
		options = options.OrderBy (x => GetRoomStatus (x.destination).isDeadEnd)
			.ThenBy (x => GetRoomStatus (x.destination).visitCount)
			.ThenBy (x => Random.Range (int.MinValue, int.MaxValue))
			.ToList ();
		
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

	void EvaluateRoom (Room room) {
		RoomStatus status = new RoomStatus ();
		roomStatus.TryGetValue (room, out status);

		int validPaths = 0;

		if (cat.currentRoom.leftDoor != null && cat.currentRoom.roomLeft != null && !GetRoomStatus (cat.currentRoom.roomLeft).isDeadEnd) {
			validPaths++;
		}

		if (cat.currentRoom.rightDoor != null && cat.currentRoom.roomRight != null && !GetRoomStatus (cat.currentRoom.roomRight).isDeadEnd) {
			validPaths++;
		}

		foreach (Door door in cat.currentRoom.staircases) {
			if (door.destination != null && door.destination.room != null && !GetRoomStatus (door.destination.room).isDeadEnd) {
				validPaths++;
			}
		}

		if (validPaths <= 1) {
			status.isDeadEnd = true;
		}
		
		status.visitCount++;
		roomStatus [room] = status;
	}

	public override void OnCollisionEnter (Collision collision) {
		if (collision.gameObject.CompareTag ("Door") && !_isBreakingDoor) {
			
			Vector3 normal = collision.contacts[0].normal;
			if ((normal.x < 0 && cat.isFacingRight) || (normal.x > 0 && cat.isFacingLeft) || (normal.z < 0)) {
				// Kick open doors in the way like a boss
				cat.StartCoroutine (BreakDoorCoroutine (collision.gameObject.GetComponentInParent<Door> (), 0.5f, 1.0f));
			}

		}
	}

	IEnumerator BreakDoorCoroutine (Door door, float breakDelay, float moveDelay) {
		if (_isBreakingDoor || door == null) yield break;
		_isBreakingDoor = true;

		cat.controller.StopMoving ();
		cat.animator.SetTrigger ("breakDoor");

		yield return new WaitForSeconds (breakDelay);

		door.BreakOpen ();

		yield return new WaitForSeconds (moveDelay);

		cat.controller.StartMoving ();

		_isBreakingDoor = false;
	}

	IEnumerator EnteringStaircaseCoroutine () {
		if (_isEnteringStaircase) yield break;
		_isEnteringStaircase = true;

		// Flip
		cat.Flip ();

		// Walk in
		cat.controller.LockX ();
		_targetPosition = _target.teleportPosition.position;
		cat.controller.SetTarget (_targetPosition);
		cat.controller.movementSpeed = cat.walkingSpeed * cat.enteringStaircaseMultiplier;
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
		cat.Flip ();

		// Fade in
		yield return cat.StartCoroutine (cat.Fade (1.0f, 0.5f));

		// Walk out
		_targetPosition = _target.destination.teleportPosition.position;
		_targetPosition.z = cat.zPosition;
		cat.controller.SetTarget (_targetPosition);
		cat.controller.movementSpeed = cat.walkingSpeed * cat.enteringStaircaseMultiplier;
		cat.controller.StartMoving ();

		while (!cat.controller.reachedTarget) {
			yield return null;
		}

		cat.controller.StopMoving ();
		cat.controller.LockZ ();
		cat.controller.movementSpeed = cat.walkingSpeed;

		// Determine next target / state
		if (GetRoomStatus (cat.currentRoom).visitCount <= 1) {
			// First time in this room, explore it
			ToExploringState ();

		} else {
			// Been here before, just move on
			retargetingCoroutine = cat.StartCoroutine (RetargetCoroutine ());
		}

		_isEnteringStaircase = false;
	}

	IEnumerator RetargetCoroutine (float retryDelay = 0.0f, bool tryToOpenDoor = false) {
		if (_isRetargeting) yield break;
		_isRetargeting = true;

		if (_isEnteringStaircase) {
			cat.StopCoroutine (enteringStaircaseCoroutine);
			_isEnteringStaircase = false;
		}

		Door oldTarget = _target;
		_target = null;

		// Stop
		cat.controller.StopMoving ();

		if (tryToOpenDoor) {
			cat.animator.SetBool ("tryingToOpenDoor", true);
		}

		// Find a new target
		do {

			yield return new WaitForSeconds (retryDelay);
			DetermineTarget ();

		} while (_target == null || (_target == oldTarget && !_target.isOpen));

		cat.animator.SetBool ("tryingToOpenDoor", false);

		// Return to original Z position if necessary
		if (cat.transform.position.z != cat.zPosition && !(_target.isStaircase && cat.transform.position.x == _target.teleportPosition.position.x)) {

			cat.Flip ();

			cat.controller.LockX ();
			Vector3 pos = cat.transform.position;
			pos.z = cat.zPosition;
			cat.controller.SetTarget (pos);
			cat.controller.movementSpeed = cat.walkingSpeed * cat.enteringStaircaseMultiplier;
			cat.controller.StartMoving ();

			while (!cat.controller.reachedTarget) {
				yield return null;
			}

		}

		// Start moving towards the new target
		cat.controller.SetTarget (_targetPosition);
		cat.controller.LockZ ();
		cat.controller.movementSpeed = cat.walkingSpeed;
		cat.controller.StartMoving ();

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
		_target = null;
		StopCoroutines ();

		cat.currentState = cat.exploringState;
	}

	public override void ToPanickingState () {
		_target = null;
		StopCoroutines ();

		cat.currentState = cat.panickingState;
	}

	public override void ToLuredState () {
		_target = null;
		StopCoroutines ();
		
		cat.currentState = cat.luredState;
	}
}
