using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(Animator))]
public class Door : MonoBehaviour, ITriggerable {

	public bool alternateDirection = false;
	public bool openByDefault = true;
	public bool playerControlled = true;

	private bool _isOpen = false;
	public bool isOpen {
		get { return _isOpen; }
	}

	private bool _isBroken = false;
	public bool isBroken {
		get {
			return _isBroken;
		}
	}

	public List<Door> openOnClose = new List<Door> ();

	[Header ("Staircase")]
	public bool isStaircase = false;
	public Room room;
	public Transform teleportPosition;
	public Door destination;

	Animator animator;

	void Awake () {
		animator = GetComponent<Animator> ();
		animator.SetBool ("altDirection", alternateDirection);

		if (openByDefault) Open ();
	}

	void Update () {
		
	}

	public void OnHoverEnter () {

	}

	public void OnHoverStay () {

	}

	public void OnHoverExit () {

	}

	public void OnTrigger () {
		if (playerControlled && !_isBroken && (!isStaircase || !destination.isBroken)) {
			Close ();

			foreach (Door door in openOnClose) {
				if (!door._isBroken && (!door.isStaircase || !door.destination.isBroken)) {
					door.Open ();

					if (door.isStaircase) {
						door.destination.Open ();
					}
				}
			}

			if (isStaircase) {
				destination.Close ();
			}
		}
	}

	public void Open () {
		animator.SetBool ("isOpen", true);
		_isOpen = true;
	}

	public void Close () {
		animator.SetBool ("isOpen", false);
		_isOpen = false;
	}

	public void Toggle () {
		if (_isOpen) {
			Close ();
		} else {
			Open ();
		}
	}

	public void BreakOpen () {
		animator.SetBool ("isOpen", true);
		_isOpen = true;
		_isBroken = true;

		if (isStaircase) {
			destination.Open ();
		}
	}
}
