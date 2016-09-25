using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class AcidBalloonScript : MonoBehaviour
{
	public Transform respawnAB;
	public GameObject acidBalloon;
	public new Rigidbody rigidbody;
	public GameObject explosion;

	public Text myText;
	public float duration;
	public bool displayInfo;


	void Start() 
	{
		rigidbody = GetComponent<Rigidbody>();

		myText = GameObject.Find("AcidBalloonInfoText").GetComponent<Text>();
		myText.color = Color.clear;
	}


	void Update()
	{
		if(Input.GetKeyDown(KeyCode.R)) // respawn for testing purpose but if destroy, cannot spawn anymore (NEED HELP~)
		{
			Instantiate (acidBalloon, respawnAB.position, respawnAB.rotation);
		}
		ShowInfo();
	}


	void OnMouseDown()
	{
		rigidbody.useGravity = true;
	}


	void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag ("Player") || other.CompareTag ("Untagged"))
		{
			Instantiate(explosion, other.transform.position, other.transform.rotation);
			Destroy (this.gameObject);
		}
	}

	void OnMouseOver()
	{
		displayInfo = true;
	}

	void OnMouseExit()
	{
		displayInfo = false;
	}

	void ShowInfo()
	{
		if(displayInfo)
		{
			myText.color = Color.Lerp (myText.color, Color.black, duration * Time.deltaTime);
		}
		else
		{
			myText.color = Color.Lerp (myText.color, Color.clear, duration * Time.deltaTime);
		}
	}
}