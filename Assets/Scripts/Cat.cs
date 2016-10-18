using UnityEngine;
using System.Collections;

public class Cat : Destructible {

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

		//TODO Solve sorting issue

		AssignStates ();
	}

	protected virtual void AssignStates () {
		
	}

	void Start () {
		progressingState.Start ();
		exploringState.Start ();
		panickingState.Start ();
		luredState.Start ();

		currentState = progressingState;
	}
	
	// Event propagating

	void Update () {
		currentState.Update ();
	}

	void FixedUpdate () {
		currentState.FixedUpdate ();
	}

	void OnCollisionEnter (Collision collision) {
		currentState.OnCollisionEnter (collision);
	}

	void OnCollisionExit (Collision collision) {
		currentState.OnCollisionExit (collision);
	}
}
