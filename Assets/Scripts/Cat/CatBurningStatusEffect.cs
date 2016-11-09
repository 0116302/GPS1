using UnityEngine;
using System.Collections;

public class CatBurningStatusEffect : CatStatusEffect {

	public CatBurningStatusEffect (float duration) {
		this.duration = duration;
	}

	public override void Start () {
		cat.panickingState.Panic (duration);
	}

	public override void Tick () {
		cat.Damage (0.1f, DamageType.Heat);
	}

	public override void End () {

	}
}
