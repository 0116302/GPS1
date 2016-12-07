using UnityEngine;
using System.Collections;

public enum CatParticleSystem {
	Burning = 0,
	Poisoned
}

public class CatParticleSystemManager : MonoBehaviour {

	private static CatParticleSystemManager _instance;
	public static CatParticleSystemManager instance {
		get {
			if (_instance == null)
				Debug.LogError ("A script is trying to access the CatParticleSystemManager which isn't present in this scene!");

			return _instance;
		}
	}

	public GameObject burning;
	public GameObject poisoned;

	void Awake () {
		if (_instance == null)
			_instance = this;
		else if (instance != this)
			Destroy(gameObject);
	}

	public ParticleSystem Get (CatParticleSystem type) {
		//TODO Use some sort of object pooling to make this more efficient
		GameObject template = null;

		switch (type) {
		case CatParticleSystem.Burning:
			template = burning;
			break;
		case CatParticleSystem.Poisoned:
			template = poisoned;
			break;
		}

		return ((GameObject) Instantiate (template, Vector3.zero, template.transform.rotation)).GetComponent<ParticleSystem> ();
	}
}
