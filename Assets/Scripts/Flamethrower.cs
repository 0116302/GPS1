using UnityEngine;
using System.Collections;

public class Flamethrower : Defense, ITargeter {

	public Transform flame;

	private Transform _target;
	public Transform target {
		get { return _target; }
	}

	public float fireDuration = 5.0f;

	public CooldownIndicator cooldownIndicator;
	private float cooldown = 0.0f;
	public float cooldownDuration = 10.0f;

	// Use this for initialization
	void Start () {
		
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
				cooldownIndicator.cooldownValue = cooldown / cooldownDuration;
			}

		} else {
			cooldown = 0.0f;

			if (cooldownIndicator != null) {
				cooldownIndicator.cooldownValue = 0.0f;
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
		DamagingRegion damage = flame.GetComponent<DamagingRegion> ();
		if (damage != null) damage.enabled = true;

		ParticleSystem effect = flame.GetComponent<ParticleSystem> ();
		if (effect != null) effect.Play ();

		yield return new WaitForSeconds (fireDuration);

		if (damage != null) damage.enabled = false;
		if (effect != null) effect.Stop ();
	}
}
