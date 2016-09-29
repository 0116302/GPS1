﻿using UnityEngine;
using System.Collections;

public class Enemy : Destructible {

	Animator animator;

	[Header("Death")]
	public Transform head;
	public GameObject decapitatedHeadPrefab;

	// Use this for initialization
	void Start () {
		animator = GetComponent<Animator> ();

		onDeath.AddListener (OnDeath);
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 pos = transform.position;
		//pos.z = 0.0f;
		transform.position = pos;
	}

	void OnCollisionEnter(Collision collision) {
		if (collision.gameObject.CompareTag("Door")) {
			// Change direction upon hitting locked door
			Vector3 newScale = transform.localScale;
			newScale.x = -newScale.x;
			transform.localScale = newScale;
		}
	}

	void OnDeath () {
		Rigidbody decapitatedHead = ((GameObject) GameObject.Instantiate (decapitatedHeadPrefab, head.transform.position, head.transform.rotation)).GetComponent<Rigidbody> ();
		decapitatedHead.AddForce (new Vector3(-0.5f, 2f, 0f), ForceMode.Impulse);
		Destroy (gameObject);
	}
}
