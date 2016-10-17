using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Enemy : Destructible {

	public TextMesh healthDisplay; // TEMPORARY

	[HideInInspector] public Animator animator;

	public enum WalkDirection
	{
		Left = -1,
		None = 0,
		Right = 1
	}

	[Header("Movement")]
	public float walkSpeed = 1.0f;
	public WalkDirection walkDirection = WalkDirection.Right;

	[Header("Death")]
	public Transform head;
	public GameObject decapitatedHeadPrefab;

	private Room _currentRoom;
	public Room currentRoom {
		get { return _currentRoom; }
	}

	private bool _isEnteringStaircase;

	public enum RoomStatus {
		Unvisited = 0,
		Visited,
		DeadEnd
	}

	private IDictionary<Room, RoomStatus> roomStatus = new Dictionary<Room, RoomStatus> ();

	[HideInInspector] public IEnemyState currentState;
	[HideInInspector] public EnemyProgressingState progressingState;

	[HideInInspector] public Staircase staircase;

	void Awake () {
		animator = GetComponent<Animator> ();

		progressingState = new EnemyProgressingState (this);
	}

	// Use this for initialization
	void Start () {
		onDeath.AddListener (OnDeath);

		currentState = progressingState;
	}
	
	// Update is called once per frame
	void Update () {
		currentState.UpdateState ();

		// Flip the enemy to face the right direction
		if ((walkDirection == WalkDirection.Right && transform.localScale.x < 0) || (walkDirection == WalkDirection.Left && transform.localScale.x > 0)) {
			Vector3 newScale = transform.localScale;
			newScale.x = -newScale.x;
			transform.localScale = newScale;
		}

		if (walkDirection != WalkDirection.None) {
			animator.SetFloat ("MovementSpeed", walkSpeed);
		} else {
			animator.SetFloat ("MovementSpeed", 0.0f);
		}

		if (healthDisplay != null) {
			healthDisplay.text = new string ('♥', Mathf.CeilToInt(this.health / this.maximumHealth * 3.0f));
		}
	}

	void FixedUpdate () {
		if (walkDirection != WalkDirection.None) {
			transform.Translate (new Vector3(walkSpeed * Time.deltaTime, 0.0f, 0.0f));
		}
	}

	void OnTriggerEnter (Collider other) {
		if (other.CompareTag("Room")) {
			Room room = other.GetComponent<Room> ();
			if (room != currentRoom) {
				_currentRoom = other.GetComponent<Room> ();
				//Debug.Log ("Enemy has entered " + _currentRoom.roomName);

//				if (_currentRoom.roomName == "Control Room") {
//					GameObject.FindObjectOfType<GUIManager> ().Lose ();
//				}
			}
		}
	}

//	void OnCollisionEnter (Collision collision) {
//		if (collision.gameObject.CompareTag("Door")) {
//			if ((collision.transform.position.x < transform.position.x && walkDirection == WalkDirection.Left) || (collision.transform.position.x > transform.position.x && walkDirection == WalkDirection.Right)) {
//				StartCoroutine (TryOpenDoor ());
//			}
//		}
//	}
//
//	private IEnumerator TryOpenDoor () {
//		walkDirection = WalkDirection.None;
//		animator.SetFloat ("MovementSpeed", 0.0f);
//		animator.SetBool ("TryingToOpenDoor", true);
//		progressingState.DoNotRepeat ();
//
//		yield return new WaitForSeconds (2.0f);
//
//		animator.SetBool ("TryingToOpenDoor", false);
//		progressingState.DetermineTarget ();
//	}

	void OnDeath () {
		Rigidbody decapitatedHead = ((GameObject) GameObject.Instantiate (decapitatedHeadPrefab, head.transform.position, head.transform.rotation)).GetComponent<Rigidbody> ();
		decapitatedHead.AddForce (new Vector3(-0.5f, 2f, 0f), ForceMode.Impulse);

//		GameManager.enemyCount--;
//		if (GameManager.enemyCount == 0) {
//			// Win
//			GameObject.FindObjectOfType<GUIManager> ().Win ();
//
//		}

		Destroy (gameObject);
	}

	public RoomStatus GetRoomStatus (Room room) {
		RoomStatus status;
		if (roomStatus.TryGetValue (room, out status)) {
			return status;
		} else {
			return RoomStatus.Unvisited;
		}
	}

	public void SetRoomStatus (Room room, RoomStatus status) {
		//Debug.Log ("Enemy set status of " + room.roomName + " to " + status.ToString());
		roomStatus[room] = status;
	}

	public void EnterStaircase (Staircase staircase) {
		StartCoroutine (EnterStaircaseCoroutine (staircase));
	}

	private IEnumerator EnterStaircaseCoroutine (Staircase staircase) {
		if (_isEnteringStaircase) yield break;
		_isEnteringStaircase = true;

		Vector3 pos = transform.position;

		float duration = 1.0f;
		float elapsedTime = 0.0f;
		float staircaseDepth = 5.25f;

		// Align
		float startingX = pos.x;
		pos.y = staircase.teleportPosition.position.y;
		while (elapsedTime < 0.1f) {
			pos.x = Mathf.Lerp (startingX, staircase.teleportPosition.position.x, (elapsedTime / 0.1f));
			transform.position = pos;

			elapsedTime += Time.fixedDeltaTime;
			yield return new WaitForFixedUpdate ();
		}
		pos.x = staircase.teleportPosition.position.x;
		transform.position = pos;

		// Walk in
		elapsedTime = 0.0f;
		while (elapsedTime < duration) {
			pos.z = Mathf.Lerp (0.0f, staircaseDepth, (elapsedTime / duration));
			transform.position = pos;

			elapsedTime += Time.fixedDeltaTime;
			yield return new WaitForFixedUpdate ();
		}
		pos.z = 5.25f;
		transform.position = pos;

		// Teleport
		pos = staircase.destination.teleportPosition.position;
		transform.position = pos;

		// Walk out
		elapsedTime = 0.0f;
		while (elapsedTime < duration) {
			pos.z = Mathf.Lerp (staircaseDepth, 0.0f, (elapsedTime / duration));
			transform.position = pos;

			elapsedTime += Time.fixedDeltaTime;
			yield return new WaitForFixedUpdate ();
		}
		pos.z = 0.0f;
		transform.position = pos;

		_isEnteringStaircase = false;
	}
}
