using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIElementFade : MonoBehaviour {

	Graphic graphic;

	public float fadeInAfter = 1.0f;
	public float fadeInDuration = 1.0f;
	public float fadeOutAfter = 1.0f;
	public float fadeOutDuration = 1.0f;

	// Use this for initialization
	void Start () {
		graphic = GetComponent<Graphic> ();
		StartCoroutine (Fade ());
	}
	
	// Update is called once per frame
	IEnumerator Fade () {
		graphic.CrossFadeAlpha (0.0f, 0.0f, true);
		yield return new WaitForSeconds (fadeInAfter);
		graphic.CrossFadeAlpha (1.0f, fadeInDuration, false);
		yield return new WaitForSeconds (fadeOutAfter);
		graphic.CrossFadeAlpha (0.0f, fadeOutDuration, false);
	}
}
