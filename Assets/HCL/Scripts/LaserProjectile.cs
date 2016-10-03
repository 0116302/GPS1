using UnityEngine;
using System.Collections;

public class LaserProjectile : MonoBehaviour
{
	public float speed = 15f;
	public float damage = 1f;

	void Update() 
	{
		transform.Translate (Vector3.down * Time.deltaTime * speed);
	}

	void OnCollisionEnter (Collision collision) {
		Destructible hit = collision.gameObject.GetComponent<Destructible> ();
		if (hit != null) {
			hit.Damage (damage);
		}
		Destroy (gameObject);
	}
}