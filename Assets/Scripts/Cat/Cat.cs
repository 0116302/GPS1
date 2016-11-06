using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Cat : Destructible {

	public float walkingSpeed = 1.0f;
	public float enteringStaircaseSpeed = 1.25f;

	[HideInInspector] public float zPosition = 0.0f;

	[HideInInspector] public Animator animator;
	[HideInInspector] public new Rigidbody rigidbody;
	[HideInInspector] public CatController controller;

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
	[HideInInspector] public CatExploringState exploringState;
	[HideInInspector] public CatState panickingState;
	[HideInInspector] public CatLuredState luredState;

	public IList<CatStatusEffect> statusEffects = new List<CatStatusEffect> ();

	// Initialization

	void Awake () {
		animator = GetComponent<Animator> ();
		rigidbody = GetComponent<Rigidbody> ();
		controller = GetComponent<CatController> ();

		controller.onTargetReached += OnTargetReached;

		onDamaged += OnDamaged;
		onDestroyed += OnDestroyed;

		// Randomize position on the Z axis to prevent overlapping with parts of other cats
		//TODO Improve this to reduce overlap even more
		Vector3 position = transform.position;
		position.z = 0.02f * Random.Range (-5, 5);
		transform.position = position;
		zPosition = position.z;

		AssignStates ();
	}

	protected virtual void AssignStates () {
		progressingState = new CatDefaultProgressingState (this);
		exploringState = new CatDefaultExploringState (this);
		luredState = new CatDefaultLuredState (this);
		currentState = progressingState;
	}

	// Event handlers

	void OnDamaged (float amount, DamageType type) {
		//TODO Flash different colors for different damage types
		if (type == DamageType.Generic || type == DamageType.Impact || type == DamageType.Heat) {
			Flash (new Color32 (255, 0, 0, 255), 0.1f);

		} else if (type == DamageType.Cold) {
			Flash (new Color32 (40, 196, 255, 255), 0.1f);

		} else if (type == DamageType.Electricity) {
			Flash (new Color32 (255, 240, 0, 255), 0.1f);

		} else if (type == DamageType.Poison) {
			Flash (new Color32 (100, 240, 20, 255), 0.1f);
		}
	}

	void OnDestroyed () {
		GameManager.instance.enemiesLeft--;
		if (GameManager.instance.enemiesLeft == 0) {
			GUIManager.instance.Win ();
		}

		//TODO Death effects

		Destroy (gameObject);
	}

	// Methods

	public void AddStatusEffect (CatStatusEffect effect) {
		effect.Attach (this);
		effect.coroutine = StartCoroutine (statusEffectCoroutine (effect));
		statusEffects.Add (effect);
	}

	IEnumerator statusEffectCoroutine (CatStatusEffect effect) {
		effect.Start ();

		while (effect.elapsedTime < effect.duration) {
			effect.Tick ();

			effect.elapsedTime += 1.0f / effect.tickFrequency;
			yield return new WaitForSeconds (1.0f / effect.tickFrequency);
		}

		effect.End ();
		statusEffects.Remove (effect);
	}

	public int HasStatusEffect<T> () {
		int count = 0;

		foreach (CatStatusEffect e in statusEffects) {
			if (e.GetType () == typeof(T))
				count++;
		}

		return count;
	}

	public void SetTint (Color color) {
		head.material.color = color;
		torso.material.color = color;
		leftArm.material.color = color;
		leftForearm.material.color = color;
		leftHand.material.color = color;
		leftHandItem.material.color = color;
		rightArm.material.color = color;
		rightForearm.material.color = color;
		rightHand.material.color = color;
		rightHandItem.material.color = color;
		pelvis.material.color = color;
		leftThigh.material.color = color;
		leftLeg.material.color = color;
		leftFoot.material.color = color;
		rightThigh.material.color = color;
		rightLeg.material.color = color;
		rightFoot.material.color = color;
		tail.material.color = color;
	}

	public void Flash (Color color, float duration) {
		StartCoroutine (FlashCoroutine (color, duration));
	}

	IEnumerator FlashCoroutine (Color color, float duration) {
		SetTint (color);
		yield return new WaitForSeconds (duration);
		SetTint (Color.white);
	}

	public void SetOpacity (float opacity) {
		Color c;

		c = head.material.color;
		c.a = opacity;
		head.material.color = c;
		c = torso.material.color;
		c.a = opacity;
		torso.material.color = c;
		c = leftArm.material.color;
		c.a = opacity;
		leftArm.material.color = c;
		c = leftForearm.material.color;
		c.a = opacity;
		leftForearm.material.color = c;
		c = leftHand.material.color;
		c.a = opacity;
		leftHand.material.color = c;
		c = leftHandItem.material.color;
		c.a = opacity;
		leftHandItem.material.color = c;
		c = rightArm.material.color;
		c.a = opacity;
		rightArm.material.color = c;
		c = rightForearm.material.color;
		c.a = opacity;
		rightForearm.material.color = c;
		c = rightHand.material.color;
		c.a = opacity;
		rightHand.material.color = c;
		c = rightHandItem.material.color;
		c.a = opacity;
		rightHandItem.material.color = c;
		c = pelvis.material.color;
		c.a = opacity;
		pelvis.material.color = c;
		c = leftThigh.material.color;
		c.a = opacity;
		leftThigh.material.color = c;
		c = leftLeg.material.color;
		c.a = opacity;
		leftLeg.material.color = c;
		c = leftFoot.material.color;
		c.a = opacity;
		leftFoot.material.color = c;
		c = rightThigh.material.color;
		c.a = opacity;
		rightThigh.material.color = c;
		c = rightLeg.material.color;
		c.a = opacity;
		rightLeg.material.color = c;
		c = rightFoot.material.color;
		c.a = opacity;
		rightFoot.material.color = c;
		c = tail.material.color;
		c.a = opacity;
		tail.material.color = c;
	}

	public IEnumerator Fade (float opacity, float duration) {
		float elapsedTime = 0.0f;

		float startingOpacity = pelvis.material.color.a;

		while (elapsedTime < duration) {
			SetOpacity (Mathf.Lerp (startingOpacity, opacity, (elapsedTime / duration)));

			elapsedTime += Time.deltaTime;
			yield return null;
		}
		SetOpacity (opacity);
	}

	// Event propagating

	void Start () {
		progressingState.Start ();
		exploringState.Start ();
		//panickingState.Start ();
		luredState.Start ();
	}

	void Update () {
		currentState.Update ();
	}

	void FixedUpdate () {
		currentState.FixedUpdate ();
	}

	void OnTargetReached () {
		currentState.OnTargetReached ();
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
