using UnityEngine;
using System.Collections;

public enum GamePhase {
	Setup = 0,
	Raid,
	Win,
	Lose
}

public class GameManager : MonoBehaviour {

	private static GameManager _instance;
	public static  GameManager instance {
		get {
			if (_instance == null)
				Debug.LogError ("A script is trying to access the GameManager which isn't present in this scene!");
			
			return _instance;
		}
	}

	public GamePhase gamePhase = GamePhase.Setup;
	public int enemyCount = 0;
	public int killsToWin = 0;
	public int cash = 10000;

	void Awake () {
		if (_instance == null)
			_instance = this;
		else if (instance != this)
			Destroy(gameObject);    

		DontDestroyOnLoad(gameObject);
	}

	public void Initialize () {

	}
}
