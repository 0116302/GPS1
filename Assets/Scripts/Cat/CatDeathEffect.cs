using UnityEngine;
using System.Collections;

public class CatDeathEffect : MonoBehaviour {

	SoundEffect sound;

	public ParticleSystem bloodParticles;
	public ParticleSystem boneParticles;

	void Start () {
		sound = GetComponent<SoundEffect> ();

		StartCoroutine (EffectCoroutine ());
	}

	IEnumerator EffectCoroutine () {
		bloodParticles.Play ();
		boneParticles.Play ();
		sound.Play (0);

		yield return new WaitForSeconds (1.0f);

		bloodParticles.Stop ();
		boneParticles.Stop ();

		while (bloodParticles.IsAlive () || boneParticles.IsAlive ()) {
			yield return null;
		}

		Destroy (gameObject);
	}
}
