using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
	public float speed;
	public int hp = 50;
	public new CameraFollow camera;

	Vector3 velocity;


	void Update()
	{
		velocity.x = Input.GetAxis ("Horizontal");
		this.transform.Translate (velocity * Time.deltaTime * speed);

		Death();
	}


	void Death()
	{
		if(hp <= 0)
		{
			camera.enabled = false;
			Destroy (this.gameObject); 
			SceneManager.LoadSceneAsync(0);
		}
	}
}