using UnityEngine;
using System.Collections;

public class HydroProjectile : MonoBehaviour
{
	public float speed = 5f;

	void Update()
	{
		transform.Translate (Vector3.left * Time.deltaTime * speed);
	}

	void OnCollisionEnter (Collision collision) {
		Destructible hit = collision.gameObject.GetComponent<Destructible> ();
		if (hit != null) {
			collision.rigidbody.AddForce(-100f, 0f, 0f); // I think its wrong, because there is both direction. -100f is always left.
		}
		Destroy (gameObject);
	}
}