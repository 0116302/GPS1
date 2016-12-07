using UnityEngine;
using System.Collections;

[System.Serializable]
public class Game {

	public static Game current = null;

	private static bool _isPaused = false;
	public static bool isPaused {
		get {
			return _isPaused;
		}
	}

	public const int levelCount = 3;
	public bool[] levelUnlocked = new bool[levelCount];
	public int[] levelCash = new int[levelCount];

	public Game () {
		levelUnlocked [0] = true;
		levelCash [0] = 15000;
	}

	public static void Pause () {
		Time.timeScale = 0.0f;
		AudioListener.pause = true;
		_isPaused = true;
	}

	public static void UnPause () {
		Time.timeScale = 1.0f;
		AudioListener.pause = false;
		_isPaused = false;
	}
}
