using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public enum DamageType {
	Generic = 0,
	Impact,
	Heat,
	Cold,
	Electricity,
	Poison
}

public class Destructible : MonoBehaviour {

	[Header("Health")]
	public float health = 10.0f;
	public float maximumHealth = 10.0f;

	private bool _isDead = false;
	public bool isDead {
		get {
			return _isDead;
		}
	}

	public delegate void DamagedEventHandler (float amount, DamageType type);
	public event DamagedEventHandler onDamaged;

	public delegate void HealedEventHandler (float amount);
	public event HealedEventHandler onHealed;

	public delegate void  DestroyedEventHandler ();
	public event DestroyedEventHandler onDestroyed;

	public virtual void Damage (float amount, DamageType type = DamageType.Generic) {
		if (!_isDead) {
			health = Mathf.Clamp (health - amount, 0, maximumHealth);

			if (amount > 0 && onDamaged != null)
				onDamaged (amount, type);

			if (health <= 0.0f) {
				if (onDestroyed != null)
					onDestroyed ();
				_isDead = true;
			}
		}
	}

	public virtual void Destroy (DamageType type = DamageType.Generic) {
		if (!_isDead) {
			float amount = health;
			health = 0.0f;

			if (health > 0 && onDamaged != null) onDamaged (amount, type);

			if (onDestroyed != null) onDestroyed ();
			_isDead = true;
		}
	}

	public virtual void Heal (float amount) {
		if (!_isDead) {
			health = Mathf.Clamp (health + amount, 0, maximumHealth);

			if (amount > 0 && onHealed != null) onHealed (amount);
		}
	}
}
