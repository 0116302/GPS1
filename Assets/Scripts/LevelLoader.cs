using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class LevelLoader : MonoBehaviour {

	public string[] levelScenes;

	private static LevelLoader _instance = null;
	public static LevelLoader instance {
		get {
			if (_instance == null)
				Debug.LogError ("A script is trying to access the LevelLoader which hasn't been created!");

			return _instance;
		}
	}

	private int _currentLevel = 0;
	public int currentLevel {
		get {
			return _currentLevel;
		}
	}

	void Awake () {
		if (_instance == null) {
			_instance = this;
			DontDestroyOnLoad (gameObject);

		} else if (instance != this) {
			Destroy(gameObject);
		}
	}

	public void LoadLevel (int level) {
		Debug.Log ("Loading level " + level + "...");

		_currentLevel = level;
		SceneManager.LoadScene (levelScenes[level]);
	}
}
