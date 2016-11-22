using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameStartScript : MonoBehaviour
{
	public void GameStart()
	{
		//Application.LoadLevel("GameScene");
		SceneManager.LoadScene("GameScene");
	}
}