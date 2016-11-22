using UnityEngine;
using System.Collections;

public class SettingWindow : MonoBehaviour {
	public AudioClip volumeClip;
	AudioSource audioSource;
	public float volume;

	private static SettingWindow _instance;
	public static SettingWindow instance{
		get{return _instance;}
	}

	void Start () {
		if (_instance == null)
			_instance = this;
		audioSource = GetComponent<AudioSource> ();
		audioSource.clip = volumeClip;
	}
	

	void Update () {
	
	}

	public void PlayVolumeClip(){
		audioSource.volume = volume * PlayerPrefs.GetFloat ("MasterVolume");
		audioSource.Play ();
	}
}
