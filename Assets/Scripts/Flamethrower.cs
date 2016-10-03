using UnityEngine;
using System.Collections;

public class Flamethrower : Defense, ITargeter {

	public GameObject projectile;
	public Transform projectileSpawnPoint;

	private Transform _target;
	public Transform target {
		get { return _target; }
	}

	// Use this for initialization
	void Start () {
	
	}

	// Update is called once per frame
	void Update () {
		if (_target != null) {
			Vector3 direction = transform.position - _target.position;
			float angle = Mathf.Atan2 (direction.y, direction.x) * Mathf.Rad2Deg;

			transform.rotation = Quaternion.Lerp (transform.rotation, Quaternion.Euler (0f, 0f, angle - 90f), 10f * Time.deltaTime);

		} else {
			transform.rotation = Quaternion.Lerp (transform.rotation, Quaternion.Euler (0f, 0f, 0f), 10f * Time.deltaTime);
		}
	}

	public void SetTarget(Transform target) {
		_target = target;
	}

	public override void OnHoverEnter() {

	}

	public override void OnHoverExit() {

	}

	public override void OnTrigger() {
		Rigidbody shot = ((GameObject) GameObject.Instantiate (projectile, projectileSpawnPoint.position, projectileSpawnPoint.rotation)).GetComponent<Rigidbody> ();
		shot.AddForce (-projectileSpawnPoint.up * 2f, ForceMode.Impulse);
	}
}
