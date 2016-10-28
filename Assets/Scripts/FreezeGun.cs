using UnityEngine;
using System.Collections;

[RequireComponent (typeof (LineRenderer))]
public class FreezeGun : Defense, ITargeter {

	public Transform laserOrigin;
	public float laserDistance = 16.0f;
	public float damage = 10.0f;

	private LineRenderer lineRenderer;

	private bool _isShooting = false;
	private Transform _target;
	public Transform target {
		get { return _target; }
	}

	//private Enemy _hitEnemy;

	public float stunDuration = 5.0f;

	public CooldownIndicator cooldownIndicator;
	private float cooldown = 0.0f;
	public float cooldownDuration = 15.0f;

	void Awake () {
		lineRenderer = GetComponent<LineRenderer> ();
	}

	// Update is called once per frame
	void Update () {
		if (placeableParent != null && !placeableParent.placed) return;

		if (!_isShooting) {
			if (_target != null) {
				Vector3 direction = transform.position - _target.position;
				float angle = Mathf.Atan2 (direction.y, direction.x) * Mathf.Rad2Deg;

				transform.rotation = Quaternion.Lerp (transform.rotation, Quaternion.Euler (0f, 0f, angle - 90f), 10f * Time.deltaTime);

			} else {
				transform.rotation = Quaternion.Lerp (transform.rotation, Quaternion.Euler (0f, 0f, 0f), 10f * Time.deltaTime);
			}

		} else {
//			if (_hitEnemy == null || _hitEnemy.health <= 0.0f) {
//				lineRenderer.enabled = false;
//			}
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

//			if (_isShooting && _hitEnemy != null) cooldown = cooldownDuration;
		}
	}

	IEnumerator Fire () {
		RaycastHit hit;
		int layerMask = (1 << LayerMask.NameToLayer ("Rooms")) | (1 << LayerMask.NameToLayer ("Room Walls")) | (1 << LayerMask.NameToLayer ("Enemies"));

		if (Physics.Raycast (laserOrigin.position, -laserOrigin.up, out hit, laserDistance, layerMask, QueryTriggerInteraction.Ignore)) {
			_isShooting = true;

			lineRenderer.SetPosition (0, laserOrigin.position);
			Vector3 laserEnd = hit.point;
			laserEnd.z = laserOrigin.position.z;
			laserEnd -= laserOrigin.up * 0.25f;
			lineRenderer.SetPosition (1, laserEnd);
			lineRenderer.enabled = true;

//			if (hit.collider.CompareTag ("Enemy")) {
//				_hitEnemy = hit.collider.GetComponent<Enemy> ();
//				_hitEnemy.canMove = false;
//
//				_hitEnemy.Damage (damage);
//
//				yield return new WaitForSeconds (stunDuration);
//
//				_hitEnemy.canMove = true; // ISSUE: IF AN ENEMY IS SHOT BY TWO FREEZE GUNS, THE EFFECT WEARS OFF AFTER THE FIRST GUN STOPS
//
//			}

			lineRenderer.enabled = false;

			_isShooting = false;
		}

		yield break;
	}
}
