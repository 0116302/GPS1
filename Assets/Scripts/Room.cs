using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Room : MonoBehaviour {

	[Header("Room Setup")]
	public string roomName = "Room";
	public Transform cameraPosition;
	public Transform placementGrid;

	[Header("Relative Position")]
	public Room roomRight;
	public Room roomLeft;
	public Room roomAbove;
	public Room roomBelow;

	public List<Staircase> staircases;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
