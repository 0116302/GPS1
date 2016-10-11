using UnityEngine;
using System.Collections;

public class Placeable : MonoBehaviour, IPlaceable {

	public int cost = 1000;

	public uint width = 1;
	public uint height = 1;

	public uint horizontalSnap = 1;
	public uint verticalSnap = 1;
	public Vector3 offset = new Vector3 (0.5f, -0.5f, -0.0001f);

	public bool placeMidAir = true;
	public bool placeOnFloor = true;
	public bool placeOnCeiling = true;
	public bool placeOnWalls = true;
	private bool isLeft = true;
	public bool isOnLeft{
		get {return isLeft; }
	}

	private Transform placementGrid;
	private int _conflictCount = 0;

	public uint gridSnapX {
		get { return horizontalSnap; }
	}

	public uint gridSnapY {
		get { return verticalSnap; }
	}

	public Vector3 placementOffset {
		get { return offset; }
	}

	public virtual bool CanBePlacedHere (int x, int y, int gridWidth, int gridHeight) {

		if (_conflictCount == 0) {
			if (x == 0) {
				// X = Left
				isLeft = true;
				if (y == 0) {
					// Y = Ceiling
					if (placeOnWalls || placeOnCeiling) return true;

				} else if (y < 0 && y > -(gridHeight - height)) {
					// Y = Middle
					if (placeOnWalls) return true;

				} else if (y == -(gridHeight - height)) {
					// Y = Floor
					if (placeOnWalls || placeOnFloor) return true;
				}


			} else if (x > 0 && x < gridWidth - width) {
				// X = Middle
				if (y == 0) {
					// Y = Ceiling
					if (placeOnCeiling) return true;

				} else if (y < 0 && y > -(gridHeight - height)) {
					// Y = Middle
					if (placeMidAir) return true;

				} else if (y == -(gridHeight - height)) {
					// Y = Floor
					if (placeOnFloor) return true;
				}


			} else if (x == gridWidth - width) {
				// X = Right
				isLeft = false;
				if (y == 0) {
					// Y = Ceiling
					if (placeOnWalls || placeOnCeiling) return true;

				} else if (y < 0 && y > -(gridHeight - height)) {
					// Y = Middle
					if (placeOnWalls) return true;

				} else if (y == -(gridHeight - height)) {
					// Y = Floor
					if (placeOnWalls || placeOnFloor) return true;
				}
			}
		}

		return false;
	}

	public virtual void OnPlace () {
		// Do nothing by default
	}

	void OnTriggerEnter (Collider other) {
		Debug.Log ("Conflict with " + other.gameObject.name);
		_conflictCount++;
	}

	void OnTriggerExit (Collider other) {
		_conflictCount--;
	}
}
