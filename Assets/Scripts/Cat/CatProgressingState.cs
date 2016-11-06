using UnityEngine;
using System.Collections;

public abstract class CatProgressingState : CatState {

	protected Room _currentRoom = null;
	public Room currentRoom {
		get { return _currentRoom; }
	}

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

	public CatProgressingState (Cat cat) : base (cat) {
		
	}
}
