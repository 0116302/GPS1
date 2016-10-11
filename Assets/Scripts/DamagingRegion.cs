using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DamagingRegion : MonoBehaviour {

	public float damageOnEntry = 0.0f;
	public float damagePerTick = 10f;
	public float ticksPerSecond = 1.0f;
	public float damageOnExit = 0.0f;

	private List<Destructible> targets = new List<Destructible> ();

	void Start () {
		StartCoroutine (DamageTick ());
	}

	void OnTriggerEnter (Collider other) {
		Destructible target = other.GetComponent<Destructible> ();
		if (target != null) {
			target.Damage (damageOnEntry);
			targets.Add (target);
		}
	}

	void OnTriggerExit (Collider other) {
		Destructible target = other.GetComponent<Destructible> ();
		if (target != null) {
			target.Damage (damageOnExit);
			targets.Remove (target);
		}
	}

	IEnumerator DamageTick () {
		while (true) {
			if (enabled) {
				foreach (Destructible target in targets) {
					target.Damage (damagePerTick);
				}
			}

			yield return new WaitForSeconds (1.0f / ticksPerSecond);
		}
	}
}
