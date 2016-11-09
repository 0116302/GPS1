using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Animator))]
public class Door : MonoBehaviour, ITriggerable {

	public bool alternateDirection = false;
	public bool openByDefault = true;
	public bool playerControlled = true;

	private bool _isOpen = false;
	public bool isOpen {
		get { return _isOpen; }
	}

	public bool _isBroken = false;
	public bool isBroken {
		get {
			return _isBroken;
		}
	}

	[Header ("Cooldown")]
	public CooldownIndicator cooldownIndicator;
	private float cooldown = 0.0f;
	public float closeDuration = 10.0f;
	public float cooldownDuration = 30.0f;

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
		if (cooldown > 0.0f) {
			cooldown -= Time.deltaTime;

			if (cooldownIndicator != null) {
				cooldownIndicator.value = cooldown / cooldownDuration;
			}

			if (cooldown <= cooldownDuration - closeDuration) {
				Open ();
			}

		} else {
			cooldown = 0.0f;

			if (cooldownIndicator != null) {
				cooldownIndicator.value = 0.0f;
			}
		}
	}

	public void OnHoverEnter () {

	}

	public void OnHoverStay () {

	}

	public void OnHoverExit () {

	}

	public void OnTrigger () {
		if (playerControlled && !_isBroken && cooldown <= 0.0f) {
			if (!isStaircase || !destination._isBroken) {
				Close ();
				cooldown = cooldownDuration;

				if (isStaircase) {
					destination.OnTrigger ();
				}
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
