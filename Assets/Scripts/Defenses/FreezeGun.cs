﻿using UnityEngine;
using System.Collections;

[RequireComponent (typeof (LineRenderer))]
public class FreezeGun : Defense, ITargeter {

	public Transform laserOrigin;
	public float laserDistance = 16.0f;
	public float damage = 1.0f;
	public float freezeDuration = 5.0f;

	private LineRenderer lineRenderer;
	SoundEffect sound;

	private Transform _target;
	public Transform target {
		get { return _target; }
	}

	public float laserDuration = 0.1f;

	public CooldownIndicator cooldownIndicator;
	private float cooldown = 0.0f;
	public float cooldownDuration = 15.0f;

	void Awake () {
		lineRenderer = GetComponent<LineRenderer> ();
		sound = GetComponent<SoundEffect> ();
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
		if (target == null) return;

		Cat cat = target.GetComponent<Cat> ();
		if (cat != null && cat.currentRoom == placeableParent.room) {
			_target = target;
		}
	}

	public override void OnTrigger () {
		if (cooldown <= 0.0f) {
			StartCoroutine (Fire ());

			cooldown = cooldownDuration;
		}
	}

	IEnumerator Fire () {
		RaycastHit hit;
		int layerMask = (1 << LayerMask.NameToLayer ("Rooms")) | (1 << LayerMask.NameToLayer ("Room Walls")) | (1 << LayerMask.NameToLayer ("Enemies"));

		if (Physics.Raycast (laserOrigin.position, -laserOrigin.up, out hit, laserDistance, layerMask, QueryTriggerInteraction.Ignore)) {
			lineRenderer.SetPosition (0, laserOrigin.position);
			Vector3 laserEnd = hit.point;
			laserEnd.z = laserOrigin.position.z;
			laserEnd -= laserOrigin.up * 0.25f;
			lineRenderer.SetPosition (1, laserEnd);
			lineRenderer.enabled = true;
			if (sound != null) sound.Play (0);

			if (hit.collider.CompareTag ("Enemy")) {
				Cat enemy = hit.collider.GetComponent<Cat> ();

				bool initial = enemy.flashOnDamage;
				enemy.flashOnDamage = false;

				enemy.Damage(damage, DamageType.Cold);
				enemy.AddStatusEffect (new CatFrozenStatusEffect (freezeDuration));

				enemy.flashOnDamage = initial;
			}

			yield return new WaitForSeconds (laserDuration);

			lineRenderer.enabled = false;
		}

		yield break;
	}
}
