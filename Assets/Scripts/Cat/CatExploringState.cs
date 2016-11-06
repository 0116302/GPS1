using UnityEngine;
using System.Collections;

public abstract class CatExploringState : CatState {

	public float exploreDuration = 3.0f;

	protected Room _currentRoom = null;
	public Room currentRoom {
		get { return _currentRoom; }
	}

	public CatExploringState (Cat cat) : base (cat) {
		
	}
}
