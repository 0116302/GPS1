using UnityEngine;
using System.Collections;

public class ElectricDamage : MonoBehaviour
{
	public float damage = 1.0f;
	public float lifetime = 2.0f;

	void Start ()
	{
		Destroy(gameObject, lifetime);
	}

	void OnTriggerEnter (Collider collision)
	{
		Destructible hit = collision.gameObject.GetComponent<Destructible> ();
		if (hit != null)
		{
			hit.Damage (damage);
		}
	}
}