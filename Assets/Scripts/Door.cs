using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(Animator))]
public class Door : MonoBehaviour, ITriggerable {

	public bool alternateDirection = false;
	public bool openByDefault = true;
	public bool playerControlled = true;

	public CooldownIndicator cooldownIndicator;
	SoundEffect sound;

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
		sound = GetComponent<SoundEffect> ();

		animator.SetBool ("altDirection", alternateDirection);

		if (openByDefault) {
			animator.SetBool ("isOpen", true);
			_isOpen = true;
		}
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
		if (!_isOpen) {
			animator.SetBool ("isOpen", true);
			_isOpen = true;
			sound.Play (0);
		}
	}

	public void Close () {
		if (isOpen) {
			animator.SetBool ("isOpen", false);
			_isOpen = false;
			sound.Play (1);
		}
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

		if (cooldownIndicator != null)
			cooldownIndicator.value = 1.0f;

		if (isStaircase) {
			destination.Open ();

			if (destination.cooldownIndicator != null)
				destination.cooldownIndicator.value = 1.0f;
		}
	}
}
