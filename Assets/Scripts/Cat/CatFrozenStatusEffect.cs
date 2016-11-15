using UnityEngine;
using System.Collections;

public class CatFrozenStatusEffect : CatStatusEffect {

	public CatFrozenStatusEffect (float duration) {
		this.duration = duration;
	}

	public override void Start () {
		cat.animator.SetBool ("frozen", true);
		cat.controller.frozen = true;
		cat.SetTint (new Color32 (40, 196, 255, 255));
	}

	public override void End () {
		if (cat.HasStatusEffect<CatFrozenStatusEffect> () <= 1) {
			cat.animator.SetBool ("frozen", false);

			if (cat.HasStatusEffect<CatElectrocutedStatusEffect> () <= 1) {
				cat.controller.frozen = false;
				cat.SetTint (Color.white);
			}
		}
	}
}
