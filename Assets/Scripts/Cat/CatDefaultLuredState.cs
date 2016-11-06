using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class CatDefaultLuredState : CatLuredState {

	public CatDefaultLuredState (Cat cat) : base(cat) {

	}

	public override void Lure (Lure target) {
		if (target != _target && target.isActive) {
			int layerMask = 1 << LayerMask.NameToLayer ("Rooms");

			if (!Physics.Linecast (cat.transform.position, target.transform.position, layerMask)) {
				// There is a direct line of sight between the cat and the lure

				// Lure successful, switch the cat to this state
				cat.currentState.ToLuredState ();

				// Set the new target
				_target = target;
				_targetPosition = cat.transform.position;
				_targetPosition.x = _target.transform.position.x; //TODO Randomize the target position slightly
				_targetPosition.z = cat.zPosition;
				cat.controller.SetTarget (_targetPosition);

				// Start moving in the target's direction
				cat.controller.UnlockXZ ();
				cat.controller.movementSpeed = cat.walkingSpeed;
				cat.controller.StartMoving ();
			}
		}
	}

	public override void Start () {
		
	}

	public override void Update () {
		
	}

	public override void OnTargetReached () {
		// Start a coroutine to stay while the lure is active
		cat.StartCoroutine (StayAtTargetCoroutine ());
		Debug.Log ("Staying at lure!");
	}

	IEnumerator StayAtTargetCoroutine () {
		
		while (_target.isActive) {

			yield return new WaitForSeconds (1.0f);
		}

		Debug.Log ("Moving from lure!");
		ToProgressingState ();
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
		Debug.Log ("Entered progressing state!");

		_target = null;
		cat.currentState = cat.progressingState;
	}

	public override void ToExploringState () {
		Debug.Log ("Entered exploring state!");

		_target = null;
		cat.currentState = cat.exploringState;
	}
}
