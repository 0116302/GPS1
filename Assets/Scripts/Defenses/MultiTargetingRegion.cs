using UnityEngine;
using System.Collections;

public class MultiTargetingRegion : MonoBehaviour {

	public string targetTag = "Enemy";
	public GameObject multiTargeterObject;
	private IMultiTargeter _targeter;

	void Start () {
		_targeter = multiTargeterObject.GetComponent<IMultiTargeter> ();
	}

	void OnTriggerEnter (Collider other) {
		if (_targeter != null && other.CompareTag(targetTag)) {
			// New target available
			_targeter.AddTarget (other.transform);
		}
	}

	void OnTriggerExit (Collider other) {
		if (_targeter != null && other.CompareTag (targetTag)) {
			// A target exits the region
			_targeter.RemoveTarget (other.transform);
		}
	}
}
