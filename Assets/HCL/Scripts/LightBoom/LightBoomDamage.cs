using UnityEngine;
using System.Collections;

public class LightBoomDamage : MonoBehaviour
{
	public int lightboomDamage;


	void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag ("Player")) 
		{
			Player player = other.GetComponent<Player> ();
			player.hp -= lightboomDamage;
		}
	}
}