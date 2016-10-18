using UnityEngine;
using System.Collections;

public abstract class CatProgressingState : CatState {

	private Room _currentRoom = null;
	public Room currentRoom {
		get { return _currentRoom; }
	}

	public CatProgressingState (Cat cat) : base (cat) {
		
	}
}
