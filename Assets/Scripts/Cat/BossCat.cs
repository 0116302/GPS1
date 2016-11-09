using UnityEngine;
using System.Collections;

public class BossCat : Cat {

	protected override void AssignStates () {
		progressingState = new CatBossProgressingState (this);
		exploringState = new CatDefaultExploringState (this);
		panickingState = new CatDefaultPanickingState (this);
		luredState = new CatDefaultLuredState (this);
		currentState = progressingState;
	}
}
