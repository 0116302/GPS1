using UnityEngine;
using System.Collections;

public class VenomArrow : MonoBehaviour
{
	public float speed = 10.0f;
	public float damage;
	public float totalDamage;
	public float intervalDuration;

	float amountDamaged = 0.0f;
	float lifetime = 21.0f;

	void Update ()
	{
		transform.position += Vector3.down * Time.deltaTime * speed;
		//transform.Translate (Vector3.right * Time.deltaTime * speed);
	}

	IEnumerator DamageOverTimeCoroutine (Collision collision)
	{
		Destructible hit = collision.gameObject.GetComponent<Destructible> ();
		while (hit != null && amountDamaged < totalDamage)
		{
			yield return new WaitForSeconds(intervalDuration);
			hit.Damage (damage);
			amountDamaged++;
		}
	}

	void OnCollisionEnter (Collision collision)
	{
		StartCoroutine (DamageOverTimeCoroutine(collision));
		Destroy(GetComponent<SpriteRenderer>());
		Destroy(GetComponent<SphereCollider>());
		Destroy(gameObject, lifetime);
	}
}