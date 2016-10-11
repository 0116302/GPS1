using UnityEngine;
using System.Collections;

public class HydroPump : Defense{

	public GameObject projectile;
	public Transform projectileSpawnPoint;

	private float cooldown = 0.0f;
	public float cooldownDuration = 10.0f;

	GUIManager guiManager;
	CameraController cameraController;
	private bool isLeft = true;
	private bool isPlaced = false;
	private bool isRotateLeft = false, isRotateRight = true;

	// Use this for initialization
	void Start () {
		guiManager = GameObject.FindObjectOfType<GUIManager> ();
		cameraController = GameObject.FindObjectOfType<CameraController> ();
//		bool isPlaced = false;
	}

	// Update is called once per frame
	void Update () {
		if (cooldown > 0.0f) {
			cooldown -= Time.deltaTime;

		} else {
			cooldown = 0.0f;
		}
		if(cameraController.isInPlacementMode == true && isPlaced == false){
			isLeft = cameraController.isOnLeft;
			if (isLeft == true && isRotateLeft == false && isRotateRight == true) {
				this.transform.Rotate (0f,180f,0f);
				isRotateLeft = true;
				isRotateRight = false;
			}else if(isLeft == false && isRotateLeft == true && isRotateRight == false){
				this.transform.Rotate (0f,180f,0f);
				isRotateLeft = false;
				isRotateRight = true;
			}
		}
		if (isPlaced == false && cameraController.isInPlacementMode == false) {
			isPlaced = true;
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
			if (isLeft == true) {
				Debug.Log (isLeft);
				Rigidbody shot = ((GameObject)GameObject.Instantiate (projectile, projectileSpawnPoint.position, projectileSpawnPoint.rotation)).GetComponent<Rigidbody> ();
				shot.AddForce (projectileSpawnPoint.right * -20f, ForceMode.Impulse);

			} else if(isLeft == false){
				Debug.Log (isLeft);
				Rigidbody shot = ((GameObject)GameObject.Instantiate (projectile, projectileSpawnPoint.position, projectileSpawnPoint.rotation)).GetComponent<Rigidbody> ();
				shot.AddForce (-projectileSpawnPoint.right * 20f, ForceMode.Impulse);
			}


			cooldown = cooldownDuration;
		}
	}
}