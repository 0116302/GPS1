using UnityEngine;
using System.Collections;

public class Static : MonoBehaviour {
	float defaultWalkSpeed;
	public float staticDuration;
	Enemy enemy;
	// Use this for initialization
	void Start () {	
		enemy = gameObject.GetComponent<Enemy> ();	
		defaultWalkSpeed = enemy.walkSpeed;
		enemy.walkSpeed = 0.0f;
	}
	
	// Update is called once per frame
	void Update () {
		if (staticDuration > 0.0f) {
			staticDuration -= Time.deltaTime;
		} else {
			staticDuration = 0;
			enemy.walkSpeed = defaultWalkSpeed;
			Destroy (GetComponent<Static>());
		}
	}
}
