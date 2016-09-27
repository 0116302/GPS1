using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HealthDisplay : MonoBehaviour
{

	public Player player;
	Text healthText;


	void Awake()
	{
		healthText = GetComponent<Text> ();
	}


	void Update()
	{
		healthText.text = "Health: " + player.hp;
	}
}