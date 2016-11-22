﻿using UnityEngine;
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
