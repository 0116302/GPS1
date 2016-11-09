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

	public PlayerController player;
	[Space (10)]

	[Header ("Level Setup")]
	public TimedSpawner enemySpawner;
	public int enemyCount = 30;
	public int startingCash = 30000;

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
		enemyCount = enemySpawner.spawns.Count;
		enemiesLeft = enemyCount;
		cash = startingCash;
	}

	public void StartRaid () {
		_gamePhase = GamePhase.Raid;
		if (player != null) player.EnterActivationMode ();
		enemySpawner.enabled = true;
	}

	public void Lose () {
		_gamePhase = GamePhase.Lose;
		GUIManager.instance.Lose ();
	}

	public void Win () {
		_gamePhase = GamePhase.Win;
		GUIManager.instance.Win ();
	}
}
