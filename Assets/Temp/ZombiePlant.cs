using UnityEngine;
using System.Collections;

public class ZombiePlant : MonoBehaviour {
	bool deactive = false;
	public float rearmTime;
	private float rearmTimer;
	public GameObject prefab;
	Vector3 position = new Vector3(3.91f,0.658f,0);
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (deactive) {
			rearmTimer -= Time.deltaTime;
			if (rearmTimer < 0) {
				deactive = false;
				this.GetComponent<SpriteRenderer>().color = Color.blue;
			}
		}
		if(Input.GetKeyDown(KeyCode.Alpha1)){ 
			Instantiate (prefab,position, Quaternion.identity);
		}
	}

	void OnTriggerEnter(Collider Other){
		if(Other.CompareTag("Cat") && deactive == false){
			Other.GetComponent<Cat> ().hp -= Other.GetComponent<Cat> ().hp;
			deactive = true;
			this.GetComponent<SpriteRenderer>().color = Color.red;
			rearmTimer = rearmTime;
		}
	}
}
