using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class InformationText : MonoBehaviour
{
	public Text myText;
	public float duration;
	public bool displayInfo;


	void Update()
	{
		ShowInfo();
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