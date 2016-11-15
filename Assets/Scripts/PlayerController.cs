using UnityEngine;
using System.Collections;

public enum CameraMode {
	Overview = 0,
	RoomView
}

public enum PlayerMode {
	Observation = 0,
	Placement,
	Removal,
	Activation
}

public class PlayerController : MonoBehaviour {

	[Header ("Camera Settings")]
	public Camera mainCamera;
	public Transform overviewPosition;
	public float overviewFOV = 25.0f;

	private Coroutine cameraTransition;

	private Room _currentRoom;
	public Room currentRoom {
		get { return _currentRoom; }
	}

	private CameraMode _cameraMode = CameraMode.Overview;
	public CameraMode cameraMode {
		get { return _cameraMode; }
	}

	private PlayerMode _playerMode = PlayerMode.Observation;
	public PlayerMode playerMode {
		get { return _playerMode; }
	}

	[Header ("Effect Settings")]
	public float cameraZoomDuration = 0.25f;
	public float cameraSwitchDuration = 0.05f;
	public float gridFadeDuration = 0.5f;

	[Header ("Advanced")]
	public float raycastDistance = 100.0f;

	private Placeable _selected;
	private Vector3 _placementOrigin; // Top-left of grid in world space
	private int _gridWidth = 0;
	private int _gridHeight = 0;
	private int _cellX = 0;
	private int _cellY = 0;
	private Vector3 _cursor;
	private bool _canPlace = false;
	private ITriggerable _mouseOver;

	// Use this for initialization
	void Awake () {
		mainCamera.transparencySortMode = TransparencySortMode.Orthographic;
	}

	void Start () {
		mainCamera.transform.position = overviewPosition.position;
		mainCamera.fieldOfView = overviewFOV;
	}
	
