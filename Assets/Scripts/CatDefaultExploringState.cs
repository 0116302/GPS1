using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class CatDefaultExploringState : CatExploringState {

	public float roomPadding = 2.0f;

	bool targetDetermined = false;
	Vector3 _targetPosition;

	Coroutine transitioning = null;

	public CatDefaultExploringState (Cat cat) : base(cat) {

	}

	public override void Start () {
		
	}

	public override void Update () {
		if (targetDetermined) {

			if (cat.transform.position == _targetPosition) {
				
				if (transitioning == null) {
					// Done exploring, transition to progressing state
					transitioning = cat.StartCoroutine (DelayedTransitionCoroutine ());

					// Play the idle/exploring animation
					cat.animator.SetFloat ("movementSpeed", 0.0f);
				}

			} else {
				cat.transform.position = Vector3.MoveTowards (cat.transform.position, _targetPosition, cat.walkingSpeed * Time.deltaTime);

				// Flip cat to face the right direction and play the moving animation
				FaceTarget ();
				cat.animator.SetFloat ("movementSpeed", cat.walkingSpeed);
			}

		} else {
			DetermineTarget ();
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

	IEnumerator DelayedTransitionCoroutine () {
		yield return new WaitForSeconds (3.0f);
		transitioning = null;
		ToProgressingState ();
	}

	void DetermineTarget () {
		_currentRoom = cat.progressingState.currentRoom;
		if (_currentRoom == null) return;

		float roomX = _currentRoom.transform.position.x;
		float roomHalfWidth = _currentRoom.roomWidth / 2.0f;

		_targetPosition = cat.transform.position;
		_targetPosition.x = Random.Range (roomX - roomHalfWidth + roomPadding, roomX + roomHalfWidth - roomPadding);

		targetDetermined = true;
	}

	public override void ToProgressingState () {
		targetDetermined = false;
		cat.currentState = cat.progressingState;
	}
}
