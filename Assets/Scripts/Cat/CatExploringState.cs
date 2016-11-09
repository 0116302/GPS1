using UnityEngine;
using System.Collections;

public abstract class CatExploringState : CatState {

	public float exploreDuration = 3.0f;

	public CatExploringState (Cat cat) : base (cat) {
		
	}
}
