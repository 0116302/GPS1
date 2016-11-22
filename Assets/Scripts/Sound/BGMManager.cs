using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
public enum BGM{
	BGM_PlACEMENT = 0,
	BGM_INGAME,
	TOTAL
}

[System.Serializable]
public class BGMAudioClipInfo
{
	public BGM bgmStatus;
	public AudioClip audioClip;
}
public class BGMManager : MonoBehaviour {
	public List<BGMAudioClipInfo> bgmAudioClipList = new List<BGMAudioClipInfo> ();
	public AudioSource bgmAudioSource;
	string audioType = "BGMVolume";
	string masterVolume = "MasterVolume";
	private static BGMManager _instance;
	public static BGMManager instance{
		get{			
			return _instance;
		}
	}

	void Awake(){
		if (_instance == null)
			_instance = this;
	}

	void Start () {
		Play ();
	}
	void Update(){
		bgmAudioSource.volume = PlayerPrefs.GetFloat (audioType) * PlayerPrefs.GetFloat (masterVolume);
	}
	public void Play(){
		if (GameManager.instance.gamePhase == GamePhase.Setup) {
			bgmAudioSource.clip = bgmAudioClipList [(int)BGM.BGM_PlACEMENT].audioClip;
		}else if(GameManager.instance.gamePhase == GamePhase.Raid){
			bgmAudioSource.clip = bgmAudioClipList [(int)BGM.BGM_INGAME].audioClip;
		}
		bgmAudioSource.loop = true;
		bgmAudioSource.Play ();
	}
}
