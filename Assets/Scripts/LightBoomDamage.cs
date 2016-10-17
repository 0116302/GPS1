using UnityEngine;
using System.Collections;

public class LightBoomDamage : MonoBehaviour
{
	public float damage = 2.0f;

	void OnTriggerEnter (Collider collision)
	{
		Destructible hit = collision.gameObject.GetComponent<Destructible> ();
		if (hit != null)
		{
			hit.Damage (damage);
			Destroy (gameObject);
		}
	}
}