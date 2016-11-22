using UnityEngine;
using System.Collections;

public class CatSFX : MonoBehaviour {
	AudioSource audioSource;
	public AudioClip deathClip;
	public AudioClip meowClip;
	string audioType = "SFXVolume";
	string masterVolume = "MasterVolume";

	void Start () {
		audioSource = GetComponent<AudioSource> ();
	}
	public void PlayDeath(){
		audioSource.clip = deathClip;
		audioSource.Play ();
	}
	public void PlayMeow(){
		audioSource.clip = meowClip;
		audioSource.Play ();
	}
	void Update () {
		audioSource.volume = PlayerPrefs.GetFloat (audioType) * PlayerPrefs.GetFloat (masterVolume);
	}
}
