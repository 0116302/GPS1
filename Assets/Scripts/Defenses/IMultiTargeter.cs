using UnityEngine;
using System.Collections;

public interface IMultiTargeter {

	void AddTarget (Transform target);
	void RemoveTarget (Transform target);
}
