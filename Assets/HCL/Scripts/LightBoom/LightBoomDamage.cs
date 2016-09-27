using UnityEngine;
using System.Collections;

public class LightBoomDamage : MonoBehaviour
{
	public int lightboomDamage;


	void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag ("Player")) 
		{
			HealthDisplay.health -= lightboomDamage;
			Player.hp -= lightboomDamage;
		}
	}
}