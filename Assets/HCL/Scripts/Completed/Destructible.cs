using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public class Destructible : MonoBehaviour {

	[Header("Health")]
	public float health;
	public float maximumHealth;

	public UnityEvent onDeath = new UnityEvent (); // ???

	public void Damage (float amount) {
		health = Mathf.Clamp (health - amount, 0, maximumHealth); // Mathf.Clamp (float value, float min (value), float max (value)) Clamps a value between a minimum float and maximum float value.
		if (health <= 0f) onDeath.Invoke ();
	}

	public void Heal (float amount) {
		health = Mathf.Clamp (health + amount, 0, maximumHealth);
		if (health <= 0f) onDeath.Invoke ();
	}
}