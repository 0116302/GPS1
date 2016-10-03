using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public class Destructible : MonoBehaviour {

	[Header("Health")]
	public float health = 100f;
	public float maximumHealth = 100f;

	public UnityEvent onDeath = new UnityEvent ();

	public void Damage (float amount) {
		health = Mathf.Clamp (health - amount, 0, maximumHealth);
		if (health <= 0f) onDeath.Invoke ();
	}

	public void Heal (float amount) {
		health = Mathf.Clamp (health + amount, 0, maximumHealth);
		if (health <= 0f) onDeath.Invoke ();
	}
}
