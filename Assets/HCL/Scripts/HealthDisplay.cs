using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HealthDisplay : MonoBehaviour
{
	public static int health;

	Text healthText;


	void Awake()
	{
		healthText = GetComponent<Text> ();
		health = Player.hp;
	}


	void Update()
	{
		healthText.text = "Health: " + health;
	}
}