using UnityEngine;
using System.Collections;

public class HydroPump : MonoBehaviour{

	public GameObject projectile;
	public Transform projectileSpawnPoint;

	public float cooldownWait;
	public float cooldown = 8f;


	void Update () {
		if(cooldownWait > 0)
		{
			cooldownWait -= Time.deltaTime;
		}
	}

	void OnMouseDown()
	{
		Debug.Log ("hit");
		if(cooldownWait <= 0)
		{
			Instantiate (projectile, projectileSpawnPoint.position, projectileSpawnPoint.rotation);
			cooldownWait = cooldown;
		}
	}
}