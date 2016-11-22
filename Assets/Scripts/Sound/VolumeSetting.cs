using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class VolumeSetting : MonoBehaviour {
	public Sprite offNode;
	public Sprite onNode;
	public float volume;
	public string volumeName;
	public List<VolumeNodeScript> listVolumeNode;

	private static VolumeSetting _instance;
	public static VolumeSetting instance{
		get{
			return _instance;
		}
	}
	// Use this for initialization
	void Start () {
		if (_instance == null)
			_instance = this;
		if (PlayerPrefs.HasKey (volumeName)) {
			volume = PlayerPrefs.GetFloat (volumeName);
		} else {
			PlayerPrefs.SetFloat (volumeName,0.8f);
			volume = 0.8f;
		}
		float temp = volume * 10f;
		int node = int.Parse (temp.ToString ());
		if(node > 0)
			listVolumeNode [node - 1].ChangeNode ();
	}
	
	// Update is called once per frame
	void Update () {
		PlayerPrefs.SetFloat (volumeName,volume);
	}

	public void OnExit(){
		
	}

	public void DecreaseVolume(){
		if (volume <= 0.1f) {
			volume = 0;
			listVolumeNode [0].OffNode ();
		}else if (volume > 0.0f) {
			volume -= 0.1f;
			float temp = volume * 10f;
			int node = int.Parse (temp.ToString ());
			listVolumeNode [node-1].ChangeNode ();
			listVolumeNode [node-1].PlayAdjusVolume ();
		}
	}

	public void IncreaseVolume(){
		if (volume >= 1.0f) {
			volume = 1.0f;
		} else if (volume < 1.0f) {
			volume += 0.1f;
			float temp = volume * 10f;
			int node = int.Parse(temp.ToString ());
			listVolumeNode [node-1].ChangeNode ();
			listVolumeNode [node-1].PlayAdjusVolume ();
		}
	}
}
