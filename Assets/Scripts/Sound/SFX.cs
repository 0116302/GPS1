using UnityEngine;
using System.Collections;

public class SFX : MonoBehaviour {
	AudioSource audioSource;
	public AudioClip audioClip;
	string audioType = "SFXVolume";
	string masterVolume = "MasterVolume";

	void Start () {
		audioSource = GetComponent<AudioSource> ();
		audioSource.clip = audioClip;
	}
	public void Play(){
		audioSource.Play ();
	}

	void Update () {
		audioSource.volume = PlayerPrefs.GetFloat (audioType) * PlayerPrefs.GetFloat (masterVolume);
	}
}
