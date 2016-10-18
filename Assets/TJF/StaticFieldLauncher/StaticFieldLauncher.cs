using UnityEngine;
using System.Collections;

public class StaticFieldLauncher : Defense {

	private float cooldown = 0.0f;
	public float cooldownDuration = 10.0f;

	GUIManager guiManager;
	public GameObject _staticField;
	// Use this for initialization
	void Start () {
		guiManager = GameObject.FindObjectOfType<GUIManager> ();
	}

	// Update is called once per frame
	void Update () {
		if (cooldown > 0.0f) {
			cooldown -= Time.deltaTime;

		} else {
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
		//Debug.Log (isLeft);
	}

	public override void OnHoverExit () {
		guiManager.cooldownDisplay.text = "";
	}

	public override void OnTrigger () {		
		if (cooldown <= 0.0f) {
			StaticFieldRegion sfr = _staticField.GetComponent<StaticFieldRegion> ();
			sfr.active = true;
			cooldown = 15.0f;
		}
	}
}
