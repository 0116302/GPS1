using UnityEngine;
using System.Collections;

public class Cat : Destructible {

	public float walkingSpeed = 1.0f;

	[HideInInspector]
	public Animator animator;

	[Header ("Parts")]
	public SpriteRenderer head;
	public SpriteRenderer torso;
	public SpriteRenderer leftArm;
	public SpriteRenderer leftForearm;
	public SpriteRenderer leftHand;
	public SpriteRenderer leftHandItem;
	public SpriteRenderer rightArm;
	public SpriteRenderer rightForearm;
	public SpriteRenderer rightHand;
	public SpriteRenderer rightHandItem;
	public SpriteRenderer pelvis;
	public SpriteRenderer leftThigh;
	public SpriteRenderer leftLeg;
	public SpriteRenderer leftFoot;
	public SpriteRenderer rightThigh;
	public SpriteRenderer rightLeg;
	public SpriteRenderer rightFoot;
	public SpriteRenderer tail;

	[HideInInspector] public CatState currentState;
	[HideInInspector] public CatProgressingState progressingState;
	[HideInInspector] public CatState exploringState;
	[HideInInspector] public CatState panickingState;
	[HideInInspector] public CatState luredState;

	// Initialization

	void Awake () {
		animator = GetComponent<Animator> ();

		// Randomize position on the Z axis to prevent overlapping with parts of other cats
		//TODO Improve this to reduce overlap even more
		Vector3 newPos = transform.position;
		newPos.z = 0.02f * Random.Range (-5, 5);
		transform.position = newPos;

		AssignStates ();
	}

	protected virtual void AssignStates () {
		progressingState = new CatDefaultProgressingState (this);
		exploringState = new CatDefaultExploringState (this);
		currentState = progressingState;
	}

	// Event propagating

	void Start () {
		progressingState.Start ();
		exploringState.Start ();
		//panickingState.Start ();
		//luredState.Start ();
	}

	void Update () {
		currentState.Update ();
	}

	void FixedUpdate () {
		currentState.FixedUpdate ();
	}

	void OnCollisionEnter (Collision collision) {
		currentState.OnCollisionEnter (collision);
	}

	void OnCollisionStay (Collision collision) {
		currentState.OnCollisionStay (collision);
	}

	void OnCollisionExit (Collision collision) {
		currentState.OnCollisionExit (collision);
	}

	void OnTriggerEnter (Collider collider) {
		currentState.OnTriggerEnter (collider);
	}

	void OnTriggerStay (Collider collider) {
		currentState.OnTriggerStay (collider);
	}

	void OnTriggerExit (Collider collider) {
		currentState.OnTriggerExit (collider);
	}
}
