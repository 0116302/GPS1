using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class CatDetectiveExploringState : CatExploringState {

	public float roomPadding = 1.0f;

	bool canBeLured = true;

	bool targetDetermined = false;
	Vector3 _targetPosition;

	bool _isDisarming = false;
	bool _isTransitioning = false;

	Coroutine transitionCoroutine = null;

	public CatDetectiveExploringState (Cat cat, bool canBeLured = true) : base(cat) {
		this.canBeLured = canBeLured;
	}

	public override void Update () {
		if (!targetDetermined) {
			DetermineTarget ();
			cat.controller.LockZ ();
			cat.controller.StartMoving ();
		}
	}

	public override void OnTargetReached ()
	{
		if (targetDetermined && !_isTransitioning) {
			// Play the idle/exploring animation
			cat.controller.StopMoving ();

			// After done exploring, transition to progressing state
			transitionCoroutine = cat.StartCoroutine (DelayedTransitionCoroutine ());
		}
	}

	IEnumerator DelayedTransitionCoroutine () {
		if (_isTransitioning) yield break;
		_isTransitioning = true;

		yield return new WaitForSeconds (exploreDuration);
		ToProgressingState ();

		_isTransitioning = false;
	}

	void DetermineTarget () {
		if (cat.currentRoom == null) return;

		float roomX = cat.currentRoom.transform.position.x;
		float roomHalfWidth = cat.currentRoom.roomWidth / 2.0f;

		_targetPosition = cat.transform.position;
		_targetPosition.x = Random.Range (roomX - roomHalfWidth + roomPadding, roomX + roomHalfWidth - roomPadding);

		cat.controller.SetTarget (_targetPosition);

		targetDetermined = true;
	}

	public override void OnTriggerEnter (Collider other) {
		if (other.CompareTag ("Defense") && !_isTransitioning) {
			Defense defense = other.GetComponentInChildren<Defense> ();

			if (defense != null && !defense.isDisarmed && (!canBeLured || !(defense is Lure))) {
				// Disarm trap
				cat.StartCoroutine (DisarmTrapCoroutine (defense, 1.0f));
			}

		}
	}

	IEnumerator DisarmTrapCoroutine (Defense defense, float duration) {
		if (_isDisarming || defense == null) yield break;
		_isDisarming = true;

		cat.controller.StopMoving ();
		cat.animator.SetTrigger ("disarm");

		yield return new WaitForSeconds (1.0f);

		defense.Disarm ();

		yield return new WaitForSeconds (duration);

		cat.controller.StartMoving ();

		_isDisarming = false;
	}

	public void StopCoroutines () {
		if (_isTransitioning) {
			cat.StopCoroutine (transitionCoroutine);
			_isTransitioning = false;
		}
	}

	public override void ToProgressingState () {
		Debug.Log ("Entered progressing state!");

		targetDetermined = false;
		StopCoroutines ();

		cat.currentState = cat.progressingState;
	}

	public override void ToPanickingState () {
		Debug.Log ("Entered panicking state!");

		targetDetermined = false;
		StopCoroutines ();

		cat.currentState = cat.panickingState;
	}

	public override void ToLuredState () {
		if (canBeLured) {
			Debug.Log ("Entered lured state!");

			targetDetermined = false;
			StopCoroutines ();

			cat.currentState = cat.luredState;
		}
	}
}
