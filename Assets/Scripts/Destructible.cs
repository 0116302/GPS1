using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public enum DamageType {
	Generic = 0,
	Impact,
	Heat,
	Cold
}

public class Destructible : MonoBehaviour {

	[Header("Health")]
	public float health = 100f;
	public float maximumHealth = 100f;

	public UnityEvent onDestroyed = new UnityEvent ();

	public virtual void Damage (float amount, DamageType type = DamageType.Generic) {
		health = Mathf.Clamp (health - amount, 0, maximumHealth);
		if (health <= 0f) onDestroyed.Invoke ();
	}

	public virtual void Heal (float amount) {
		health = Mathf.Clamp (health + amount, 0, maximumHealth);
		if (health <= 0f) onDestroyed.Invoke ();
	}
}
