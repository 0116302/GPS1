using UnityEngine;
using System.Collections;

public class MiniRocketProjectile : MonoBehaviour {

	public GameObject explosion;

	public float damage = 2f;
	public float speed = 2f;
	//public float rotateSpeed = 2;

	private Rigidbody rb;
	public Transform target;

	void Start ()
	{
		rb = GetComponent<Rigidbody>();

		rb.velocity = transform.forward * speed;
		target = GameObject.FindGameObjectWithTag ("Player").transform;
	}

	void Update ()
	{
		transform.position += (target.position - transform.position).normalized * speed * Time.deltaTime;
		//transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation (target.position - transform.position), rotateSpeed * Time.deltaTime);
	}

	void OnCollisionEnter (Collision collision) {
		Destructible hit = collision.gameObject.GetComponent<Destructible> ();
		if (hit != null) {
			hit.Damage (damage);
		}
		Instantiate(explosion, transform.position, transform.rotation);
		Destroy (gameObject);
	}
}