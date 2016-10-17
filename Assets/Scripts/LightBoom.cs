using UnityEngine;
using System.Collections;

public class LightBoom : Defense
{
	public GameObject explosion;

	float lifetime = 2.0f;

	public override void OnTrigger ()
	{
		Invoke("Explosion", lifetime);
		Destroy (gameObject, lifetime);
	}

	void Explosion ()
	{
		Instantiate(explosion, transform.position, transform.rotation);
	}
}