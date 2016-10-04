using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyProgressingState : IEnemyState {

	private readonly Enemy enemy;

	private enum TargetType
	{
		None = 0,
		Room,
		Staircase
	}

	private int targetPathId = 0;
	private TargetType targetType = TargetType.None;
	private Staircase targetStaircase; // Only used when targetType is Staircase
	private bool enteringStaircase = false;

	private IList<int> ignoredPaths = new List<int> (); // List of paths to ignore (e.g. because the door is locked)

	public EnemyProgressingState (Enemy enemy) {
		this.enemy = enemy;
	}

	public void OnEnterRoom () {
		enteringStaircase = false;

		ignoredPaths.Clear ();

		DetermineTarget ();
	}

	public void DetermineTarget () {
		if (enemy.currentRoom != null) {
			// Determine if the current room is a dead end
			int unexploredPaths = 0;
			int exploredPaths = 0;
			int deadEnds = 0;
			int id = 0;

			Enemy.RoomStatus status;

			if (enemy.currentRoom.roomLeft != null && !ignoredPaths.Contains(++id)) {
				status = enemy.GetRoomStatus (enemy.currentRoom.roomLeft);
				switch (status) {
				case Enemy.RoomStatus.Unvisited:
					unexploredPaths++;
					break;
				case Enemy.RoomStatus.Visited:
					exploredPaths++;
					break;
				case Enemy.RoomStatus.DeadEnd:
					deadEnds++;
					break;
				}
			}

			if (enemy.currentRoom.roomRight != null && !ignoredPaths.Contains(++id)) {
				status = enemy.GetRoomStatus (enemy.currentRoom.roomRight);
				switch (status) {
				case Enemy.RoomStatus.Unvisited:
					unexploredPaths++;
					break;
				case Enemy.RoomStatus.Visited:
					exploredPaths++;
					break;
				case Enemy.RoomStatus.DeadEnd:
					deadEnds++;
					break;
				}
			}

			foreach (Staircase staircase in enemy.currentRoom.staircases) {
				if (staircase.destination != null && staircase.destination.room != null && !ignoredPaths.Contains(++id)) {
					status = enemy.GetRoomStatus (staircase.destination.room);
					switch (status) {
					case Enemy.RoomStatus.Unvisited:
						unexploredPaths++;
						break;
					case Enemy.RoomStatus.Visited:
						exploredPaths++;
						break;
					case Enemy.RoomStatus.DeadEnd:
						deadEnds++;
						break;
					}
				}
			}

			if (unexploredPaths + exploredPaths + ignoredPaths.Count <= 1) {
				enemy.SetRoomStatus (enemy.currentRoom, Enemy.RoomStatus.DeadEnd);

				if (unexploredPaths + exploredPaths + ignoredPaths.Count <= 0) {
					// This room has no way in or out. How'd we get here?
					// Stand still in confusion
					targetType = TargetType.None;
					enemy.walkDirection = Enemy.WalkDirection.None;
				}

			} else {
				enemy.SetRoomStatus (enemy.currentRoom, Enemy.RoomStatus.Visited);
			}

			// Randomly pick next room
			targetType = TargetType.None;

			Enemy.RoomStatus category;
			int choices = 0;

			if (unexploredPaths > 0) {
				// There are unexplored paths, choose randomly between them
				category = Enemy.RoomStatus.Unvisited;
				choices = unexploredPaths;

			} else if (exploredPaths > 0) {
				// There are no unexplored paths, choose one that isn't a known dead end
				category = Enemy.RoomStatus.Visited;
				choices = exploredPaths;

			} else {
				// There are only dead ends, life is meaningless
				category = Enemy.RoomStatus.DeadEnd;
				choices = deadEnds;
			}

			int choice = Random.Range (1, choices + 1);
			bool chosen = false;
			int count = 0;
			id = 0;

			if (!chosen && enemy.currentRoom.roomLeft != null) {
				id++;
				if (!ignoredPaths.Contains(id) && enemy.GetRoomStatus(enemy.currentRoom.roomLeft) == category && choice == ++count) {
					// Chosen path is through left door
					targetPathId = id;
					targetType = TargetType.Room;
					enemy.walkDirection = Enemy.WalkDirection.Left;
					Debug.Log ("Enemy has chosen left room!");
				}
			}

			if (!chosen && enemy.currentRoom.roomRight != null) {
				id++;
				if (!ignoredPaths.Contains(id) && enemy.GetRoomStatus(enemy.currentRoom.roomRight) == category && choice == ++count) {
					// Chosen path is through right door
					targetPathId = id;
					targetType = TargetType.Room;
					enemy.walkDirection = Enemy.WalkDirection.Right;
					Debug.Log ("Enemy has chosen right room!");
				}
			}

			if (!chosen) {
				foreach (Staircase staircase in enemy.currentRoom.staircases) {
					if (staircase.destination != null && staircase.destination.room != null) {
						id++;
						if (!ignoredPaths.Contains(id) && enemy.GetRoomStatus(staircase.destination.room) == category && choice == ++count) {
							// Chosen path is through a staircase
							targetPathId = id;
							targetType = TargetType.Staircase;
							targetStaircase = staircase;
							enemy.walkDirection = (staircase.transform.position.x < enemy.transform.position.x) ? Enemy.WalkDirection.Left : Enemy.WalkDirection.Right;
							Debug.Log ("Enemy has chosen a staircase!");
							break;
						}
					}
				}
			}

		} else {
			targetPathId = 0;
			targetType = TargetType.None;
		}
	}

	public void DoNotRepeat() {
		ignoredPaths.Add (targetPathId);
	}

	public void UpdateState () {
		// If target is a starcase and we've arrived in front of it, enter it
		if (targetType == TargetType.Staircase && !enteringStaircase && Mathf.Abs(enemy.transform.position.x - targetStaircase.transform.position.x) < 0.1f) {
			enemy.EnterStaircase (targetStaircase);
			enteringStaircase = true;
		}

		if (Input.GetKeyDown (KeyCode.P)) {
			foreach (int i in ignoredPaths) {
				Debug.Log (i);
			}
		}
	}

	public void ToExploreState () {

	}

	public void ToProgressingState () {
		// Already in this state
	}
}
