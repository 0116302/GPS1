using UnityEngine;
using System.Collections;

public abstract class Lure : Defense {

	public float minDistance = 0.5f;
	public float maxDistance = 1.0f;

	protected bool _isActive = false;
	public bool isActive {
		get {
			return _isActive;
		}
	}

}
