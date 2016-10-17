using UnityEngine;
using System.Collections;

public class MiniRocketDamage : MonoBehaviour
{
	public float damage = 1.0f;

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