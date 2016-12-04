using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public class SegmentedSlider : MonoBehaviour {

	public SegmentedSliderNode[] nodes;

	[Space (10)]
	public float defaultValue = 1.0f;

	private float _value = 1.0f;
	public float value {
		get {
			return _value;
		}

		set {
			UpdateValue (value);
		}
	}

	int activeNodeCount = 1;

	[Space (10)]
	public UnityEvent onValueChanged;

	private bool _mouseDown = false;
	public bool mouseDown {
		get {
			return _mouseDown;
		}
	}

	void Start () {
		for (int i = 0; i < nodes.Length; i++) {
			nodes [i].value = 1.0f / nodes.Length * (i + 1);
		}
		activeNodeCount = nodes.Length;
	}

	void Update () {
		if (_mouseDown && Input.GetMouseButtonUp (0)) {
			_mouseDown = false;

			onValueChanged.Invoke ();
		}
	}

	public void OnMouseDown () {
		_mouseDown = true;
	}

	public void UpdateValue (float value) {
		this._value = Mathf.Clamp (value, 0.0f, 1.0f);

		activeNodeCount = Mathf.RoundToInt (this._value * nodes.Length);
		for (int i = 0; i < nodes.Length; i++) {
			nodes [i].SetActive (i < activeNodeCount);
		}
	}

	public void Increment () {
		activeNodeCount = Mathf.Clamp (activeNodeCount + 1, 0, nodes.Length);
		UpdateValue (activeNodeCount * 1.0f / nodes.Length);

		onValueChanged.Invoke ();
	}

	public void Decrement () {
		activeNodeCount = Mathf.Clamp (activeNodeCount - 1, 0, nodes.Length);
		UpdateValue (activeNodeCount * 1.0f / nodes.Length);

		onValueChanged.Invoke ();
	}
}
