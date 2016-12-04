using UnityEngine;
using System.Collections;

public class CatHead : MonoBehaviour, ITriggerable {

	public int value = 100;
	public float despawnDelay = 5.0f;

	new Rigidbody rigidbody;
	new SpriteRenderer renderer;
	SoundEffect sound;

	Coroutine despawnCoroutine = null;
	bool collected = false;

	// Use this for initialization
	void Start () {
		rigidbody = GetComponent<Rigidbody> ();
		renderer = GetComponent<SpriteRenderer> ();
		sound = GetComponent<SoundEffect> ();

		despawnCoroutine = StartCoroutine (Despawn (despawnDelay));
	}

	IEnumerator Despawn (float delay) {
		yield return new WaitForSeconds (delay);
		Destroy (gameObject);
	}

	public void OnTrigger () {
		if (despawnCoroutine != null) StopCoroutine (despawnCoroutine);
		if (!collected) {
			StartCoroutine (Collect ());
			collected = true;
		}
	}

	IEnumerator Collect () {
		LevelManager.instance.cash += value;
		if (sound != null) sound.Play (0);
		rigidbody.isKinematic = true;

		float elapsedTime = 0.0f;
		Color startingColor = renderer.material.color;
		Color targetColor = startingColor;
		targetColor.a = 0.0f;

		while (elapsedTime < 1.0f) {
			renderer.material.color = Color.Lerp (startingColor, targetColor, (elapsedTime / 1.0f));
			transform.Translate (new Vector3 (0.0f, 1.0f, 0.0f) * Time.deltaTime, Space.World);

			elapsedTime += Time.deltaTime;
			yield return null;
		}
		renderer.material.color = targetColor;

		Destroy (gameObject);
	}

	public void OnHoverEnter () {

	}

	public void OnHoverStay () {

	}

	public void OnHoverExit () {

	}
}
