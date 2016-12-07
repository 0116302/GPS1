using UnityEngine;
using System.Collections;

public class CatElectrocutedStatusEffect : CatStatusEffect {

	public CatElectrocutedStatusEffect (float duration) {
		this.duration = duration;
	}

	public override void Start () {
		cat.animator.SetBool ("beingElectrocuted", true);
		cat.controller.frozen = true;
		cat.SetTint (new Color32 (255, 240, 0, 255));

		cat.Say ("*zap*");
	}

	public override void End () {
		if (cat.HasStatusEffect<CatElectrocutedStatusEffect> () <= 1) {
			cat.animator.SetBool ("beingElectrocuted", false);

			if (cat.HasStatusEffect<CatFrozenStatusEffect> () <= 1) {
				cat.controller.frozen = false;
				cat.SetTint (Color.white);
			}
		}
	}
}
