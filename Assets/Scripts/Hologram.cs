using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Hologram : Lure, IMultiTargeter {

	List<Cat> targets = new List<Cat> ();

	public float activeDuration = 10.0f;

	public CooldownIndicator cooldownIndicator;
	private float cooldown = 0.0f;
	public float cooldownDuration = 10.0f;

	// Use this for initialization
	void Start () {
		placeableParent.placed = true;
	}

	// Update is called once per frame
	void Update () {
		if (placeableParent != null && !placeableParent.placed) return;

		if (cooldown > 0.0f) {
			cooldown -= Time.deltaTime;

			if (cooldownIndicator != null) {
				cooldownIndicator.cooldownValue = cooldown / cooldownDuration;
			}

		} else {
			cooldown = 0.0f;

			if (cooldownIndicator != null) {
				cooldownIndicator.cooldownValue = 0.0f;
			}
		}
	}

	public void AddTarget (Transform target) {
		Cat enemy = target.GetComponent<Cat> ();
		if (enemy == null)
			return;

		targets.Add (enemy);

		if (_isActive) {
			enemy.luredState.Lure (this);
		}
	}

	public void RemoveTarget (Transform target) {
		targets.Remove (target.GetComponent<Cat> ());
	}

	public void Activate () {
		_isActive = true;
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
	}

	public override void OnTrigger () {
		if (cooldown <= 0.0f) {
			Activate ();

			cooldown = cooldownDuration;
		}
	}
}
