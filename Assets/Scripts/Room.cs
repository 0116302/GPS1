using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Room : MonoBehaviour {

	[Header ("Room Setup")]
	public string roomName = "Room";
	public float roomWidth = 5.0f;
	public Transform cameraPosition;
	public Transform placementGrid;

	[Header ("Gameplay Settings")]
	public bool canPlaceDefenses = true;

	[Header ("Relative Position")]
	public Room roomRight;
	public Room roomLeft;
	public Room roomAbove;
	public Room roomBelow;

	[Header ("Paths")]
	public Door leftDoor;
	public Door rightDoor;
	public List<Door> staircases;

	[Header ("Camera Settings")]
	public float cameraFOV = 12.0f;

	[Header ("Visual Settings")]
	public float gridAlpha = 0.8f;

	[HideInInspector] public List<Cat> occupants = new List<Cat> ();

	void Start () {
		placementGrid.gameObject.SetActive (true);

		Material material = placementGrid.GetComponent<Renderer> ().material;
		Color color = material.color;
		color.a = 0.0f;
		material.color = color;
	}

	void OnTriggerEnter (Collider other) {
		if (other.CompareTag ("Enemy")) {
			Cat enemy = other.GetComponent<Cat> ();
			if (enemy != null) {
				occupants.Add (enemy);
			}
		}
	}

	void OnTriggerExit (Collider other) {
		if (other.CompareTag ("Enemy")) {
			Cat enemy = other.GetComponent<Cat> ();
			if (enemy != null) {
				occupants.Remove (enemy);
			}
		}
	}

	public void fadeInGrid (float duration = 0.5f) {
		StartCoroutine (fadeInGridCoroutine (duration));
	}

	public void fadeOutGrid (float duration = 0.5f) {
		StartCoroutine (fadeOutGridCoroutine (duration));
	}

	IEnumerator fadeInGridCoroutine (float duration) {
		float elapsedTime = 0.0f;
		YieldInstruction waitForEndOfFrame = new WaitForEndOfFrame ();

		Material material = placementGrid.GetComponent<Renderer> ().material;
		Color color = material.color;

		while (elapsedTime < duration) {
			color.a = Mathf.Lerp (0.0f, gridAlpha, (elapsedTime / duration));
			material.color = color;

			elapsedTime += Time.deltaTime;
			yield return waitForEndOfFrame;
		}
		color.a = gridAlpha;
		material.color = color;

		yield break;
	}

	IEnumerator fadeOutGridCoroutine (float duration) {
		float elapsedTime = 0.0f;
		YieldInstruction waitForEndOfFrame = new WaitForEndOfFrame ();

		Material material = placementGrid.GetComponent<Renderer> ().material;
		Color color = material.color;

		while (elapsedTime < duration) {
			color.a = Mathf.Lerp (gridAlpha, 0.0f, (elapsedTime / duration));
			material.color = color;

			elapsedTime += Time.deltaTime;
			yield return waitForEndOfFrame;
		}
		color.a = 0.0f;
		material.color = color;

		yield break;
	}
}
