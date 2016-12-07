using UnityEngine;
using System.Collections;

public enum GamePhase {
	Setup = 0,
	Raid,
	Win,
	Lose
}

public class LevelManager : MonoBehaviour {

	private static LevelManager _instance;
	public static  LevelManager instance {
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

	[HideInInspector]
	public int enemiesLeft;
	[HideInInspector]
	public int enemiesOnScreen = 0;
	[HideInInspector]
	public int cash;

	public Transform bonusObjective1;
	public int bonus1Amount = 500;
	[HideInInspector]
	public bool bonus1Received = true;
	public Transform bonusObjective2;
	public int bonus2Amount = 750;
	[HideInInspector]
	public bool bonus2Received = true;
	public Transform bonusObjective3;
	public int bonus3Amount = 1000;
	[HideInInspector]
	public bool bonus3Received = true;

	[Header ("Music")]
	public AudioClip setupPhaseBGM;
	public AudioClip raidPhaseBGM;

	void Awake () {
		if (_instance == null)
			_instance = this;
		else if (instance != this)
			Destroy(gameObject);
	}

	void Start () {
		enemiesLeft = enemySpawner.spawns.Count;
		cash = Game.current.levelCash[LevelLoader.instance.currentLevel];

		Debug.Log ("Started level " + LevelLoader.instance.currentLevel + " with $" + cash + "!");

		SoundManager.instance.PlayBGM (setupPhaseBGM);
	}

	public void StartRaid () {
		_gamePhase = GamePhase.Raid;
		if (player != null) player.EnterActivationMode ();
		enemySpawner.enabled = true;

		SoundManager.instance.PlayBGM (raidPhaseBGM);
	}

	public void Lose () {
		_gamePhase = GamePhase.Lose;
		Invoke ("LoseGUI", 3.0f);
	}

	void LoseGUI () {
		GUIManager.instance.Lose ();
	}

	public void Win () {
		_gamePhase = GamePhase.Win;

		if (bonus1Received) {
			cash += bonus1Amount;
		}

		if (bonus2Received) {
			cash += bonus2Amount;
		}

		if (bonus3Received) {
			cash += bonus3Amount;
		}

		Invoke ("WinGUI", 3.0f);

		// Unlock the next level
		if (LevelLoader.instance.currentLevel < Game.levelCount) {
			Game.current.levelUnlocked [LevelLoader.instance.currentLevel + 1] = true;
			Game.current.levelCash [LevelLoader.instance.currentLevel + 1] = cash;
			SaveManager.Save ();
		}
	}

	void WinGUI () {
		GUIManager.instance.Win ();
	}
}
