using UnityEngine;
using System.Collections;

public abstract class Placeable : MonoBehaviour, IPlaceable {

	public virtual uint gridSnapX {
		get { return 1; }
	}

	public virtual uint gridSnapY {
		get { return 1; }
	}

	public virtual Vector3 placementOffset {
		get { return new Vector3 (0.5f, -0.5f, -0.0001f); }
	}

	public virtual bool CanBePlacedHere (Room room) {
		// Perform no checking by default
		return true;
	}

	public virtual void OnPlace () {
		// Do nothing by default
	}
}
