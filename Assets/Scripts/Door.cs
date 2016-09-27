using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Animator))]
public class Door : MonoBehaviour, ITriggerable {

	public bool altDirection = false;
	public bool openByDefault = true;

	private bool _isOpen = false;
	public bool isOpen {
		get { return _isOpen; }
	}

	Animator animator;

	void Start () {
		animator = GetComponent<Animator> ();
		animator.SetBool ("altDirection", altDirection);

		if (openByDefault) Open ();
	}

	public void OnHoverEnter() {

	}

	public void OnHoverExit () {

	}

	public void OnTrigger () {
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
