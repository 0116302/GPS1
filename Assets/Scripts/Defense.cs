using UnityEngine;
using System.Collections;

public abstract class Defense : MonoBehaviour, ITriggerable {

	public Placeable placeableParent;

	public virtual void OnHoverEnter () {

	}

	public virtual void OnHoverStay () {

	}

	public virtual void OnHoverExit () {

	}

	public virtual void OnTrigger () {
		
	}
}
