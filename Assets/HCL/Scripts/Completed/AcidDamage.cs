using UnityEngine;
using System.Collections;

public class AcidDamage : MonoBehaviour {

	public float damage;
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
	}
}