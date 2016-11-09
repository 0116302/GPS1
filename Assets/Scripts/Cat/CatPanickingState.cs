using UnityEngine;
using System.Collections;

public abstract class CatPanickingState : CatState {

	protected Door _target;
	public Door target {
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

	protected float _timeLeft;

	public CatPanickingState (Cat cat) : base (cat) {
		
	}

	public abstract void Panic (float duration);
}
