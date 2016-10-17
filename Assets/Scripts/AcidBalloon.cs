using UnityEngine;
using System.Collections;

public class AcidBalloon : Defense
{
	public GameObject explosion;

	float speed = 5.0f;

	bool interacted;

	void Update ()
	{
		if (interacted)
		{
		DropFromCeiling ();
		}
	}

	public override void OnTrigger ()
	{
		interacted = true;
	}

	void DropFromCeiling ()
	{
		transform.position += Vector3.down * Time.deltaTime * speed;
	}

	void OnCollisionEnter (Collision collision)
	{
			Instantiate(explosion, transform.position, transform.rotation);
			Destroy (gameObject);
	}
}