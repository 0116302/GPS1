using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class CameraController : MonoBehaviour {

	public Room startingRoom;

	private Room _currentRoom;
	public Room currentRoom {
		get { return _currentRoom; }
	}

	public Text roomNameDisplay;

	bool _isInPlacementMode = false;

	void Start () {
		MoveToRoom (startingRoom);
	}
	
	void Update () {
		// Camera "switching"
		Room target = currentRoom;

		if (Input.GetButtonDown("Right") && target.roomRight) target = target.roomRight;
		if (Input.GetButtonDown("Left") && target.roomLeft) target = target.roomLeft;
		if (Input.GetButtonDown("Up") && target.roomAbove) target = target.roomAbove;
		if (Input.GetButtonDown("Down") && target.roomBelow) target = target.roomBelow;

		MoveToRoom (target);

		// Placement mode
		if (Input.GetKeyDown(KeyCode.E)) TogglePlacementMode ();
	}

	void MoveToRoom (Room target) {
		if (!target) return;

		if (_isInPlacementMode) {
			_currentRoom.placementPlane.gameObject.SetActive (false);
			target.placementPlane.gameObject.SetActive (true);
		}
		
		transform.position = target.cameraPosition.position;
		_currentRoom = target;

		if (roomNameDisplay)
			roomNameDisplay.text = _currentRoom.roomName + " ┘";
	}

	void EnterPlacementMode () {
		if (!currentRoom) return;
		currentRoom.placementPlane.gameObject.SetActive (true);
		_isInPlacementMode = true;
	}

	void ExitPlacementMode () {
		if (!currentRoom) return;
		currentRoom.placementPlane.gameObject.SetActive (false);
		_isInPlacementMode = false;
	}

	void TogglePlacementMode () {
		if (_isInPlacementMode)
			ExitPlacementMode (); 
		else 
			EnterPlacementMode ();
	}
}
