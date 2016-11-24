using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class CatDefaultPanickingState : CatPanickingState {

	public float roomPadding = 1.0f;

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

	public CatDefaultPanickingState (Cat cat) : base(cat) {

	}

	public override void Panic (float duration) {
		if (cat.currentState != this) {

			// Switch to this state
			cat.currentState.ToPanickingState ();

			// Cancel if the current state refuses
			if (cat.currentState != this)
				return;

			// Start the timer
			_timeLeft = duration;
			cat.StartCoroutine (TimerCoroutine ());

			// Pick a target
			retargetingCoroutine = cat.StartCoroutine (RetargetCoroutine ());

		} else {
			// If the cat is already panicking, extend the duration
			if (_timeLeft < duration) {
				_timeLeft = duration;
			}
		}
	}

	IEnumerator TimerCoroutine () {
		cat.animator.SetBool ("panicking", true);

		while (_timeLeft > 0.0f) {
			_timeLeft -= Time.deltaTime;
			yield return null;
		}

		cat.animator.SetBool ("panicking", false);

		// Wait for coroutines to finish before transitioning
		if (_isEnteringStaircase) {
			yield return enteringStaircaseCoroutine;

		} else if (_isRetargeting) {
			yield return retargetingCoroutine;
		}

		ToProgressingState ();
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

				// Move to the next target
				retargetingCoroutine = cat.StartCoroutine (RetargetCoroutine ());

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

		// Pick a random door
		options = options.OrderBy (x => Random.Range (int.MinValue, int.MaxValue))
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

	public override void OnCollisionEnter (Collision collision) {
		if (collision.gameObject.CompareTag ("Door") && !_isRetargeting) {
			
			Vector3 normal = collision.contacts[0].normal;
			if ((normal.x < 0 && cat.isFacingRight) || (normal.x > 0 && cat.isFacingLeft) || (normal.z < 0)) {
				// Find a new target if current destination is suddenly blocked by a door
				retargetingCoroutine = cat.StartCoroutine (RetargetCoroutine (2.0f, true));
			}

		}
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
		cat.controller.movementSpeed = cat.panickingSpeed * cat.enteringStaircaseMultiplier;
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
		cat.controller.movementSpeed = cat.panickingSpeed * cat.enteringStaircaseMultiplier;
		cat.controller.StartMoving ();

		while (!cat.controller.reachedTarget) {
			yield return null;
		}

		cat.controller.StopMoving ();
		cat.controller.LockZ ();
		cat.controller.movementSpeed = cat.panickingSpeed;

		// Move on to next target
		retargetingCoroutine = cat.StartCoroutine (RetargetCoroutine ());

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
			cat.controller.movementSpeed = cat.panickingSpeed * cat.enteringStaircaseMultiplier;
			cat.controller.StartMoving ();

			while (!cat.controller.reachedTarget) {
				yield return null;
			}

		}

		// Start moving towards the new target
		cat.controller.SetTarget (_targetPosition);
		cat.controller.LockZ ();
		cat.controller.movementSpeed = cat.panickingSpeed;
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

	public override void ToProgressingState () {
		Debug.Log ("Entered progressing state!");

		_target = null;
		StopCoroutines ();

		cat.currentState = cat.progressingState;
	}

	public override void ToExploringState () {
		// The exploring state can only be entered from the progressing state
	}

	public override void ToLuredState () {
		// Can't be lured while panicking
	}
}
