using UnityEngine;
using System.Collections;

[RequireComponent (typeof (LineRenderer))]
public class Laser : Defense, ITargeter {

	public Transform laserOrigin;
	public float laserDistance = 16.0f;
	public float damage = 10.0f;

	private LineRenderer lineRenderer;

	private Transform _target;
	public Transform target {
		get { return _target; }
	}

	public float laserDuration = 0.1f;
	private float cooldown = 0.0f;
	public float cooldownDuration = 5.0f;

	GUIManager guiManager;

	void Awake () {
		guiManager = GameObject.FindObjectOfType<GUIManager> ();
		lineRenderer = GetComponent<LineRenderer> ();
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
			StartCoroutine (Fire ());

			cooldown = cooldownDuration;
		}
	}

	IEnumerator Fire () {
		RaycastHit hit;
		int layerMask = 1 | (1 << 10) | (1 << 11);

		if (Physics.Raycast (laserOrigin.position, -laserOrigin.up, out hit, laserDistance, layerMask, QueryTriggerInteraction.Ignore)) {
			lineRenderer.SetPosition (0, laserOrigin.position);
			lineRenderer.SetPosition (1, hit.point + (-laserOrigin.up * 0.25f));
			lineRenderer.enabled = true;

			if (hit.collider.CompareTag ("Enemy")) {
				hit.collider.GetComponent<Enemy> ().Damage(damage);
			}

			yield return new WaitForSeconds (laserDuration);

			lineRenderer.enabled = false;
		}

		yield break;
	}
}
