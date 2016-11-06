using UnityEngine;
using System.Collections;

public abstract class Lure : Defense {

	protected bool _isActive = false;
	public bool isActive {
		get {
			return _isActive;
		}
	}

}
