using UnityEngine;
using System.Collections;

public abstract class Defense : MonoBehaviour, ITriggerable {

	public Placeable placeableParent;

	protected bool _isDisarmed = false;
	public bool isDisarmed {
		get {
			return _isDisarmed;
		}
	}

	public virtual void OnHoverEnter () {

	}

	public virtual void OnHoverStay () {

	}

	public virtual void OnHoverExit () {

	}

	public virtual void OnTrigger () {
		
	}

	public virtual void Disarm () {
		_isDisarmed = true;
	}
}
