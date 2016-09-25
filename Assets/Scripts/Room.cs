using UnityEngine;
using System.Collections;

public class Room : MonoBehaviour {

	[Header("Room Setup")]
	public string roomName = "Room";
	public Transform cameraPosition;
	public Transform placementPlane;

	[Header("Relative Position")]
	public Room roomRight;
	public Room roomLeft;
	public Room roomAbove;
	public Room roomBelow;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
