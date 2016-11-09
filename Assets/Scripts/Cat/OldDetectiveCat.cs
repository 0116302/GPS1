using UnityEngine;
using System.Collections;

public class OldDetectiveCat : Cat {

	protected override void AssignStates () {
		progressingState = new CatDetectiveProgressingState (this, false);
		exploringState = new CatDetectiveExploringState (this, false);
		panickingState = new CatDefaultPanickingState (this);
		luredState = new CatDefaultLuredState (this);
		currentState = progressingState;
	}
}
