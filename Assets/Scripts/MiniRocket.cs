using UnityEngine;
using System.Collections;

public class MiniRocket : MonoBehaviour, ITargeter
{
	public float speed = 3.0f;
	public float damage = 2.0f;

	public Transform rocketSmoke;

	private Transform _target;
	public Transform target
	{
		get { return _target; }
	}

	public GameObject explosion;

	void Update ()
	{
		if (_target != null)
		{
			transform.position += (_target.position - transform.position).normalized * speed * Time.deltaTime;

//			Vector3 direction = transform.position - _target.position;
//			float angle = Mathf.Atan2 (direction.y, direction.x) * Mathf.Rad2Deg;
//
//			transform.rotation = Quaternion.Lerp (transform.rotation, Quaternion.Euler (0f, 0f, angle), 10f * Time.deltaTime);

			ParticleSystem effect = rocketSmoke.GetComponent<ParticleSystem> ();
			if (effect != null) effect.Play ();
		}
	}

	public void SetTarget (Transform target)
	{
		_target = target;
	}

	void OnCollisionEnter (Collision collision)
	{
		Destructible hit = collision.gameObject.GetComponent<Destructible> ();
		if (hit != null)
		{
			hit.Damage (damage);
		}
		Instantiate(explosion, transform.position, transform.rotation);
		Destroy (gameObject);
	}
}