using UnityEngine;
using System.Collections;

public class CatFrozenStatusEffect : CatStatusEffect {

	public CatFrozenStatusEffect () {
		duration = 10.0f;
	}

	public override void Start () {
		cat.animator.SetBool ("frozen", true);
		cat.controller.frozen = true;
	}

	public override void End () {
		if (cat.HasStatusEffect<CatFrozenStatusEffect> () <= 1) {
			cat.animator.SetBool ("frozen", false);
			cat.controller.frozen = false;
		}
	}
}
