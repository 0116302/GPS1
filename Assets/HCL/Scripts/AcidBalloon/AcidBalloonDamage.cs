using UnityEngine;
using System.Collections;

public class AcidBalloonDamage : MonoBehaviour
{
	public int acidDamage;


	void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag ("Player")) 
		{
			HealthDisplay.health -= acidDamage;
			Player.hp -= acidDamage;
		}
	}
}