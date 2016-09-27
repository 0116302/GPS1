using UnityEngine;
using System.Collections;

public class AcidBalloonDamage : MonoBehaviour
{
	public int acidDamage;


	void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag ("Player")) 
		{
			Player player = other.GetComponent<Player> ();
			player.hp -= acidDamage;
		}
	}
}