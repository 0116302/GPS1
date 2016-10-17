using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TimedSpawner : MonoBehaviour {

	public GameObject prefab;
	public List<float> spawnTimes = new List<float> ();

	private float elapsedTime = 0.0f;

	void Update () {
//		elapsedTime += Time.deltaTime;
//		if (spawnTimes.Count >= 1 && elapsedTime >= spawnTimes[0]) {
//			GameObject.Instantiate (prefab, transform.position, transform.rotation);
//			spawnTimes.RemoveAt (0);
//			elapsedTime = 0.0f;
//		}

		if (Input.GetKeyDown(KeyCode.Q))
		{
			GameObject.Instantiate (prefab, transform.position, transform.rotation);
		}
	}
}
