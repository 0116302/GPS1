using UnityEngine;
using System.Collections;

public class FadingText : MonoBehaviour {

	public TextMesh text;
	public TextMesh highlight;

	public string value = "Text";

	public float riseSpeed = 0.25f;
	public float fadeAfter = 2.0f;
	public float fadeDuration = 0.5f;

	void Start () {
		StartCoroutine (Animation ());
	}

	void OnValidate () {
		SetText (value);
	}

	public void SetText (string value) {
		this.value = value;
		text.text = value;
		highlight.text = value;
	}
	
	IEnumerator Animation () {
		Color textColor1 = text.color;
		Color textColor2 = textColor1;
		textColor2.a = 0.0f;

		Color highlightColor1 = highlight.color;
		Color highlightColor2 = highlightColor1;
		highlightColor2.a = 0.0f;

		float elapsedTime = 0.0f;
		float duration = fadeAfter + fadeDuration;
		while (elapsedTime < duration) {
			// Rise
			Vector3 pos = transform.localPosition;
			pos.y += riseSpeed * Time.deltaTime;
			transform.localPosition = pos;

			// Fade
			if (elapsedTime >= fadeAfter) {
				text.color = Color.Lerp (textColor1, textColor2, (elapsedTime - fadeAfter) / fadeDuration);
				highlight.color = Color.Lerp (highlightColor1, highlightColor2, (elapsedTime - fadeAfter) / fadeDuration);
			}

			elapsedTime += Time.deltaTime;
			yield return null;
		}

		Destroy (gameObject);
	}
}
