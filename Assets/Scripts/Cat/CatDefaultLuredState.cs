using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class CatDefaultLuredState : CatLuredState {

	public CatDefaultLuredState (Cat cat) : base(cat) {

	}

	public override void Lure (Lure target) {
		if (cat.currentState != this && target.isActive) {
			int layerMask = 1 << LayerMask.NameToLayer ("Rooms");

			if (!Physics.Linecast (cat.head.transform.position, target.transform.position, layerMask, QueryTriggerInteraction.Ignore)) {
				// There is a direct line of sight between the cat and the lure

				// Lure successful, switch the cat to this state
				_target = target;
				cat.currentState.ToLuredState ();

				// Cancel if the current state refuses
				if (cat.currentState != this)
					return;

				// Approach the lure
				cat.StartCoroutine (ApproachTarget ());
			}
		}
	}

	IEnumerator ApproachTarget () {

		// Return to original Z position if necessary
		if (cat.transform.position.z != cat.zPosition) {
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

		// Set the new target
		_targetPosition = cat.transform.position;
		_targetPosition.x = _target.transform.position.x;
		_targetPosition.z = cat.zPosition;

		// Randomize the target position
		if (Random.Range (0, 2) == 0) {
			_targetPosition.x += Random.Range (_target.minDistance, _target.maxDistance);

		} else {
			_targetPosition.x -= Random.Range (_target.minDistance, _target.maxDistance);
		}

		cat.controller.SetTarget (_targetPosition);

		// Start moving in the target's direction
		cat.controller.LockZ ();
		cat.controller.movementSpeed = cat.walkingSpeed;
		cat.controller.StartMoving ();

		while (!cat.controller.reachedTarget) {
			yield return null;
		}

		if (_target.isActive) {
			// Flip the cat to face the lure
			float deltaX = _target.transform.position.x - cat.transform.position.x;

			if (deltaX > 0.0f) {
				cat.FaceRight ();
			} else if (deltaX < 0.0f) {
				cat.FaceLeft ();
			}

			int r = Random.Range (1, 5);
			switch (r) {
			case 1:
				cat.Say ("What's a fine cat like you doing in a place like this?");
				break;
			case 2:
				cat.Say ("Meeoww!");
				break;
			case 3:
				cat.Say ("Rawrr!");
				break;
			case 4:
				cat.Say ("Hey girl.");
				break;
			case 5:
				cat.Say ("So... you come here often?");
				break;
			}
		}

		while (_target.isActive) {
			// Stay until the lure deactivates
			yield return new WaitForSeconds (1.0f);
		}

		ToProgressingState ();
	}

	public override void Update () {
		if (_target == null) {
			// A target wasn't specified, return to progressing state
			ToProgressingState ();
		}
	}

	public override void OnCollisionEnter (Collision collision) {
		if (collision.gameObject.CompareTag ("Door")) {
			
			Vector3 normal = collision.contacts[0].normal;
			if ((normal.x < 0 && cat.transform.localScale.x > 0) || (normal.x > 0 && cat.transform.localScale.x < 0) || (normal.z < 0)) {
				// Switch to progressing state if blocked by a door while approaching lure
				ToProgressingState ();
				cat.progressingState.OnCollisionEnter (collision);
			}

		}
	}

	public override void ToProgressingState () {
		_target = null;
		cat.currentState = cat.progressingState;
	}

	public override void ToExploringState () {
		// The exploring state can only be entered from the progressing state
	}

	public override void ToPanickingState () {
		_target = null;
		cat.currentState = cat.panickingState;
	}
}
