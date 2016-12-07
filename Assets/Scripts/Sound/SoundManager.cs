using UnityEngine;
using System.Collections;

public class SoundManager : MonoBehaviour {

	private static SoundManager _instance = null;
	public static SoundManager instance {
		get {
			if (_instance == null)
			Debug.LogError ("A script is trying to access the SoundManager which isn't present in this scene!");
			
			return _instance;
		}
	}

	[Header ("Volume Setting Keys")]
	public string masterVolumeSetting = "masterVolume";
	public string soundVolumeSetting = "soundVolume";
	public string musicVolumeSetting = "musicVolume";

	[Header ("Background Music")]
	public AudioSource BGMSource;
	public AudioClip defaultBGM;
	public bool autoplay = false;
	public bool loop = true;

	void Awake () {
		if (_instance == null)
			_instance = this;
		else if (instance != this)
			Destroy(gameObject);

		BGMSource.ignoreListenerPause = true;
		BGMSource.volume = GetMusicVolume () * GetMasterVolume ();

		if (autoplay) {
			BGMSource.clip = defaultBGM;
			BGMSource.loop = loop;
			BGMSource.Play ();
		}
	}

	public void PlayBGM (AudioClip clip, bool loop = true) {
		BGMSource.clip = clip;
		BGMSource.loop = loop;
		BGMSource.Play ();
	}

	public float GetMasterVolume () {
		return PlayerPrefs.GetFloat (masterVolumeSetting, 1.0f);
	}

	public float GetSoundVolume () {
		return PlayerPrefs.GetFloat (soundVolumeSetting, 1.0f);
	}

	public float GetMusicVolume () {
		return PlayerPrefs.GetFloat (musicVolumeSetting, 1.0f);
	}

	public void SetMasterVolume (float value) {
		PlayerPrefs.SetFloat (masterVolumeSetting, value);
		BGMSource.volume = GetMusicVolume () * GetMasterVolume ();
	}

	public void SetSoundVolume (float value) {
		PlayerPrefs.SetFloat (soundVolumeSetting, value);
	}

	public void SetMusicVolume (float value) {
		PlayerPrefs.SetFloat (musicVolumeSetting, value);
		BGMSource.volume = GetMusicVolume () * GetMasterVolume ();
	}
}
