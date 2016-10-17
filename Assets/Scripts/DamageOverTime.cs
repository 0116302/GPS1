using UnityEngine;
using System.Collections;

public class DamageOverTime : MonoBehaviour
{
	public float damage;
	public float lifetime;
	public float totalDamage;
	public float intervalDuration;

	float amountDamaged = 0.0f;

	void Start ()
	{
		Destroy (gameObject, lifetime);
	}

	IEnumerator DamageOverTimeCoroutine(Collider collision)
	{
		Destructible hit = collision.gameObject.GetComponent<Destructible> ();
		while (hit != null && amountDamaged < totalDamage)
		{
			hit.Damage (damage);
			amountDamaged++;
			yield return new WaitForSeconds(intervalDuration);
			// Physics.IgnoreCollision(collision.collider, GetComponent<Collider>());
		}
	}

	void OnTriggerEnter (Collider collision)
	{
		StartCoroutine (DamageOverTimeCoroutine(collision));
	}

	// void OnTriggerStay - Reset amountDamaged
}