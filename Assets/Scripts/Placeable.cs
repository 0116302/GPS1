using UnityEngine;
using System.Collections;

public class Placeable : MonoBehaviour, IPlaceable {

	public uint horizontalSnap = 1;
	public uint verticalSnap = 1;
	public Vector3 offset = new Vector3 (0.5f, -0.5f, -0.0001f);

	public uint gridSnapX {
		get { return horizontalSnap; }
	}

	public uint gridSnapY {
		get { return verticalSnap; }
	}

	public Vector3 placementOffset {
		get { return offset; }
	}

	public virtual bool CanBePlacedHere (Room room) {
		// Perform no checking by default
		return true;
	}

	public virtual void OnPlace () {
		// Do nothing by default
	}
}
