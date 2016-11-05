using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class CatDefaultExploringState : CatExploringState {

	public float roomPadding = 1.0f;

	bool targetDetermined = false;
	Vector3 _targetPosition;

	bool transitioning = false;

	public CatDefaultExploringState (Cat cat) : base(cat) {

	}

	public override void Start () {
		
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
		if (targetDetermined && !transitioning) {
			// Play the idle/exploring animation
			cat.controller.StopMoving ();

			// After done exploring, transition to progressing state
			cat.StartCoroutine (DelayedTransitionCoroutine ());
		}
	}

	IEnumerator DelayedTransitionCoroutine () {
		if (transitioning) yield break;
		transitioning = true;

		yield return new WaitForSeconds (3.0f);
		ToProgressingState ();

		transitioning = false;
	}

	void DetermineTarget () {
		_currentRoom = cat.progressingState.currentRoom;
		if (_currentRoom == null) return;

		float roomX = _currentRoom.transform.position.x;
		float roomHalfWidth = _currentRoom.roomWidth / 2.0f;

		_targetPosition = cat.transform.position;
		_targetPosition.x = Random.Range (roomX - roomHalfWidth + roomPadding, roomX + roomHalfWidth - roomPadding);

		cat.controller.SetTarget (_targetPosition);

		targetDetermined = true;
	}

	public override void ToProgressingState () {
		Debug.Log ("Entered progressing state!");

		targetDetermined = false;
		cat.currentState = cat.progressingState;
	}
}
