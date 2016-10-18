using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class StaticFieldRegion : MonoBehaviour {
	private List<GameObject> _targets = new List<GameObject> ();
	public bool active = false;
	float haloDuration;
	Behaviour halo;
	// Use this for initialization
	void Start () {
		halo = (Behaviour)gameObject.GetComponent ("Halo");
	}
	
	// Update is called once per frame
	void Update () {
		if (active == true) {
			haloDuration = 1.0f;
			foreach (GameObject target in _targets) {
				Static _static = target.GetComponent<Static> ();
				if (_static == null) {
					target.AddComponent<Static> ();
					Static addedStatic = target.GetComponent<Static> ();
					addedStatic.staticDuration = 8.0f;
				}
				if (_static != null) {
					_static.staticDuration = 8.0f;
				}
			}
			active = false;
		}
		if (haloDuration > 0) {
			halo.enabled = true;
			haloDuration -= Time.deltaTime;
		} else {
			halo.enabled = false;
			haloDuration = 0.0f;
		}
	}

	void OnTriggerEnter(Collider collider){
		Destructible target = collider.gameObject.GetComponent<Destructible> ();
		if (target != null) {
			_targets.Add (collider.gameObject);
		}

	}

	void OnTriggerExit(Collider collider){
		Destructible target = collider.gameObject.GetComponent<Destructible> ();
		if (target != null) {
			_targets.Remove (collider.gameObject);
		}

	}
}
