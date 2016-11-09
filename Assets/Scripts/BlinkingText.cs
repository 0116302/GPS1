using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[RequireComponent (typeof(Text))]
public class BlinkingText : MonoBehaviour {

	public float showDuration = 1.0f;
	public float hideDuration = 1.0f;

	// Use this for initialization
	void Start () {
		StartCoroutine (BlinkingCoroutine (GetComponent<Text> (), showDuration, hideDuration));
	}
	
	IEnumerator BlinkingCoroutine (Text text, float showDuration, float hideDuration) {
		string textString = text.text;

		while (true) {
			text.text = "";
			yield return new WaitForSeconds (hideDuration);
			text.text = textString;
			yield return new WaitForSeconds (showDuration);
		}
	}
}
