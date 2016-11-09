using UnityEngine;
using System.Collections;

public class Placeable : MonoBehaviour {

	[Header ("Gameplay Settings")]
	public int cost = 1000;
	public bool removable = true;
	public bool refundable = true;

	[Header ("Placement Parameters")]
	public int width = 1;
	public int height = 1;

	[Space (10)]
	public int horizontalSnap = 1;
	public int verticalSnap = 1;

	[Space (10)]
	public SpriteRenderer highlight;
	public Color positiveHighlight = new Color (0.0f, 1.0f, 0.0f);
	public Color negativeHighlight = new Color (1.0f, 0.0f, 0.0f);
	public float transitionDuration = 0.2f;
	private Coroutine transition = null;

	[Space (10)]
	public Vector3 placementOffset = new Vector3 (0.0f, 0.0f, -0.0001f);

	[Space (10)]
	public bool placeableInMidAir = true;
	public bool placeableOnFloors = true;
	public bool placeableOnCeilings = true;
	public bool placeableOnWalls = true;

	private bool _placed = false;
	public bool placed {
		get { return _placed; }
		set { _placed = value; }
	}

	private Transform placementGrid;
	private int _conflictCount = 0;

	void OnEnable () {
		_conflictCount = 0;
	}

	public virtual bool CanBePlacedHere (int x, int y, int gridWidth, int gridHeight) {

		if (_conflictCount == 0) {
			if (x == 0) {
				// X = Left
				if (y == 0) {
					// Y = Ceiling
					if (placeableOnWalls || placeableOnCeilings)
						return true;

				} else if (y < 0 && y > -(gridHeight - height)) {
					// Y = Middle
					if (placeableOnWalls)
						return true;

				} else if (y == -(gridHeight - height)) {
					// Y = Floor
					if (placeableOnWalls || placeableOnFloors)
						return true;
				}


			} else if (x > 0 && x < gridWidth - width) {
				// X = Middle
				if (y == 0) {
					// Y = Ceiling
					if (placeableOnCeilings)
						return true;

				} else if (y < 0 && y > -(gridHeight - height)) {
					// Y = Middle
					if (placeableInMidAir)
						return true;

				} else if (y == -(gridHeight - height)) {
					// Y = Floor
					if (placeableOnFloors)
						return true;
				}


			} else if (x == gridWidth - width) {
				// X = Right
				if (y == 0) {
					// Y = Ceiling
					if (placeableOnWalls || placeableOnCeilings)
						return true;

				} else if (y < 0 && y > -(gridHeight - height)) {
					// Y = Middle
					if (placeableOnWalls)
						return true;

				} else if (y == -(gridHeight - height)) {
					// Y = Floor
					if (placeableOnWalls || placeableOnFloors)
						return true;
				}
			}
		}

		return false;
	}

	public virtual void OnPlace () {
		// Do nothing by default
	}

	public virtual void OnRemove () {
		// Do nothing by default
	}

	void OnTriggerEnter (Collider other) {
		_conflictCount++;
	}

	void OnTriggerExit (Collider other) {
		_conflictCount--;
	}

	public void HighlightPositive () {
		if (transition != null) StopCoroutine (transition);

		if (highlight.color != positiveHighlight)
			transition = StartCoroutine (ChangeHighlightColor (highlight.color, positiveHighlight, transitionDuration));
	}

	public void HighlightNegative () {
		if (transition != null) StopCoroutine (transition);

		if (highlight.color != negativeHighlight)
			transition = StartCoroutine (ChangeHighlightColor (highlight.color, negativeHighlight, transitionDuration));
	}

	public void HideHighlight () {
		if (transition != null) StopCoroutine (transition);

		Color hidden = new Color (0.0f, 0.0f, 0.0f, 0.0f);
		if (highlight.color != hidden)
			transition = StartCoroutine (ChangeHighlightColor (highlight.color, hidden, transitionDuration));
	}

	IEnumerator ChangeHighlightColor (Color sourceColor, Color destinationColor, float duration) {
		float elapsedTime = 0.0f;
		YieldInstruction waitForEndOfFrame = new WaitForEndOfFrame ();

		while (elapsedTime < duration) {
			highlight.color = Color.Lerp (sourceColor, destinationColor, (elapsedTime / duration));

			elapsedTime += Time.deltaTime;
			yield return waitForEndOfFrame;
		}
		highlight.color = destinationColor;

		yield break;
	}
}
