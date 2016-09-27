using UnityEngine;
using System.Collections;

public class Cat : MonoBehaviour {
	
	public float speed;
	public int hp = 3;
	Vector3 velocity;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		velocity.x = Input.GetAxis ("Horizontal");
		this.transform.Translate (Vector3.right * Time.deltaTime * speed);
		Death ();
	}

	void Death(){
		if (hp <= 0) {
			Destroy (gameObject);
		}
	}
}
