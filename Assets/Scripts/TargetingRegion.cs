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
		if (_targeter != null && other.CompareTag(targetTag)) {
			// Change target if targeter is currently idle or new target is closer
			if (_targeter.target == null || (targeterObject.transform.position - other.transform.position).sqrMagnitude < (targeterObject.transform.position - _targeter.target.position).sqrMagnitude) {
				_targeter.SetTarget (other.transform);
			}
		}
	}

	void OnTriggerExit (Collider other) {
		if (_targeter != null && other.CompareTag(targetTag) && _targeter.target == other.transform) {
			_targeter.SetTarget (null);
		}
	}
}
