using UnityEngine;
using System.Collections;

public class AcidDropPayload : MonoBehaviour {

	public float explosionRadius = 1.5f;
	public int particleCount = 10;

	bool exploded = false;

	new SpriteRenderer renderer;
	public new ParticleSystem particleSystem;

	// Use this for initialization
	void Awake () {
		renderer = GetComponent<SpriteRenderer> ();
	}

	void OnCollisionEnter (Collision collision) {
		if (!exploded) {
			StartCoroutine (Explosion ());
			exploded = true;
		}
	}

	IEnumerator Explosion () {
		particleSystem.Emit (particleCount);
		particleSystem.Stop ();
		renderer.enabled = false;

		int layerMask = 1 << LayerMask.NameToLayer("Enemies");
		Collider[] hits = Physics.OverlapSphere (transform.position, explosionRadius, layerMask, QueryTriggerInteraction.Ignore);

		foreach (Collider hit in hits) {
			Cat enemy = hit.GetComponent<Cat> ();
			if (enemy != null) {
				// Poison all enemies in the explosion radius
				enemy.AddStatusEffect (new CatPoisonedStatusEffect (6.0f));
			}
		}

		while (particleSystem.IsAlive ()) {
			yield return null;
		}

		Destroy (gameObject);
	}
}
