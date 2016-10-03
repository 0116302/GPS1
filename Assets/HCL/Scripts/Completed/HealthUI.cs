using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HealthUI : MonoBehaviour
{
	public Destructible des;
	Text healthText;

	void Awake()
	{
		healthText = GetComponent<Text> ();
	}
		
	void Update()
	{
		healthText.text = "Health: " + des.health;
	}
}