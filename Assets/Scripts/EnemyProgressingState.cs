using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyProgressingState : IEnemyState {

	private readonly Enemy enemy;

	private enum TargetType
	{
		None = 0,
		Door,
		Staircase
	}

	private Room lastRoom;

	private TargetType targetType = TargetType.None;
	private Door targetDoor; // Only used when targetType is Door
	private Staircase targetStaircase; // Only used when targetType is Staircase
	private Room targetRoom;

	public EnemyProgressingState (Enemy enemy) {
		this.enemy = enemy;
	}

	public void DetermineTarget () {
		if (enemy.currentRoom != null) {
			// Determine if the current room is a dead end
			int unexploredPaths = 0;
			int exploredPaths = 0;
			int deadEnds = 0;
			int closedUnexplored = 0;
			int closedExplored = 0;
			int closedDeadEnds = 0;

			Enemy.RoomStatus status;

			if (enemy.currentRoom.roomLeft != null && enemy.currentRoom.leftDoor != null) {
				status = enemy.GetRoomStatus (enemy.currentRoom.roomLeft);
				switch (status) {
				case Enemy.RoomStatus.Unvisited:
					if (enemy.currentRoom.leftDoor.isOpen)unexploredPaths++; else closedUnexplored++;
					break;
				case Enemy.RoomStatus.Visited:
					if (enemy.currentRoom.leftDoor.isOpen) exploredPaths++; else closedExplored++;
					break;
				case Enemy.RoomStatus.DeadEnd:
					if (enemy.currentRoom.leftDoor.isOpen) deadEnds++; else closedDeadEnds++;
					break;
				}
			}

			if (enemy.currentRoom.roomRight != null && enemy.currentRoom.rightDoor != null) {
				status = enemy.GetRoomStatus (enemy.currentRoom.roomRight);
				switch (status) {
				case Enemy.RoomStatus.Unvisited:
					if (enemy.currentRoom.rightDoor.isOpen) unexploredPaths++; else closedUnexplored++;
					break;
				case Enemy.RoomStatus.Visited:
					if (enemy.currentRoom.rightDoor.isOpen) exploredPaths++; else closedExplored++;
					break;
				case Enemy.RoomStatus.DeadEnd:
					if (enemy.currentRoom.rightDoor.isOpen) deadEnds++; else closedDeadEnds++;
					break;
				}
			}

			foreach (Staircase staircase in enemy.currentRoom.staircases) {
				if (staircase.destination != null && staircase.destination.room != null) {
					status = enemy.GetRoomStatus (staircase.destination.room);
					switch (status) {
					case Enemy.RoomStatus.Unvisited:
						if (staircase.door.isOpen && staircase.destination.door.isOpen) unexploredPaths++; else closedUnexplored++;
						break;
					case Enemy.RoomStatus.Visited:
						if (staircase.door.isOpen && staircase.destination.door.isOpen) exploredPaths++; else closedExplored++;
						break;
					case Enemy.RoomStatus.DeadEnd:
						if (staircase.door.isOpen && staircase.destination.door.isOpen) deadEnds++; else closedDeadEnds++;
						break;
					}
				}
			}

			if (unexploredPaths + closedUnexplored + exploredPaths + closedExplored <= 1) {
				enemy.SetRoomStatus (enemy.currentRoom, Enemy.RoomStatus.DeadEnd);

			} else {
				enemy.SetRoomStatus (enemy.currentRoom, Enemy.RoomStatus.Visited);
			}

			if (unexploredPaths + exploredPaths + deadEnds == 0) {
				// This room has no way in or out
				// Stand still and wait for rescue
				targetType = TargetType.None;
				enemy.walkDirection = Enemy.WalkDirection.None;
			}

			// Randomly pick next room from available choices
			targetType = TargetType.None;

			Enemy.RoomStatus category;
			int choices = 0;

			if (unexploredPaths > 0) {
				// There are unexplored paths, choose randomly between them
				category = Enemy.RoomStatus.Unvisited;
				choices = unexploredPaths;

			} else if (exploredPaths > 0) {
				// There are no unexplored paths, choose one that at least isn't a known dead end
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

			if (!chosen && enemy.currentRoom.roomLeft != null && enemy.currentRoom.leftDoor != null && enemy.currentRoom.leftDoor.isOpen) {
				if (enemy.GetRoomStatus(enemy.currentRoom.roomLeft) == category && choice == ++count) {
					// Chosen path is through left door
					targetType = TargetType.Door;
					targetDoor = enemy.currentRoom.leftDoor;
					targetRoom = enemy.currentRoom.roomLeft;
					enemy.walkDirection = Enemy.WalkDirection.Left;
					//Debug.Log ("Enemy has chosen the left door");
				}
			}

			if (!chosen && enemy.currentRoom.roomRight != null && enemy.currentRoom.rightDoor != null && enemy.currentRoom.rightDoor.isOpen) {
				if (enemy.GetRoomStatus(enemy.currentRoom.roomRight) == category && choice == ++count) {
					// Chosen path is through right door
					targetType = TargetType.Door;
					targetDoor = enemy.currentRoom.rightDoor;
					targetRoom = enemy.currentRoom.roomRight;
					enemy.walkDirection = Enemy.WalkDirection.Right;
					//Debug.Log ("Enemy has chosen the right door");
				}
			}

			if (!chosen) {
				foreach (Staircase staircase in enemy.currentRoom.staircases) {
					if (staircase.destination != null && staircase.destination.room != null && staircase.door.isOpen && staircase.destination.door.isOpen) {
						if (enemy.GetRoomStatus(staircase.destination.room) == category && choice == ++count) {
							// Chosen path is through a staircase
							targetType = TargetType.Staircase;
							targetStaircase = staircase;
							targetRoom = staircase.destination.room;
							enemy.walkDirection = (staircase.transform.position.x <= enemy.transform.position.x) ? Enemy.WalkDirection.Left : Enemy.WalkDirection.Right;
							//Debug.Log ("Enemy has chosen staircase " + staircase.name);
							break;
						}
					}
				}
			}

		} else {
			targetType = TargetType.None;
		}
	}

	public void UpdateState () {
		// If in new room, find new target
		if (enemy.currentRoom != lastRoom) {
			DetermineTarget ();

			lastRoom = enemy.currentRoom;
		}

		// If target invalid, find new target
		if (targetType == TargetType.None || (targetType == TargetType.Door && !targetDoor.isOpen) || (targetType == TargetType.Staircase && (!targetStaircase.door.isOpen || !targetStaircase.destination.door.isOpen))) {
			DetermineTarget ();
		}

		// If target is a starcase and we've arrived in front of it, enter it
		if (targetType == TargetType.Staircase && Mathf.Abs(enemy.transform.position.x - targetStaircase.transform.position.x) < 0.1f) {
			enemy.EnterStaircase (targetStaircase);
		}
	}

	public void ToExploreState () {

	}

	public void ToProgressingState () {
		// Already in this state
	}
}
