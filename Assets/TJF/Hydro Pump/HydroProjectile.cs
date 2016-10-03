using UnityEngine;
using System.Collections;

public class HydroProjectile : MonoBehaviour
{
	public float speed = 5f;

	void Update()
	{
		transform.Translate (Vector3.left * Time.deltaTime * speed);
		if (transform.position.z >= 0) {
			transform.Translate (new Vector3(0,0,-1f) * Time.deltaTime * speed);
		}
	}

	void OnCollisionEnter (Collision collision) {
		if (collision.rigidbody != null) {
			collision.rigidbody.AddForce(-100f, 0f, 0f);
		}
		Destroy (gameObject);
	}
}