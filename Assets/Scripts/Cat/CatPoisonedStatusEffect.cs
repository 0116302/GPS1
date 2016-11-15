using UnityEngine;
using System.Collections;

public class CatPoisonedStatusEffect : CatStatusEffect {

	public CatPoisonedStatusEffect (float duration) {
		this.duration = duration;
	}

	public override void Start () {
		
	}

	public override void Tick () {
		cat.Damage (0.1f, DamageType.Poison);
	}

	public override void End () {

	}
}
