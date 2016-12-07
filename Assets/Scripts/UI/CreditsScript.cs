using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class CreditsScript : MonoBehaviour
{
	public void Credits()
	{
		SceneManager.LoadScene("Credits");
	}
}