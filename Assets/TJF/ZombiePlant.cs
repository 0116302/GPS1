using UnityEngine;
using System.Collections;

public class ZombiePlant : Defense {
	bool deactive = false;

	// Use this for initialization
	private float cooldown = 0.0f;
	public float cooldownDuration = 10.0f;

	GUIManager guiManager;

	// Use this for initialization
	void Start () {
		guiManager = GameObject.FindObjectOfType<GUIManager> ();
	}

	// Update is called once per frame
	void Update () {
		if (cooldown > 0.0f) {
			cooldown -= Time.deltaTime;
			Debug.Log (cooldown);

		} else {
			deactive = false;
			this.GetComponent<SpriteRenderer>().color = Color.white;
			cooldown = 0.0f;
		}
	}

	public override void OnHoverEnter () {

	}

	public override void OnHoverStay () {
		if (cooldown > 0.0f) {
			guiManager.cooldownDisplay.text = "Cooldown: " + Mathf.CeilToInt (cooldown) + "s";

		} else {
			guiManager.cooldownDisplay.text = "";
		}
	}

	public override void OnHoverExit () {
		guiManager.cooldownDisplay.text = "";
	}

	public override void OnTrigger () {
	}

	void OnTriggerEnter(Collider collision){
		if (deactive != true) {
			Destructible hit = collision.gameObject.GetComponent<Destructible> ();
			if (hit != null) {
				hit.Damage (hit.maximumHealth);
				deactive = true;
				this.GetComponent<SpriteRenderer>().color = Color.red;
				cooldown = cooldownDuration;
			}
		}
	}

	void OnTriggerStay(Collider collision){
		if (deactive != true) {
			Destructible hit = collision.gameObject.GetComponent<Destructible> ();
			if (hit != null) {
				hit.Damage (hit.maximumHealth);
				deactive = true;
				this.GetComponent<SpriteRenderer>().color = Color.red;
				cooldown = cooldownDuration;
			}
		}
	}
}