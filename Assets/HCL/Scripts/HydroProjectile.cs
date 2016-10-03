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
		collision.rigidbody.AddForce(-100f, 0f, 0f);
		Destroy (gameObject);
	}
}