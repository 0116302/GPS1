using UnityEngine;
using System.Collections;

public class ToxicGasSpray : MonoBehaviour {

	public GameObject projectile;
	public Transform projectileSpawnPoint;

	void OnMouseDown()
	{
		Instantiate (projectile, projectileSpawnPoint.position, projectileSpawnPoint.rotation);
		Destroy (gameObject);
	}
}