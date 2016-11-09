using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Hologram : Lure, IMultiTargeter {

	public float activeDuration = 10.0f;

	public SpriteRenderer projection; //TODO Replace this with actual animations

	public CooldownIndicator cooldownIndicator;
	private float cooldown = 0.0f;
	public float cooldownDuration = 10.0f;

	public int maxUses = 5;
	int uses = 0;

	List<Cat> targets = new List<Cat> ();

	// Use this for initialization
	void Start () {
		projection.enabled = false;
	}

	// Update is called once per frame
	void Update () {
		if (placeableParent != null && !placeableParent.placed) return;

		if (_isActive && disarmed) {
			_isActive = false;
			projection.enabled = false;
		}

		if (!disarmed || uses >= maxUses) {
			
			if (cooldown > 0.0f) {
				cooldown -= Time.deltaTime;

				if (cooldownIndicator != null) {
					//cooldownIndicator.value = cooldown / cooldownDuration;
					cooldownIndicator.value = 1.0f;
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
			enemy.luredState.Lure (this);
		}
	}

	public void RemoveTarget (Transform target) {
		targets.Remove (target.GetComponent<Cat> ());
	}

	public void Activate () {
		if (disarmed || uses >= maxUses) return;

		_isActive = true;
		uses++;
		projection.enabled = true;
		StartCoroutine (DeactivateCoroutine ());

		foreach (Cat enemy in targets) {
			if (enemy == null)
				continue;

			enemy.luredState.Lure (this);
		}
	}

	IEnumerator DeactivateCoroutine () {
		yield return new WaitForSeconds (activeDuration);
		_isActive = false;
		projection.enabled = false;
	}

	public override void OnTrigger () {
		if (cooldown <= 0.0f) {
			Activate ();

			cooldown = cooldownDuration;
		}
	}
}
