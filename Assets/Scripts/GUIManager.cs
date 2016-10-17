using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class GUIManager : MonoBehaviour {

	public Transform trapToolbar;
	public Text cashDisplay;
	public Text enemyCountDisplay;
	public Text cooldownDisplay;
	public Text winLoseDisplay;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		cashDisplay.text = "$" + GameManager.cash;
		enemyCountDisplay.text = "Enemies: " + GameManager.enemyCount;

		TempStart();
		TempRestart();
	}

	public void StartRaid () {
		GameManager.gamePhase = GamePhase.Raid;
		GameObject.Find ("EnemySpawner").GetComponent<TimedSpawner> ().enabled = true;
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
		GameManager.gamePhase = GamePhase.Setup;
		GameManager.enemyCount = 10;
		GameManager.cash = 10000;

		SceneManager.LoadScene ("GameScene");
	}

	public void TempStart ()
	{
		if (Input.GetKeyDown(KeyCode.Return))
		{
		GameManager.gamePhase = GamePhase.Raid;
		GameObject.Find ("EnemySpawner").GetComponent<TimedSpawner> ().enabled = true;
		trapToolbar.gameObject.SetActive(false);
		}
	}

	public void TempRestart ()
	{
		if (Input.GetKeyDown(KeyCode.R))
		{
			GameManager.gamePhase = GamePhase.Setup;
			GameManager.enemyCount = 10;
			GameManager.cash = 10000;

			SceneManager.LoadScene ("GameScene");
		}
	}
}
