using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class Player : Destructible
{
	public float speed;
	public new CameraFollow camera;

	Vector3 velocity;

	void Start ()
	{
		onDeath.AddListener (OnDeath);
	}

	void Update()
	{
		velocity.x = Input.GetAxis ("Horizontal");
		transform.Translate (velocity * Time.deltaTime * speed);
	}
		
	void OnDeath()
	{
		camera.enabled = false;
		Destroy (gameObject);
		Scene scene = SceneManager.GetActiveScene ();
		SceneManager.LoadScene (scene.name);
	}
}