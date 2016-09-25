using UnityEngine;
using System.Collections;

public class FlameDamage : MonoBehaviour
{
	public int flameDamage;


	void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag ("Player")) 
		{
			HealthDisplay.health -= flameDamage;
			other.GetComponent<Player>().hp -= flameDamage;
		}
	}
}