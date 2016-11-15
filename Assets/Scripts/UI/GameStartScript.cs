using UnityEngine;
using System.Collections;

public class GameStartScript : MonoBehaviour
{
	public void GameStart()
	{
		Application.LoadLevel("GameScene");
	}
}