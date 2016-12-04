using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Fire : MonoBehaviour {

	public float burnDuration = 4.0f;

	HashSet<Cat> burnt = new HashSet<Cat> ();

	void Update () {

	}

	void OnTriggerStay (Collider other) {
		if (enabled && other.CompareTag ("Enemy")) {
			Cat enemy = other.GetComponent<Cat> ();

			if (!burnt.Contains (enemy) && enemy.HasStatusEffect<CatBurningStatusEffect> () == 0) {
				enemy.AddStatusEffect (new CatBurningStatusEffect (burnDuration));
				burnt.Add (enemy);
			}
		}
	}

	void OnTriggerExit (Collider other) {
		if (enabled && other.CompareTag ("Enemy")) {
			Cat enemy = other.GetComponent<Cat> ();
			burnt.Remove (enemy);
		}
	}
}
