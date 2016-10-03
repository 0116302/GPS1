using UnityEngine;
using System.Collections;

public class ZombiePlant : MonoBehaviour {
	bool deactive = false;
	public float rearmTime;
	private float rearmTimer;
	public GameObject prefab;
	Vector3 position = new Vector3(3.91f,0.658f,0);//use to spawn some cat

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (deactive) {
			rearmTimer -= Time.deltaTime;
			if (rearmTimer < 0) {
				deactive = false;
				this.GetComponent<SpriteRenderer>().color = Color.white;
			}
		}
		if(Input.GetKeyDown(KeyCode.Alpha1)){//this is to spawn cat
			Instantiate (prefab,position, Quaternion.identity);
		}
	}

	void OnTriggerEnter(Collider Other){
		if(Other.CompareTag("Enemy") && deactive == false){
			Other.GetComponent<Enemy> ().health -= Other.GetComponent<Enemy> ().maximumHealth;
			deactive = true;
			this.GetComponent<SpriteRenderer>().color = Color.red;
			rearmTimer = rearmTime;
		}
	}

	void OnTriggerStay(Collider Other){
		if(Other.CompareTag("Enemy") && deactive == false){
			Other.GetComponent<Enemy> ().health -= Other.GetComponent<Enemy> ().maximumHealth;
			deactive = true;
			this.GetComponent<SpriteRenderer>().color = Color.red;
			rearmTimer = rearmTime;
		}
	}
}
