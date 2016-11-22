using UnityEngine;
using System.Collections;

public class CatPoisonedStatusEffect : CatStatusEffect {

	public CatPoisonedStatusEffect (float duration) {
		this.duration = duration;
	}

	public override void Start () {
		int r = Random.Range (1, 5);
		switch (r) {
		case 1:
			cat.Say ("*cough*");
			break;
		case 2:
			cat.Say ("Why am I so dizzy?");
			break;
		case 3:
			cat.Say ("I feel like I'm going to puke.");
			break;
		case 4:
			cat.Say ("Which way to the toilet?");
			break;
		case 5:
			cat.Say ("Not again!");
			break;
		}
	}

	public override void Tick () {
		cat.Damage (0.1f, DamageType.Poison);
	}

	public override void End () {

	}
}
