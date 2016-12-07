using UnityEngine;
using System.Collections;

public class CatFrozenStatusEffect : CatStatusEffect {

	public CatFrozenStatusEffect (float duration) {
		this.duration = duration;
	}

	public override void Start () {
		if (cat.HasStatusEffect<CatBurningStatusEffect> () > 0) return; // A burning cat can't be frozen
		
		cat.animator.SetBool ("frozen", true);
		cat.controller.frozen = true;
		cat.SetTint (new Color32 (40, 196, 255, 255));

		int r = Random.Range (1, 6);
		switch (r) {
		case 1:
			cat.Say ("CAN'T... MOVE...");
			break;
		case 2:
			cat.Say ("C-c-C-Cold!");
			break;
		case 3:
			cat.Say ("I need a blanket!");
			break;
		case 4:
			cat.Say ("Who messed with the thermostat!?");
			break;
		case 5:
			cat.Say ("Not again!");
			break;
		}
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
