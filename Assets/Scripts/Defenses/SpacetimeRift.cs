using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpacetimeRift :  Defense, IMultiTargeter {

	List<Cat> targets = new List<Cat> ();

	public CooldownIndicator cooldownIndicator;
	private float cooldown = 0.0f;
	public float cooldownDuration = 180.0f;

	// Use this for initialization
	void Start () {
		
	}

	// Update is called once per frame
	void Update () {
		if (placeableParent != null && !placeableParent.placed) return;

		if (!_isDisarmed) {

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
	}

	public void RemoveTarget (Transform target) {
		targets.Remove (target.GetComponent<Cat> ());
	}

	public void Activate () {
		if (_isDisarmed) return;

		for (int i = targets.Count - 1; i >= 0; i--) {
			Cat enemy = targets [i];

			if (enemy == null)
				continue;

			targets.RemoveAt (i);
			StartCoroutine (DestroyCatCoroutine (enemy));
		}
	}

	IEnumerator DestroyCatCoroutine (Cat cat) {
		cat.controller.frozen = true;
		cat.SetTint (Color.black);

		yield return cat.StartCoroutine (cat.Fade (0.0f, 0.25f));

		cat.Destroy ();
	}

	public override void OnTrigger () {
		if (cooldown <= 0.0f) {
			Activate ();

			cooldown = cooldownDuration;
		}
	}
}
