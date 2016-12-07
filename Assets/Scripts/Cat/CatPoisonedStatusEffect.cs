using UnityEngine;
using System.Collections;

public class CatPoisonedStatusEffect : CatStatusEffect {

	ParticleSystem particleSystem;

	public CatPoisonedStatusEffect (float duration) {
		this.duration = duration;
	}

	public override void Start () {
		particleSystem = CatParticleSystemManager.instance.Get (CatParticleSystem.Poisoned);
		particleSystem.transform.parent = cat.transform;
		particleSystem.transform.localPosition = Vector3.zero;

		int r = Random.Range (1, 6);
		switch (r) {
		case 1:
			cat.Say ("*cough*");
			break;
		case 2:
			cat.Say ("Why is all my fur falling off?");
			break;
		case 3:
			cat.Say ("I feel like I'm gonna puke.");
			break;
		case 4:
			cat.Say ("Which way to the bathroom?");
			break;
		case 5:
			cat.Say ("Not again!");
			break;
		}
	}

	public override void Tick () {
		cat.Damage (0.5f, DamageType.Poison);
	}

	public override void End () {
		cat.StartCoroutine (DestroyParticleSystem ());
	}

	IEnumerator DestroyParticleSystem () {
		particleSystem.Stop ();

		while (particleSystem.IsAlive ()) {
			yield return null;
		}

		GameObject.Destroy (particleSystem.gameObject);
	}
}
