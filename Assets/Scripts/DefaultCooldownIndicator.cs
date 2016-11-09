using UnityEngine;
using System.Collections;

[RequireComponent (typeof(SpriteRenderer))]
public class DefaultCooldownIndicator : CooldownIndicator {

	protected new SpriteRenderer renderer;

	void Awake () {
		renderer = GetComponent<SpriteRenderer> ();
	}

	void Update () {
		renderer.material.SetFloat ("_Grayscale", 1.0f - value);
	}
}
