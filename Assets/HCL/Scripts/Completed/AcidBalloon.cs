using UnityEngine;
using System.Collections;

public class AcidBalloon : MonoBehaviour
{
	public GameObject explosion;
	public GameObject acidBalloon;
	Rigidbody rb;

	void Start()
	{
		rb = GetComponent<Rigidbody>();
	}
		
	void OnMouseDown()
	{
		rb.useGravity = true;
	}

	void OnCollisionEnter (Collision collision) {
		Instantiate(explosion, transform.position, transform.rotation);
		Destroy (gameObject);
	}
}