	// Update is called once per frame
	void Update () {
		Ray ray = mainCamera.ScreenPointToRay (Input.mousePosition);
		int layerMask = 0;
		RaycastHit hit;

		// Camera controls
		if (_cameraMode == CameraMode.Overview) {
			Room room = null;

			// Raycast to detect if the mouse is hovering over a room
			layerMask = 1 << LayerMask.NameToLayer ("Rooms");
			if (Physics.Raycast (ray, out hit, raycastDistance, layerMask, QueryTriggerInteraction.Collide)) {
				if (hit.collider.CompareTag ("Room")) {
					room = hit.collider.GetComponent<Room> ();
				}
			}

			// Focus on selected room on mouse click
			if ((Input.GetButtonDown ("Interact") || Input.GetAxis ("Mouse ScrollWheel") > 0) && room != null) {
				SwitchToRoomView (room);
			}

		} else if (_cameraMode == CameraMode.RoomView) {

			if (Input.GetAxis ("Mouse ScrollWheel") < 0) {
				SwitchToOverview ();

			} else {
				Room target = currentRoom;

				if (Input.GetButtonDown("Right") && target.roomRight) target = target.roomRight;
				if (Input.GetButtonDown("Left") && target.roomLeft) target = target.roomLeft;
				if (Input.GetButtonDown("Up") && target.roomAbove) target = target.roomAbove;
				if (Input.GetButtonDown("Down") && target.roomBelow) target = target.roomBelow;

				if (target != _currentRoom) SwitchToRoomView (target);
			}

			if (playerMode == PlayerMode.Placement) {
				if (_selected == null) EnterObservationMode ();

				layerMask = 1 << LayerMask.NameToLayer ("Placement Grid");
				if (Physics.Raycast (ray, out hit, raycastDistance, layerMask, QueryTriggerInteraction.Collide) && hit.collider.transform == _currentRoom.placementGrid) {
					_cursor = hit.point - _placementOrigin;
					_cursor.z = _placementOrigin.z;

					float halfWidth = _selected.width / 2;
					bool oddWidth = _selected.width % 2 != 0;

					float halfHeight = _selected.height / 2;
					bool oddHeight = _selected.height % 2 != 0;

					// Snap to grid
					_cellX = Mathf.FloorToInt ((_cursor.x / (float)_selected.horizontalSnap));
					_cellY = Mathf.CeilToInt ((_cursor.y / (float)_selected.verticalSnap));

					_cellX -= Mathf.FloorToInt (halfWidth);
					_cellY += Mathf.FloorToInt (halfHeight);

					_cursor.x = (float)_cellX * _selected.horizontalSnap + halfWidth;
					_cursor.y = (float)_cellY * _selected.verticalSnap - halfHeight;

					if (oddWidth)
						_cursor.x += 0.5f;
					if (oddHeight)
						_cursor.y -= 0.5f;

					if (!_selected.gameObject.activeSelf) _selected.gameObject.SetActive (true);
					_selected.transform.position = _placementOrigin + _cursor + _selected.placementOffset;

					_canPlace = _selected.CanBePlacedHere (_cellX, _cellY, _gridWidth, _gridHeight);

					if (_canPlace)
						_selected.HighlightPositive ();
					else
						_selected.HighlightNegative ();

				} else {
					if (_selected != null) {
						_selected.gameObject.SetActive (false);
					}

					_canPlace = false;
				}

				if (Input.GetButtonDown ("Interact") && _selected != null && _canPlace) {
					GameManager.instance.cash -= _selected.cost;

					_selected.HideHighlight ();
					_selected.room = _currentRoom;
					_selected.placed = true;
					_selected.OnPlace ();
					_selected = null;

					EnterObservationMode ();
				}

			} else if (playerMode == PlayerMode.Removal) {
				
				layerMask = 1 << LayerMask.NameToLayer ("Grid Objects");
				if (Physics.Raycast (ray, out hit, raycastDistance, layerMask, QueryTriggerInteraction.Collide)) {

					Placeable placeable = hit.collider.GetComponent<Placeable> ();
					if (placeable != null) {

						if (_selected != placeable) {
							if (_selected != null) {
								_selected.HideHighlight ();
							}

							_selected = placeable;
							_selected.HighlightNegative ();
						}

					}

				} else {
					if (_selected != null) {
						_selected.HideHighlight ();
						_selected = null;
					}
				}

				if (Input.GetButtonDown ("Interact") && _selected != null) {
					if (_selected.removable) {
						if (_selected.refundable)
							GameManager.instance.cash += _selected.cost;

						_selected.OnRemove ();

						Destroy (_selected.gameObject);

						EnterObservationMode ();
					}
				}

			} else if (playerMode == PlayerMode.Activation) {

				layerMask = 1 << LayerMask.NameToLayer ("Interactive Objects");
				if (Physics.Raycast (ray, out hit, raycastDistance, layerMask, QueryTriggerInteraction.Collide)) {
					
					ITriggerable triggerable = hit.collider.GetComponent<ITriggerable> ();
					if (triggerable != null) {
						
						if (_mouseOver != triggerable) {
							if (_mouseOver != null)
								_mouseOver.OnHoverExit ();

							_mouseOver = triggerable;
							_mouseOver.OnHoverEnter ();

						} else {
							_mouseOver.OnHoverStay ();
						}

					}

				} else {
					if (_mouseOver != null) {
						_mouseOver.OnHoverExit ();
						_mouseOver = null;
					}
				}

				if (Input.GetButtonDown ("Interact") && _mouseOver != null) {
					if (_mouseOver != null) {
						_mouseOver.OnTrigger ();
					}
				}

			}

		}
	}

	public void SwitchToRoomView (Room room) {
		if (_cameraMode == CameraMode.Overview) {
			if (cameraTransition != null) StopCoroutine (cameraTransition);
			cameraTransition = StartCoroutine (MoveCamera (room.cameraPosition.position, room.cameraFOV, cameraZoomDuration));

		} else {
			if (cameraTransition != null) StopCoroutine (cameraTransition);
			cameraTransition = StartCoroutine (MoveCamera (room.cameraPosition.position, room.cameraFOV, cameraSwitchDuration));

			if (_playerMode == PlayerMode.Placement || _playerMode == PlayerMode.Removal) {
				if (room.canPlaceDefenses) {
					_currentRoom.fadeOutGrid (gridFadeDuration);
					room.fadeInGrid (gridFadeDuration);

				} else {
					EnterObservationMode ();
				}
			}
		}

		GUIManager.instance.SwitchToRoom (room);

		_currentRoom = room;
		_placementOrigin = _currentRoom.placementGrid.position - new Vector3 (_currentRoom.placementGrid.localScale.x / 2f, _currentRoom.placementGrid.localScale.y / -2f, 0f);
		_gridWidth = Mathf.FloorToInt (_currentRoom.placementGrid.localScale.x);
		_gridHeight = Mathf.FloorToInt (_currentRoom.placementGrid.localScale.y);
		_cameraMode = CameraMode.RoomView;
	}

