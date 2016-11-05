using UnityEngine;
using System.Collections;

public abstract class CatExploringState : CatState {

	protected Room _currentRoom = null;
	public Room currentRoom {
		get { return _currentRoom; }
	}

	public CatExploringState (Cat cat) : base (cat) {
		
	}
}
