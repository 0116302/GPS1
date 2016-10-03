using UnityEngine;
using System.Collections;

public class LightBoom : MonoBehaviour
{
	public GameObject lightBoom;
	public GameObject explosion;
	public float lifetime;

	void OnMouseDown()
	{
		Invoke("Explosion", lifetime);
		Destroy (gameObject, lifetime);
	}

	void Explosion()
	{
		Instantiate(explosion, transform.position, transform.rotation);
	}
}