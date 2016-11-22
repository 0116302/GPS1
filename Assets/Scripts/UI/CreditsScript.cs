using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class CreditsScript : MonoBehaviour
{
	public void Credits()
	{
		//Application.LoadLevel("Credits");
		SceneManager.LoadScene("Credits");
	}
}