using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class AutoSwitchScene : MonoBehaviour {

	public string targetScene = "MainMenu";
	public float delay = 11.0f;


	void Start () {
		Invoke ("Transition", delay);
	}

	void Transition () {
		SceneManager.LoadScene (targetScene);
	}
}