	public void SwitchToOverview () {
		if (_playerMode == PlayerMode.Placement || _playerMode == PlayerMode.Removal) EnterObservationMode ();
		if (cameraTransition != null) StopCoroutine (cameraTransition);
		cameraTransition = StartCoroutine (MoveCamera (overviewPosition.position, overviewFOV, cameraZoomDuration));

		GUIManager.instance.SwitchToOverview ();

		_currentRoom = null;
		_cameraMode = CameraMode.Overview;
	}

	public void EnterObservationMode () {
		if (_playerMode == PlayerMode.Placement || _playerMode == PlayerMode.Removal) _currentRoom.fadeOutGrid (gridFadeDuration);
		if (_selected != null) {
			Destroy (_selected.gameObject);
		}

		_playerMode = PlayerMode.Observation;
	}

	public void EnterPlacementMode (GameObject placeableObject) {
		if (GameManager.instance.gamePhase != GamePhase.Setup) return; // Can't enter placement mode unless in setup phase
		if (_cameraMode == CameraMode.Overview) return; // Can't enter placement mode while in overview
		if (_currentRoom == null || !_currentRoom.canPlaceDefenses) return; // Can't enter placement mode in rooms that don't allow it

		if (_selected != null) {
			Destroy (_selected.gameObject);
		}

		Placeable template;
		if ((template = placeableObject.GetComponent<Placeable> ()) != null && GameManager.instance.cash >= template.cost) {

			GameObject instance = GameObject.Instantiate (placeableObject);
			_selected = instance.GetComponent<Placeable> ();
			instance.SetActive (false);
			
			if (_playerMode != PlayerMode.Placement && _playerMode != PlayerMode.Removal) _currentRoom.fadeInGrid (gridFadeDuration);
			_playerMode = PlayerMode.Placement;

		}
	}

	public void EnterRemovalMode () {
		if (GameManager.instance.gamePhase != GamePhase.Setup) return; // Can't enter removal mode unless in setup phase
		if (_cameraMode == CameraMode.Overview) return; // Can't enter removal mode while in overview

		if (_playerMode != PlayerMode.Placement && _playerMode != PlayerMode.Removal) _currentRoom.fadeInGrid (gridFadeDuration);
		if (_selected != null) {
			Destroy (_selected.gameObject);
		}

		_playerMode = PlayerMode.Removal;
	}

	public void EnterActivationMode () {
		if (_playerMode == PlayerMode.Placement || _playerMode == PlayerMode.Removal) _currentRoom.fadeOutGrid (gridFadeDuration);
		if (_selected != null) {
			Destroy (_selected.gameObject);
		}

		_playerMode = PlayerMode.Activation;
	}

	IEnumerator MoveCamera (Vector3 destination, float destinationFOV, float duration) {
		float elapsedTime = 0.0f;

		Vector3 startingPosition = mainCamera.transform.position;
		float startingFOV = mainCamera.fieldOfView;

		while (elapsedTime < duration) {
			mainCamera.transform.position = Vector3.Lerp (startingPosition, destination, (elapsedTime / duration));
			mainCamera.fieldOfView = Mathf.Lerp (startingFOV, destinationFOV, (elapsedTime / duration));

			elapsedTime += Time.deltaTime;
			yield return null;
		}
		mainCamera.transform.position = destination;
		mainCamera.fieldOfView = destinationFOV;
	}
}
