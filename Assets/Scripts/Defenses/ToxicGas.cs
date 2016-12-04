using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ToxicGas :  Defense {

	public float activeDuration = 4.0f;
	float activeTime = 0.0f;

	public float damagePerTick = 0.5f;
	public float tickFrequency = 1.0f;

	public new ParticleSystem particleSystem;
	SoundEffect sound;

	public CooldownIndicator cooldownIndicator;
	bool used = false;

	void Awake () {
		sound = GetComponent<SoundEffect> ();
	}

	public void Activate () {
		if (_isDisarmed) return;

		if (sound != null) sound.Play (0);

		activeTime = activeDuration;
		StartCoroutine (Damage ());
	}

	IEnumerator Damage () {
		particleSystem.Play ();

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
