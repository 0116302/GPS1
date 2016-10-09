using UnityEngine;
using System.Collections;

public interface IPlaceable {

	uint gridSnapX { get; }
	uint gridSnapY { get; }
	Vector3 placementOffset { get; }
	bool CanBePlacedHere (int x, int y, int gridWidth, int gridHeight);
	void OnPlace ();
}
