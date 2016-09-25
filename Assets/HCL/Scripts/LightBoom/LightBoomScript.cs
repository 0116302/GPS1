using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LightBoomScript : MonoBehaviour
{
	public Transform respawnLB;
	public GameObject lightBoom;
	public GameObject explosion;
	public float lifetime;

	public Text myText;
	public float duration;
	public bool displayInfo;


	void Start() 
	{
		myText = GameObject.Find("LightBoomInfoText").GetComponent<Text>();
		myText.color = Color.clear;
	}


	void Update()
	{
		if(Input.GetKeyDown(KeyCode.E)) // respawn for testing purpose but if destroy, cannot spawn anymore (NEED HELP~)
		{
			Instantiate (lightBoom, respawnLB.position, respawnLB.rotation);
		}
		ShowInfo();
	}


	void OnMouseDown()
	{
		Destroy (this.gameObject, lifetime);
		/* Not working, looking for a way to instantiate the explosion after the game object destroy
		if (this.gameObject == null)
		{
			Instantiate(explosion, transform.position, transform.rotation);
		}
		*/
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