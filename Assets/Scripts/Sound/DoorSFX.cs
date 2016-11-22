using UnityEngine;
using System.Collections;

public class DoorSFX : MonoBehaviour {
	AudioSource audioSource;
	public AudioClip OpenClip;
	public AudioClip CloseClip;
	string audioType = "SFXVolume";
	string masterVolume = "MasterVolume";
	Door door;

	private static DoorSFX _instance;
	public static DoorSFX instance{
		get{ return _instance; }
	}
	// Use this for initialization
	void Start () {
		if (_instance == null)
			_instance = this;
		audioSource = GetComponent<AudioSource> ();
	}
	
	// Update is called once per frame
	void Update () {
		audioSource.volume = PlayerPrefs.GetFloat (audioType) * PlayerPrefs.GetFloat (masterVolume);
	}

	public void Play(){
		door = GetComponent<Door> ();
		audioSource = GetComponent<AudioSource> ();
		if (door != null) {
			if (door.isOpen) {
				audioSource.clip = OpenClip;
			} else {
				audioSource.clip = CloseClip;
			}
			Debug.Log ("Before");
			audioSource.Play ();
			Debug.Log("After");
		}
	}
	public void Go(){
		Debug.Log ("Hello");
		audioSource.Play ();
	}
}
