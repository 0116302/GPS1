using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ToxicGas :  Defense {

	public float activeDuration = 4.0f;
	float activeTime = 0.0f;

	public float damagePerTick = 0.1f;
	public float tickFrequency = 1.0f;

	public new ParticleSystem particleSystem;
	public CooldownIndicator cooldownIndicator;
	bool used = false;

	// Use this for initialization
	void Start () {
		
	}

	public void Activate () {
		if (_isDisarmed) return;

		activeTime = activeDuration;
		StartCoroutine (Damage ());
	}

	IEnumerator Damage () {
		particleSystem.Play ();
		Debug.Log ("Particles... AWAY!");

		while (activeTime > 0.0f) {

			foreach (Cat enemy in placeableParent.room.occupants) {
				enemy.Damage (damagePerTick, DamageType.Poison);
			}

			activeTime -= 1.0f / tickFrequency;
			yield return new WaitForSeconds (1.0f / tickFrequency);
		}

		particleSystem.Stop ();
	}

	public override void Disarm () {
		_isDisarmed = true;
		StopAllCoroutines ();

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
