using UnityEngine;
using System.Collections;

[RequireComponent (typeof(AudioSource))]
public class SoundEffect : MonoBehaviour {

	AudioSource source;
	public AudioClip[] clips;

	void Awake () {
		source = GetComponent<AudioSource> ();
	}

	public void Play (int clip = 0) {
		source.PlayOneShot (clips[clip], SoundManager.instance.GetSoundVolume () * SoundManager.instance.GetMasterVolume ());
	}
}
