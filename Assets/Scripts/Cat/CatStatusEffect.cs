using UnityEngine;
using System.Collections;

public abstract class CatStatusEffect {

	protected Cat cat;
	public Coroutine coroutine;

	public float tickFrequency = 1.0f;
	public float duration = 5.0f;
	public float elapsedTime = 0.0f;

	public void Attach (Cat cat) {
		this.cat = cat;
	}

	public virtual void Start () {
		
	}

	public virtual void Tick () {
		
	}

	public virtual void End () {

	}
}
