using UnityEngine;
using System.Collections;

public class CatBurningStatusEffect : CatStatusEffect {

	public override void Start () {
		tickFrequency = 2.0f;
	}

	public override void Tick () {
		cat.Damage (0.5f);
	}

	public override void End () {

	}
}
