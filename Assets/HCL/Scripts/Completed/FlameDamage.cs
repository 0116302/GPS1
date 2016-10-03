using UnityEngine;
using System.Collections;

public class FlameDamage : MonoBehaviour {

	public float damage = 1f;
	public float lifetime;

	void Start()
	{
		Destroy (gameObject, lifetime); // Fire lifetime
	}

	IEnumerator OnCollisionEnter (Collision collision) {

		float amountDamaged = 0f;
		float totalDamage = 4f;

		Destructible hit = collision.gameObject.GetComponent<Destructible> ();
		while (hit != null && amountDamaged < totalDamage)
		{
			amountDamaged++;
			hit.Damage (damage); // Damage over time. HINT: Coroutine.
			yield return new WaitForSeconds(1f);
		}
	}
}