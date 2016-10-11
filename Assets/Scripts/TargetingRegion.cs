using UnityEngine;
using System.Collections;

public class TargetingRegion : MonoBehaviour {

	public string targetTag = "Enemy";
	public GameObject targeterObject;
	private ITargeter _targeter;

	void Start () {
		_targeter = targeterObject.GetComponent<ITargeter> ();
	}

	void OnTriggerStay (Collider other) {
		if (_targeter != null && other.CompareTag(targetTag) && _targeter.target == null) {
			_targeter.SetTarget (other.transform);
		}
	}

	void OnTriggerExit (Collider other) {
		if (_targeter != null && other.CompareTag(targetTag) && _targeter.target == other.transform) {
			_targeter.SetTarget (null);
		}
	}
}
