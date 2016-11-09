using UnityEngine;
using System.Collections;

public class Flamethrower : Defense, ITargeter {

	public Transform flame;
	Fire damageRegion;
	new ParticleSystem particleSystem;

	private Transform _target;
	public Transform target {
		get { return _target; }
	}

	public float fireDuration = 4.0f;

	public CooldownIndicator cooldownIndicator;
	private float cooldown = 0.0f;
	public float cooldownDuration = 10.0f;

	// Use this for initialization
	void Awake () {
		damageRegion = flame.GetComponent<Fire> ();
		particleSystem = flame.GetComponent<ParticleSystem> ();
	}

	// Update is called once per frame
	void Update () {
		if (placeableParent != null && !placeableParent.placed) return;

		if (_target != null) {
			Vector3 direction = transform.position - _target.position;
			float angle = Mathf.Atan2 (direction.y, direction.x) * Mathf.Rad2Deg;

			transform.rotation = Quaternion.Lerp (transform.rotation, Quaternion.Euler (0f, 0f, angle - 90f), 10f * Time.deltaTime);

		} else {
			transform.rotation = Quaternion.Lerp (transform.rotation, Quaternion.Euler (0f, 0f, 0f), 10f * Time.deltaTime);
		}

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
	}

	public void SetTarget (Transform target) {
		_target = target;
	}

	public override void OnTrigger () {
		if (cooldown <= 0.0f) {
			StartCoroutine (Fire ());

			cooldown = cooldownDuration;
		}
	}

	IEnumerator Fire () {
		if (damageRegion != null) damageRegion.enabled = true;
		if (particleSystem != null) particleSystem.Play ();

		yield return new WaitForSeconds (fireDuration);

		if (damageRegion != null) damageRegion.enabled = false;
		if (particleSystem != null) particleSystem.Stop ();
	}
}
