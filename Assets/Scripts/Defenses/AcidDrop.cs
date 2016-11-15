using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AcidDrop :  Defense {

	public Rigidbody payload;

	public CooldownIndicator cooldownIndicator;
	bool used = false;

	// Use this for initialization
	void Start () {
		
	}

	public void Activate () {
		if (_isDisarmed) return;

		// Drop the payload
		payload.isKinematic = false;
		payload.useGravity = true;
	}

	public override void Disarm () {
		_isDisarmed = true;
		if (cooldownIndicator != null) {
			cooldownIndicator.value = 1.0f;
		}
	}

	public override void OnTrigger () {
		if (!used) {
			Activate ();

			used = true;
			if (cooldownIndicator != null) {
				cooldownIndicator.value = 1.0f;
			}
		}
	}
}
