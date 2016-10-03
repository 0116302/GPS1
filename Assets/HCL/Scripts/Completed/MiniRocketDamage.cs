using UnityEngine;
using System.Collections;

public class MiniRocketDamage : MonoBehaviour {

	public float damage = 1f;
	public float lifetime;

	void Start()
	{
		Destroy (gameObject, lifetime);
	}

	void OnCollisionEnter(Collision collision)
	{
		Destructible hit = collision.gameObject.GetComponent<Destructible> ();
		if (hit != null) {
			hit.Damage (damage);
		}
		Destroy (gameObject);
	}
}