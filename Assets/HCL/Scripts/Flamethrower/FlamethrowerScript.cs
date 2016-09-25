using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FlamethrowerScript : MonoBehaviour
{
	public Transform shotPosition;
	public Transform projectile;
	public Transform target;
	public float range;

	public Text myText;
	public float duration;
	public bool displayInfo;

	void Start ()
	{
		target = GameObject.FindGameObjectWithTag ("Player").transform;

		myText = GameObject.Find("FlamethrowerInfoText").GetComponent<Text>();
		myText.color = Color.clear;
	}


	void Update ()
	{
		float dist = (Vector3.Distance(this.transform.position, target.transform.position));
		if (dist < range)
		{
			Vector3 direction = this.transform.position - target.position;
			direction.Normalize ();

			float angle = Mathf.Atan2 (direction.y, direction.x) * Mathf.Rad2Deg;

			this.transform.rotation = Quaternion.Lerp (this.transform.rotation, Quaternion.Euler (0f, 0f, angle - 90f), Time.deltaTime * 3f);
		}
		ShowInfo();
	}


	void OnMouseDown ()
	{
		Instantiate (projectile, shotPosition.position, shotPosition.rotation);
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