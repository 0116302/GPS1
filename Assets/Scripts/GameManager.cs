using UnityEngine;
using System.Collections;

public enum GamePhase {
	Setup = 0,
	Raid
}

public static class GameManager {

	public static GamePhase gamePhase = GamePhase.Setup;
	public static int enemyCount = 10;
	public static int cash = 10000;
}
