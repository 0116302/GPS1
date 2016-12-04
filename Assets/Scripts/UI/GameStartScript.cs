using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameStartScript : MonoBehaviour {
	
	public Button level2Button;
	public Button level3Button;

	void Start () {
		if (Game.current.levelUnlocked [1])
			level2Button.interactable = true;
		else
			level2Button.interactable = false;

		if (Game.current.levelUnlocked [2])
			level3Button.interactable = true;
		else
			level3Button.interactable = false;
	}

	public void GameStart() {
		LevelLoader.instance.LoadLevel (0);
	}

	public void LoadLevel(int level) {
		LevelLoader.instance.LoadLevel (level);
	}
}