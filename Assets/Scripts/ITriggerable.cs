using UnityEngine;
using System.Collections;

public interface ITriggerable {

	void OnHoverEnter ();
	void OnHoverStay();
	void OnHoverExit ();
	void OnTrigger ();
}
