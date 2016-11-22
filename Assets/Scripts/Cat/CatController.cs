using UnityEngine;
using System.Collections;

public class CatController : MonoBehaviour {

	public Cat cat;
	Animator animator;
	new Rigidbody rigidbody;

	public float movementSpeed = 1.0f;
	public bool frozen = false;

	Vector3 _target;
	public Vector3 target {
		get {
			return _target;
		}
	}

	bool _isMoving = false;
	public bool isMoving {
		get {
			return _isMoving;
		}
	}

	public bool reachedTarget {
		get {
			return transform.position == _target;
		}
	}

	public delegate void TargetReachedEventHandler();
	public event TargetReachedEventHandler onTargetReached;

	void Start () {
		animator = cat.animator;
		rigidbody = cat.rigidbody;
	}

	void FixedUpdate () {
		if (_isMoving && !frozen) {

			if (transform.position != _target) {
				
				Vector3 deltaPos = _target - transform.position;
				Vector3 velocity = deltaPos.normalized * movementSpeed;
				Vector3 movement = velocity * Time.fixedDeltaTime;

				if (movement.sqrMagnitude >= deltaPos.sqrMagnitude) {
					// Target reached
					transform.position = _target;
					rigidbody.velocity = Vector3.zero;
					_isMoving = false;

					if (onTargetReached != null) onTargetReached ();

				} else {

					// Target not yet reached
					rigidbody.velocity = velocity;
					animator.SetFloat ("movementSpeed", movementSpeed);

					if (movement.x > 0.0f)
						cat.FaceRight ();
					else if (movement.x < 0.0f)
						cat.FaceLeft ();
					
				}

			} else if (transform.position == _target) {
				// Target reached
				transform.position = _target;
				rigidbody.velocity = Vector3.zero;
				_isMoving = false;

				if (onTargetReached != null) onTargetReached ();
			}

		} else {
			rigidbody.velocity = Vector3.zero;
			animator.SetFloat ("movementSpeed", 0.0f);
		}
	}

	public void SetTarget (Vector3 target) {
		_target = target;
	}

	public void StartMoving () {
		_isMoving = true;
	}

	public void StopMoving () {
		_isMoving = false;
	}

	public void LockZ () {
		rigidbody.constraints =
			RigidbodyConstraints.FreezePositionZ |
			RigidbodyConstraints.FreezePositionY |
			RigidbodyConstraints.FreezeRotationX |
			RigidbodyConstraints.FreezeRotationY |
			RigidbodyConstraints.FreezeRotationZ;
	}

	public void LockX () {
		rigidbody.constraints =
			RigidbodyConstraints.FreezePositionX |
			RigidbodyConstraints.FreezePositionY |
			RigidbodyConstraints.FreezeRotationX |
			RigidbodyConstraints.FreezeRotationY |
			RigidbodyConstraints.FreezeRotationZ;
	}

	public void UnlockXZ () {
		rigidbody.constraints =
			RigidbodyConstraints.FreezePositionY |
			RigidbodyConstraints.FreezeRotationX |
			RigidbodyConstraints.FreezeRotationY |
			RigidbodyConstraints.FreezeRotationZ;
	}

	public void LockAll () {
		rigidbody.constraints = RigidbodyConstraints.FreezeAll;
	}

	public void UnlockAll () {
		rigidbody.constraints = RigidbodyConstraints.None;
	}
}
