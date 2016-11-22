using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class VolumeNodeScript : MonoBehaviour {
	public VolumeNodeScript parentVolumeNode;
	public VolumeNodeScript childVolumeNode;
	public Image nodeImage;
	public float nodeVolume;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void HoverEnter(){
		if (Input.GetMouseButton (0)) {
			ChangeNode ();
		}
		if (Input.GetMouseButtonUp (0)) {
			PlayAdjusVolume ();
			Debug.Log ("hello");
		}
	}

	public void ChangeNode(){
		nodeImage.sprite = VolumeSetting.instance.onNode;
		ChangeParentNode ();
		ChangeChildNode ();
		gameObject.GetComponentInParent<VolumeSetting> ().volume = nodeVolume;
	}

	public void ChangeParentNode(){		
		if (parentVolumeNode != null) {
			parentVolumeNode.nodeImage.sprite = VolumeSetting.instance.onNode;
			parentVolumeNode.ChangeParentNode ();
		}
			
	}
	public void ChangeChildNode(){		
		if (childVolumeNode != null) {
			childVolumeNode.nodeImage.sprite = VolumeSetting.instance.offNode;
			childVolumeNode.ChangeChildNode ();
		}
	}
	public void OffNode(){
		nodeImage.sprite = VolumeSetting.instance.offNode;
	}
	public void PlayAdjusVolume(){
		SettingWindow.instance.volume = nodeVolume;
		SettingWindow.instance.PlayVolumeClip ();
	}
}
