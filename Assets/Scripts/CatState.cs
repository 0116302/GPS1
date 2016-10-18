using UnityEngine;
using System.Collections;

public abstract class CatState {

	protected Cat cat;

	public CatState (Cat cat) {
		this.cat = cat;
	}

	// Events

	public virtual void Start () {

	}

	public virtual void Update () {
		
	}

	public virtual void FixedUpdate () {

	}

	public virtual void OnCollisionEnter (Collision collision) {

	}

	public virtual void OnCollisionExit (Collision collision) {

	}

	// Transitions

	public virtual void ToProgressingState () {
		cat.currentState = cat.progressingState;
	}

	public virtual void ToExploringState () {
		cat.currentState = cat.exploringState;
	}

	public virtual void ToPanickedState () {
		cat.currentState = cat.panickingState;
	}

	public virtual void ToLuredState () {
		cat.currentState = cat.luredState;
	}

}
