using UnityEngine;
using System.Collections;

public interface ITargeter {

	Transform target { get; }
	void SetTarget (Transform target);
}