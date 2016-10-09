using UnityEngine;
using System.Collections;

public class Flamethrower : Defense, ITargeter {

	public GameObject projectile;
	public Transform projectileSpawnPoint;

	private Transform _target;
	public Transform target {
		get { return _target; }
	}

	private float cooldown = 0.0f;
	public float cooldownDuration = 10.0f;

	GUIManager guiManager;

	// Use this for initialization
	void Start () {
		guiManager = GameObject.FindObjectOfType<GUIManager> ();
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

		if (cooldown > 0.0f) {
			cooldown -= Time.deltaTime;

		} else {
			cooldown = 0.0f;
		}
	}

	public void SetTarget (Transform target) {
		_target = target;
	}

	public override void OnHoverEnter () {

	}

	public override void OnHoverStay () {
		if (cooldown > 0.0f) {
			guiManager.cooldownDisplay.text = "Cooldown: " + Mathf.CeilToInt (cooldown) + "s";

		} else {
			guiManager.cooldownDisplay.text = "";
		}
	}

	public override void OnHoverExit () {
		guiManager.cooldownDisplay.text = "";
	}

	public override void OnTrigger () {
		if (cooldown <= 0.0f) {
			Rigidbody shot = ((GameObject) GameObject.Instantiate (projectile, projectileSpawnPoint.position, projectileSpawnPoint.rotation)).GetComponent<Rigidbody> ();
			shot.AddForce (-projectileSpawnPoint.up * 2f, ForceMode.Impulse);

			cooldown = cooldownDuration;
		}
	}
}
