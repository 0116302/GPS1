using UnityEngine;
using System.Collections;

public class LightBoomDamage : MonoBehaviour {

	public float damage;
	public float lifetime;

	void Start()
	{
		Destroy (gameObject, lifetime);
	}

	void OnTriggerEnter(Collider collision)
	{
		Destructible hit = collision.gameObject.GetComponent<Destructible> ();
		if (hit != null) {
			hit.Damage (damage);
		}
		Destroy (gameObject);
	}
}