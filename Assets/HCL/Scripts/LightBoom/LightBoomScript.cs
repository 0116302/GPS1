using UnityEngine;
using System.Collections;

public class LightBoomScript : MonoBehaviour
{
	public Transform respawnLB;
	public GameObject lightBoom;
	public GameObject explosion;
	public float lifetime;


	void OnMouseDown()
	{
		Invoke("Explosion", lifetime);
		Destroy (this.gameObject, lifetime);
	}


	void Explosion()
	{
		Instantiate(explosion, transform.position, transform.rotation);
	}
}