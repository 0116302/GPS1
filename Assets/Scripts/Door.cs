using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Animator))]
public class Door : MonoBehaviour, ITriggerable {

	public bool altDirection = false;
	public bool openByDefault = true;
	public bool playerControlled = true;

	private bool _isOpen = false;
	public bool isOpen {
		get { return _isOpen; }
	}

	private float cooldown = 0.0f;
	public float closeDuration = 10.0f;
	public float cooldownDuration = 30.0f;

	Animator animator;

	GUIManager guiManager;

	void Start () {
		animator = GetComponent<Animator> ();
		animator.SetBool ("altDirection", altDirection);

		guiManager = GameObject.FindObjectOfType<GUIManager> ();

		if (openByDefault) Open ();
	}

	void Update () {
		if (cooldown > 0.0f) {
			cooldown -= Time.deltaTime;

			if (cooldown <= cooldownDuration - closeDuration) {
				Open ();
			}

		} else {
			cooldown = 0.0f;
		}
	}

	public void OnHoverEnter () {
		
	}

	public void OnHoverStay () {
		if (cooldown > 0.0f) {
			guiManager.cooldownDisplay.text = "Cooldown: " + Mathf.CeilToInt (cooldown) + "s";

		} else {
			guiManager.cooldownDisplay.text = "";
		}
	}

	public void OnHoverExit () {
		guiManager.cooldownDisplay.text = "";
	}

	public void OnTrigger () {
		if (playerControlled && cooldown <= 0.0f) {
			Close ();
			cooldown = cooldownDuration;
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

	public void Toggle() {
		if (_isOpen) {
			Close ();
		} else {
			Open ();
		}
	}
}
