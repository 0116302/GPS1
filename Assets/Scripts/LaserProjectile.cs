using UnityEngine;
using System.Collections;

public class LaserProjectile : MonoBehaviour
{
	public float speed = 15.0f;
	public float damage = 1.0f;

	void Update ()
	{
		transform.position += Vector3.down * Time.deltaTime * speed;
	}

	void OnCollisionEnter (Collision collision)
	{
		Destructible hit = collision.gameObject.GetComponent<Destructible> ();
		if (hit != null)
		{
			hit.Damage (damage);
		}
		Destroy (gameObject);
	}
}