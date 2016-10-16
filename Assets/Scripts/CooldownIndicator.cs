using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CooldownIndicator : MonoBehaviour {

	public Image overlay;
	[Range (0.0f, 1.0f)]
	public float cooldownValue = 0.0f;
	
	void OnValidate () {
		overlay.fillAmount = cooldownValue;
	}
	
	// Update is called once per frame
	void Update () {
		overlay.fillAmount = cooldownValue;
	}
}
