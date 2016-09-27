using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class Player : MonoBehaviour
{
	public float speed;
	public static int hp = 10;
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
			//SceneManager.LoadScene("GameScene"); // WTF NOT WORKING?
		}
	}
}