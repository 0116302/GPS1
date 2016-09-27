﻿using UnityEngine;
using System.Collections;

public class FlameDamage : MonoBehaviour
{
	public int flameDamage;


	void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag ("Player")) 
		{
			Player player = other.GetComponent<Player> ();
			player.hp -= flameDamage;
		}
	}
}