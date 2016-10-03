using UnityEngine;
using System.Collections;

public class FlameProjectile : MonoBehaviour
{
	public float speed = 5f;
	public GameObject explosion;

	void Update() 
	{
		transform.Translate (Vector3.down * Time.deltaTime * speed);
	}

	void OnCollisionEnter (Collision collision) {
		Instantiate(explosion, transform.position, transform.rotation);
		Destroy (gameObject);
	}
}