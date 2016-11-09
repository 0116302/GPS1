using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public abstract class CooldownIndicator : MonoBehaviour {

	[Range (0.0f, 1.0f)]
	protected float _value = 0.0f;
	public float value {
		get {
			return _value;
		}

		set {
			_value = Mathf.Clamp (value, 0.0f, 1.0f);
		}
	}

}
