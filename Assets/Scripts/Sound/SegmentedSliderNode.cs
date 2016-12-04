using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[RequireComponent (typeof (Button))]
public class SegmentedSliderNode : MonoBehaviour {

	SegmentedSlider parent;

	public float value = 1.0f;

	void Awake () {
		parent = transform.parent.GetComponent<SegmentedSlider> ();
	}

	public void OnMouseEnter () {
		if (parent.mouseDown) {
			parent.UpdateValue (value);
		}
	}

	public void OnMouseDown () {
		parent.OnMouseDown ();
	}

	public void OnMouseClick () {
		parent.UpdateValue (value);
	}

	public void SetActive (bool value) {
		GetComponent<Button> ().interactable = value;
	}
}
