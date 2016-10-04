using UnityEngine;
using System.Collections;

public interface IEnemyState {

	void UpdateState();
	void ToExploreState();
	void ToProgressingState();
}
