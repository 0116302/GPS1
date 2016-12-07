using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public struct SpawnInstruction {
	public GameObject prefab;
	public float delay;

	public SpawnInstruction (GameObject prefab, float delay) {
		this.prefab = prefab;
		this.delay = delay;
	}
}

public class TimedSpawner : MonoBehaviour {
	public List<SpawnInstruction> spawns = new List<SpawnInstruction> ();

	private float elapsedTime = 0.0f;
	
	// Update is called once per frame
	void Update () {
		elapsedTime += Time.deltaTime;
		if (spawns.Count > 0 && elapsedTime >= spawns[0].delay) {
			GameObject.Instantiate (spawns[0].prefab, transform.position, transform.rotation);
			spawns.RemoveAt (0);
			elapsedTime = 0.0f;
		}
	}

	public void SpawnNext () {
		if (spawns.Count > 0) {
			GameObject.Instantiate (spawns[0].prefab, transform.position, transform.rotation);
			spawns.RemoveAt (0);
			elapsedTime = 0.0f;
		}
	}
}
