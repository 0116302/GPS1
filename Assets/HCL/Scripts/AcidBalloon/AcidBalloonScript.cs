using UnityEngine;
using System.Collections;

public class AcidBalloonScript : MonoBehaviour
{
	public Transform respawnAB;
	public GameObject explosion;
	public GameObject acidBalloon;
	public new Rigidbody rigidbody;


	void Start()
	{
		rigidbody = GetComponent<Rigidbody>();
	}


	void OnMouseDown()
	{
		rigidbody.useGravity = true;
	}


	void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag ("Player") || other.CompareTag ("Untagged"))
		{
			Instantiate(explosion, other.transform.position, other.transform.rotation);
			Destroy (this.gameObject);
		}
	}
}