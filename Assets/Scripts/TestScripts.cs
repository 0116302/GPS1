using UnityEngine;
using System.Collections;

public class TestScript : MonoBehaviour
{
	public float speed;
	public float damage = 2.0f;

	public GameObject explosion;
	private GameObject closestTarget;
	private GameObject[] enemyArray;

	private Vector3 distanceDifference;
	private Vector3 rocketPosition;

	private float currentDistance;
	private float oldDistance;

	private Rigidbody rb;


	void Awake ()
	{
		oldDistance = Mathf.Infinity;

		rocketPosition = GameObject.FindGameObjectWithTag("Rocket").transform.position; // Find ("Player")

		enemyArray = GameObject.FindGameObjectsWithTag("Player"); // Find ("Hazard")

		for (int i = 0; i < enemyArray.Length; i++)
		{
			distanceDifference = enemyArray[i].transform.position - rocketPosition;

			currentDistance = distanceDifference.sqrMagnitude;

			if (currentDistance < oldDistance && rocketPosition.z < enemyArray[i].transform.position.z - 5)
			{
				closestTarget = enemyArray[i];

				oldDistance = currentDistance;
			}
		}
	}

	void Start ()
	{
		rb = GetComponent<Rigidbody>();
	}

	void FixedUpdate()
	{
		if (closestTarget == false)
		{
			rb.velocity = transform.forward * speed;
		}
		else if (closestTarget == true)
		{
			transform.LookAt(closestTarget.transform.position);

			transform.position = Vector3.MoveTowards(transform.position, closestTarget.transform.position, Time.deltaTime * speed);
		}
	}

	void OnCollisionEnter (Collision collision)
	{
		Destructible hit = collision.gameObject.GetComponent<Destructible> ();
		if (hit != null)
		{
			hit.Damage (damage);
		}
		Instantiate(explosion, transform.position, transform.rotation);
		Destroy (gameObject);
	}
}