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

	private GamePhase _gamePhase = GamePhase.Setup;
	public GamePhase gamePhase {
		get {
			return _gamePhase;
		}
	}

	[Header("Level Setup")]
	public TimedSpawner enemySpawner;
	public int enemyCount = 10;
	public int startingCash = 10000;

	[HideInInspector]
	public int enemiesLeft;
	[HideInInspector]
	public int cash;

	void Awake () {
		if (_instance == null)
			_instance = this;
		else if (instance != this)
			Destroy(gameObject);

		Initialize ();
	}

	public void Initialize () {
		enemiesLeft = enemyCount;
		cash = startingCash;
	}

	public void StartRaid () {
		_gamePhase = GamePhase.Raid;
		enemySpawner.enabled = true;

		//TODO Force exit placement mode and into activation mode
	}

	public void Lose () {
		_gamePhase = GamePhase.Lose;
	}

	public void Win () {
		_gamePhase = GamePhase.Win;
	}
}
