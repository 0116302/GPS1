using UnityEngine;
using System.Collections;

public abstract class CatLuredState : CatState {

	protected Lure _target;
	public Lure target {
		get {
			return _target;
		}
	}

	protected Vector3 _targetPosition;
	public Vector3 targetPosition {
		get {
			return _targetPosition;
		}
	}

	public CatLuredState (Cat cat) : base (cat) {
		
	}

	public abstract void Lure (Lure target);
}
