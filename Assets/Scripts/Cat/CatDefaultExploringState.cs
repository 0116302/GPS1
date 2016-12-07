using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class CatDefaultExploringState : CatExploringState {

	public float roomPadding = 1.0f;

	bool targetDetermined = false;
	Vector3 _targetPosition;

	bool _isTransitioning = false;

	Coroutine transitionCoroutine = null;

	public CatDefaultExploringState (Cat cat) : base(cat) {

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

			if (cat.currentRoom.roomName == "Gold Room" && LevelManager.instance.bonus1Received) {
				GameObject.Destroy (LevelManager.instance.bonusObjective1.gameObject);
				LevelManager.instance.bonus1Received = false;

				int r = Random.Range (1, 6);
				switch (r) {
				case 1:
					cat.Say ("Jackpot!");
					break;
				case 2:
					cat.Say ("We're gonna need a truck for these");
					break;
				case 3:
					cat.Say ("My precious...");
					break;
				case 4:
					cat.Say ("Smaug? You there?");
					break;
				case 5:
					cat.Say ("My wife's gonna love this");
					break;
				}

			} else if (cat.currentRoom.roomName == "Treasury" && LevelManager.instance.bonus2Received) {
				GameObject.Destroy (LevelManager.instance.bonusObjective2.gameObject);
				LevelManager.instance.bonus2Received = false;

				int r = Random.Range (1, 6);
				switch (r) {
				case 1:
					cat.Say ("This is why I love being a cop");
					break;
				case 2:
					cat.Say ("I'M RICH!");
					break;
				case 3:
					cat.Say ("*pockets cash discreetly*");
					break;
				case 4:
					cat.Say ("Is it payday already?");
					break;
				case 5:
					cat.Say ("I can finally pay off my student loans!");
					break;
				}

			} else if (cat.currentRoom.roomName == "Drug Room" && LevelManager.instance.bonus3Received) {
				GameObject.Destroy (LevelManager.instance.bonusObjective3.gameObject);
				LevelManager.instance.bonus3Received = false;

				int r = Random.Range (1, 6);
				switch (r) {
				case 1:
					cat.Say ("We've hit the motherload!");
					break;
				case 2:
					cat.Say ("I'm gonna try these just to make sure");
					break;
				case 3:
					cat.Say ("*sniff* *sniff*");
					break;
				case 4:
					cat.Say ("We'll be taking these");
					break;
				case 5:
					cat.Say ("Wow! How much is this stuff worth?");
					break;
				}
			}

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

	public void StopCoroutines () {
		if (_isTransitioning) {
			cat.StopCoroutine (transitionCoroutine);
			_isTransitioning = false;
		}
	}

	public override void ToProgressingState () {
		targetDetermined = false;
		StopCoroutines ();

		cat.currentState = cat.progressingState;
	}

	public override void ToPanickingState () {
		targetDetermined = false;
		StopCoroutines ();

		cat.currentState = cat.panickingState;
	}

	public override void ToLuredState () {
		targetDetermined = false;
		StopCoroutines ();

		cat.currentState = cat.luredState;
	}
}
