using UnityEngine;
using System.Collections;

public class FlamethrowerScript : MonoBehaviour
{
	public Transform shotPosition;
	public Transform projectile;
	public Transform target;
	public float range;


	void Start()
	{
		target = GameObject.FindGameObjectWithTag ("Player").transform;
	}


	void Update()
	{
		float dist = (Vector3.Distance(this.transform.position, target.transform.position));
		if (dist < range)
		{
			Vector3 direction = this.transform.position - target.position;
			direction.Normalize ();

			float angle = Mathf.Atan2 (direction.y, direction.x) * Mathf.Rad2Deg;

			this.transform.rotation = Quaternion.Lerp (this.transform.rotation, Quaternion.Euler (0f, 0f, angle - 90f), Time.deltaTime * 3f);
		}
		Stop();
	}


	void OnMouseDown()
	{
		Instantiate (projectile, shotPosition.position, shotPosition.rotation);
	}

	void Stop()
	{
		if(Player.hp == 0)
		{
			gameObject.SetActive(false);
		}
	}
}