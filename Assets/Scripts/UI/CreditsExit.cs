using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class CreditsExit : MonoBehaviour
{	
	void Update ()
	{
		if (Input.anyKeyDown)
		{
			//Application.LoadLevel("MainMenu");
			SceneManager.LoadScene("MainMenu");
		}
	}
}