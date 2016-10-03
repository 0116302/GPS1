using UnityEngine;
using System.Collections;

public class MiniRocket : MonoBehaviour {

	public GameObject projectile;
	public Transform projectileSpawnPoint;

	public float cooldownWait;
	public float cooldown = 15f;

	void Update()
	{
		if(cooldownWait > 0)
		{
			cooldownWait -= Time.deltaTime;
		}
	}

	void OnMouseDown()
	{
		if(cooldownWait <= 0)
		{
			Instantiate (projectile, projectileSpawnPoint.position, projectileSpawnPoint.rotation);
			cooldownWait = cooldown;
		}
	}
}