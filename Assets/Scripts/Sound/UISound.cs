using UnityEngine;
using System.Collections;

public class UISound : MonoBehaviour {
	public AudioClip mouseOver;
	public AudioClip mouseClick;
	AudioSource audioSource;
	string audioType = "SFXVolume";
	string masterVolume = "MasterVolume";
	// Use this for initialization
	void Start () {
		audioSource = GetComponent<AudioSource> ();
	}
	
	// Update is called once per frame
	void Update () {
		audioSource.volume = PlayerPrefs.GetFloat (audioType) * PlayerPrefs.GetFloat (masterVolume);
	}

	public void OnMouseOver(){
		audioSource.clip = mouseOver;
		audioSource.Play ();
	}

	public void OnMouseClick(){
		audioSource.clip = mouseClick;
		audioSource.Play ();
	}
}
