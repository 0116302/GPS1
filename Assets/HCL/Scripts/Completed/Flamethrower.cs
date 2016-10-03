using UnityEngine;
using System.Collections;

public class Flamethrower : MonoBehaviour, ITargeter {

	public GameObject projectile;
	public Transform projectileSpawnPoint;

	public float cooldownWait;
	public float cooldown = 10f;

	private Transform _target;
	public Transform target {
		get { return _target; }
	}

	void Update () {
		if (_target != null) {
			Vector3 direction = transform.position - _target.position;
			float angle = Mathf.Atan2 (direction.y, direction.x) * Mathf.Rad2Deg;

			transform.rotation = Quaternion.Lerp (transform.rotation, Quaternion.Euler (0f, 0f, angle - 90f), 10f * Time.deltaTime);
		} 
		else
		{
			transform.rotation = Quaternion.Lerp (transform.rotation, Quaternion.Euler (0f, 0f, 0f), 10f * Time.deltaTime);
		}

		if(cooldownWait > 0)
		{
			cooldownWait -= Time.deltaTime;
		}
	}

	public void SetTarget(Transform target) {
		_target = target;
	}

	void OnMouseDown()
	{
		if(cooldownWait <= 0)
		{
			Instantiate (projectile, projectileSpawnPoint.position, projectileSpawnPoint.rotation);
			cooldownWait = cooldown;
		}
	}
}