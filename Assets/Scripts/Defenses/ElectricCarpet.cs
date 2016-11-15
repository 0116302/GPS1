using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ElectricCarpet : Defense, IMultiTargeter {

	public float damage = 1.0f;
	public float activeDuration = 10.0f;

	bool _isActive = false;
	float activeTime = 0.0f;

	public CooldownIndicator cooldownIndicator;
	private float cooldown = 0.0f;
	public float cooldownDuration = 10.0f;

	List<Cat> targets = new List<Cat> ();

	// Use this for initialization
	void Start () {
		
	}

	// Update is called once per frame
	void Update () {
		if (placeableParent != null && !placeableParent.placed) return;

		if (_isActive) {

			activeTime -= Time.deltaTime;
			if (activeTime <= 0.0f) {
				_isActive = false;
				activeTime = 0.0f;
			}

			if (isDisarmed) _isActive = false;
		}

		if (!isDisarmed) {
			
			if (cooldown > 0.0f) {
				cooldown -= Time.deltaTime;

				if (cooldownIndicator != null) {
					cooldownIndicator.value = cooldown / cooldownDuration;
				}

			} else {
				cooldown = 0.0f;

				if (cooldownIndicator != null) {
					cooldownIndicator.value = 0.0f;
				}
			}

		} else {
			if (cooldownIndicator != null) {
				cooldownIndicator.value = 1.0f;
			}
		}
	}

	public void AddTarget (Transform target) {
		Cat enemy = target.GetComponent<Cat> ();
		if (enemy == null) return;

		targets.Add (enemy);

		if (_isActive) {
			// Shock
			bool initial = enemy.flashOnDamage;
			enemy.flashOnDamage = false;
			enemy.Damage (damage, DamageType.Electricity);
			enemy.flashOnDamage = initial;
			enemy.AddStatusEffect (new CatElectrocutedStatusEffect (activeTime));
		}
	}

	public void RemoveTarget (Transform target) {
		targets.Remove (target.GetComponent<Cat> ());
	}

	public void Activate () {
		if (isDisarmed) return;

		_isActive = true;
		activeTime = activeDuration;

		foreach (Cat enemy in targets) {
			if (enemy == null)
				continue;

			// Shock
			bool initial = enemy.flashOnDamage;
			enemy.flashOnDamage = false;
			enemy.Damage (damage, DamageType.Electricity);
			enemy.flashOnDamage = initial;
			enemy.AddStatusEffect (new CatElectrocutedStatusEffect (activeTime));
		}
	}

	public override void OnTrigger () {
		if (cooldown <= 0.0f) {
			Activate ();

			cooldown = cooldownDuration;
		}
	}
}
