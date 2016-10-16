using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class GUIManager : MonoBehaviour {

	private static GUIManager _instance;
	public static GUIManager instance {
		get {
			if (_instance == null)
				Debug.LogError ("A script is trying to access the GUIManager which isn't present in this scene!");

			return _instance;
		}
	}

	public Transform trapToolbar;
	public Text cashDisplay;
	public Text enemyCountDisplay;
	public Text cooldownDisplay;
	public Text winLoseDisplay;

	void Awake () {
		if (_instance == null)
			_instance = this;
		else if (instance != this)
			Destroy(gameObject);

		DontDestroyOnLoad(gameObject);
	}
	
	// Update is called once per frame
	void Update () {
		cashDisplay.text = "$" + GameManager.instance.cash;
		enemyCountDisplay.text = "Enemies: " + GameManager.instance.enemyCount;
	}

	// Update the GUI to match the room we are in
	public void SwitchToRoom () {

	}

	public void StartRaid () {
		GameManager.instance.StartRaid ();
		trapToolbar.gameObject.SetActive(false);
	}

	public void Win () {
		winLoseDisplay.text = "WIN!";
		winLoseDisplay.color = new Color (0, 255, 0);
	}

	public void Lose () {
		winLoseDisplay.text = "LOSE!";
		winLoseDisplay.color = new Color (255, 0, 0);
	}

	public void RestartLevel () {
		SceneManager.LoadScene ("GameScene");
	}
}
