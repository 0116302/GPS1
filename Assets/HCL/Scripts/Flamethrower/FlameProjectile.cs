using UnityEngine;
using System.Collections;

public class FlameProjectile : MonoBehaviour
{

	public float speed;
	public Vector3 direction;
	public GameObject explosion;


	void Update () 
	{
		this.transform.Translate (Vector3.down * Time.deltaTime * speed);
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