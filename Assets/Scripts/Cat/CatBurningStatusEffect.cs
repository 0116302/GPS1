using UnityEngine;
using System.Collections;

public class CatBurningStatusEffect : CatStatusEffect {

	ParticleSystem particleSystem;

	public CatBurningStatusEffect (float duration) {
		this.duration = duration;
	}

	public override void Start () {
		cat.RemoveStatusEffect<CatFrozenStatusEffect> ();

		cat.panickingState.Panic (duration);
		particleSystem = CatParticleSystemManager.instance.Get (CatParticleSystem.Burning);
		particleSystem.transform.parent = cat.transform;
		particleSystem.transform.localPosition = Vector3.zero;

		int r = Random.Range (1, 5);
		switch (r) {
		case 1:
			cat.Say ("I'M ON FIRE!!");
			break;
		case 2:
			cat.Say ("PUT IT OUT!! PUT IT OUT!");
			break;
		case 3:
			cat.Say ("HOT! HOT! HOT! HOT!");
			break;
		case 4:
			cat.Say ("MY TAIL! IT'S BURNING!");
			break;
		case 5:
			cat.Say ("Not again!");
			break;
		}
	}

	public override void Tick () {
		cat.Damage (0.5f, DamageType.Heat);
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
