using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Animator))]
public class Door : MonoBehaviour {

	public bool altDirection = false;

	private bool _isOpen = false;
	public bool isOpen {
		get { return _isOpen; }
	}

	Animator animator;

	void Start () {
		animator = GetComponent<Animator> ();
		animator.SetBool ("altDirection", altDirection);
	}

	void OnMouseDown () {
		Toggle ();
	}

	public void Open () {
		animator.SetBool ("isOpen", true);
		_isOpen = true;
	}

	public void Close () {
		animator.SetBool ("isOpen", false);
		_isOpen = false;
	}

	public void Toggle() {
		if (_isOpen) {
			Close ();
		} else {
			Open ();
		}
	}
}
