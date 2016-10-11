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

	private bool _isInPlacementMode = false;
	public bool isInPlacementMode {
		get { return _isInPlacementMode; }
	}

	public GameObject enemyPrefab;

	public float raycastDistance = 100.0f;

	private bool _zooming = false;
	private bool _zoomedIn = true;
	private Placeable _placing;
	public bool isOnLeft{
		get {return _placing.isOnLeft; }
	}
	private Vector3 _placementOrigin;
	private int _gridWidth = 0;
	private int _gridHeight = 0;
	private Vector3 _cursor;
	private bool _readyToPlace = false;
	private ITriggerable _mouseOver;

	void Start () {
		GetComponent<Camera> ().transparencySortMode = TransparencySortMode.Orthographic;
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

		// Camera zooming
		if (!_zooming) {
			if (Input.GetAxis ("Mouse ScrollWheel") < 0) {
				StartCoroutine (ZoomOut ());

			} else if (Input.GetAxis ("Mouse ScrollWheel") > 0) {
				StartCoroutine (ZoomIn ());
			}
		}

		// Toggle placement mode
		//if (Input.GetKeyDown(KeyCode.E) && _zoomedIn) TogglePlacementMode ();

		if (_isInPlacementMode && GameManager.gamePhase != GamePhase.Setup) {
			ExitPlacementMode ();
		}

		// Cursor raycast
		Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
		int layerMask = (_isInPlacementMode) ? (1 << 8) : (1 << 9); // Raycast only to either the placement grid OR interactive objects based on current mode

		RaycastHit hit;
		if (Physics.Raycast (ray, out hit, raycastDistance, layerMask, QueryTriggerInteraction.Collide)) {
			if (_isInPlacementMode) {
				if (_placing != null) {
					_cursor = hit.point - _placementOrigin;
					_cursor.z = _placementOrigin.z;

					// Snap to grid
					int cellX = Mathf.FloorToInt ((_cursor.x / (float)_placing.gridSnapX));
					int cellY = Mathf.CeilToInt ((_cursor.y / (float)_placing.gridSnapY));

					_cursor.x = (float)cellX * _placing.gridSnapX;
					_cursor.y = (float)cellY * _placing.gridSnapY;

					if (!_placing.gameObject.activeSelf)
						_placing.gameObject.SetActive (true);
					_placing.transform.position = _placementOrigin + _cursor + _placing.placementOffset;

					_readyToPlace = _placing.CanBePlacedHere (cellX, cellY, _gridWidth, _gridHeight);

				} else {
					_readyToPlace = false;
				}

			} else {
				ITriggerable triggerable = hit.collider.gameObject.GetComponent<ITriggerable> ();
				if (triggerable != null) {
					// Detect when the mouse is hovering over an interactive object
					if (_mouseOver != triggerable) {
						if (_mouseOver != null)
							_mouseOver.OnHoverExit ();

						_mouseOver = triggerable;
						_mouseOver.OnHoverEnter ();

					} else if (_mouseOver == triggerable) {
						_mouseOver.OnHoverStay ();
					}
				}
			}

		} else {
			if (_isInPlacementMode) {
				if (_placing != null && _placing.gameObject.activeSelf) {
					_placing.gameObject.SetActive (false);
				}

				_readyToPlace = false;

			} else {
				if (_mouseOver != null) {
					_mouseOver.OnHoverExit ();
					_mouseOver = null;
				}
			}
		}

		// Interacting with objects
		if (Input.GetButtonDown ("Interact")) {
			if (_isInPlacementMode && _readyToPlace) {
				if (_placing != null) {
					GameManager.cash -= _placing.cost;
					_placing.OnPlace ();
					_placing = null;

					ExitPlacementMode ();
				}

			} else if (GameManager.gamePhase == GamePhase.Raid) {
				if (_mouseOver != null) {
					_mouseOver.OnTrigger ();
				}
			}
		}
	}

	public IEnumerator ZoomOut () {
		_zooming = true;

		ExitPlacementMode ();

		Camera camera = GetComponent<Camera> ();
		float startingFov = camera.fieldOfView;
		float targetFov = 25.0f;

		float duration = 0.25f;
		float elapsedTime = 0.0f;

		while (elapsedTime < duration) {
			camera.fieldOfView = Mathf.Lerp (startingFov, targetFov, (elapsedTime / duration));

			elapsedTime += Time.deltaTime;
			yield return new WaitForEndOfFrame ();
		}
		camera.fieldOfView = targetFov;

		_zooming = false;
		_zoomedIn = false;
	}

	public IEnumerator ZoomIn () {
		_zooming = true;

		Camera camera = GetComponent<Camera> ();
		float startingFov = camera.fieldOfView;
		float targetFov = 12.0f;

		float duration = 0.25f;
		float elapsedTime = 0.0f;

		while (elapsedTime < duration) {
			camera.fieldOfView = Mathf.Lerp (startingFov, targetFov, (elapsedTime / duration));

			elapsedTime += Time.deltaTime;
			yield return new WaitForEndOfFrame ();
		}
		camera.fieldOfView = targetFov;

		_zooming = false;
		_zoomedIn = true;
	}

	public void MoveToRoom (Room target) {
		if (!target) return;

		if (_isInPlacementMode) {
			_currentRoom.placementGrid.gameObject.SetActive (false);
			target.placementGrid.gameObject.SetActive (true);
		}
		
		transform.position = target.cameraPosition.position;
		_currentRoom = target;
		_placementOrigin = _currentRoom.placementGrid.position - new Vector3 (_currentRoom.placementGrid.localScale.x / 2f, _currentRoom.placementGrid.localScale.y / -2f, 0f);
		_gridWidth = Mathf.FloorToInt (_currentRoom.placementGrid.localScale.x);
		_gridHeight = Mathf.FloorToInt (_currentRoom.placementGrid.localScale.y);

		if (roomNameDisplay)
			roomNameDisplay.text = _currentRoom.roomName + " ┘";
	}

	public void EnterPlacementMode () {
		if (!currentRoom) return;
		currentRoom.placementGrid.gameObject.SetActive (true);
		_isInPlacementMode = true;
	}

	public void ExitPlacementMode () {
		if (_placing != null) Destroy (_placing.gameObject);
		if (currentRoom != null) currentRoom.placementGrid.gameObject.SetActive (false);

		_isInPlacementMode = false;
	}

	public void TogglePlacementMode () {
		if (_isInPlacementMode)
			ExitPlacementMode (); 
		else 
			EnterPlacementMode ();
	}

	public void Place (GameObject prefab) {
		if (_placing != null) Destroy (_placing.gameObject);

		if (GameManager.gamePhase != GamePhase.Setup || !_zoomedIn) return;

		GameObject instance = GameObject.Instantiate (prefab);
		_placing = instance.GetComponent<Placeable> ();
		instance.SetActive (false);

		if (GameManager.cash >= _placing.cost) {
			EnterPlacementMode ();

		} else {
			Destroy (_placing.gameObject);
		}
	}
}
