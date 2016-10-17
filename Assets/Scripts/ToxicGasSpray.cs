using UnityEngine;
using System.Collections;

public class ToxicGasSpray : Defense
{
	public GameObject projectile;
	public Transform projectileSpawnPoint;

	public override void OnTrigger ()
	{
		Instantiate (projectile, projectileSpawnPoint.position, projectileSpawnPoint.rotation);
		Destroy (gameObject);
	}
}