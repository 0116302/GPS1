using UnityEngine;
using System.Collections;

public class DetectiveCat : Cat {

	protected override void AssignStates () {
		progressingState = new CatDetectiveProgressingState (this, true);
		exploringState = new CatDetectiveExploringState (this, true);
		panickingState = new CatDefaultPanickingState (this);
		luredState = new CatDefaultLuredState (this);
		currentState = progressingState;
	}
}
