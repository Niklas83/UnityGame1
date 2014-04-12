using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public abstract class BaseMover : MonoBehaviour
{
	public bool isPusher = false; 					// Can push stuff around
	public SoundEffectPlayer soundEffectPLayer;

	// Public properties
	public bool IsMoving { get { return isMoving; } }
	// This position will always be aligned with the grid, either transform position or the target.
	public Vector3 Position { get { return isMoving ? currentTargetPosition : transform.position; } }
	
	// Protected members
	protected Vector3 currentTargetPosition;
	protected bool isMoving = false;
	protected BaseUnit unit;
	protected GridManager gridManager;
	protected Queue<Vector2> moveQueue;

	public void Start() {
		Floor floor = Helper.Find<Floor>("Floor");
		gridManager = floor.GridManager;
		unit = GetComponent<BaseUnit>();

		soundEffectPLayer = GetComponentInChildren<SoundEffectPlayer>();
	}

	protected bool CanMove(BaseTile tile, int xDir, int zDir) {
		bool canMove = true;

		foreach (BaseUnit u in tile.OccupyingUnits(unit)) {
			if (canMove) {
				canMove = unit == u || u.CanWalkOn(gameObject.tag); // You can walk here if it's to yourself or to a "walkable" unit.
				if (!canMove && isPusher) {
					// If not a walkable, check if you are a 'Pusher' and it can be moved.
					BaseMover mover = u.GetComponent<BaseMover>();
					if (mover != null) {
						canMove = mover.TryMove(xDir, zDir);
					} 
				}
			}
			unit.OnCollided(u);
			u.OnCollided(unit);
		}
		return canMove && tile.CanWalkOn(unit);
	}

	public abstract bool TryMove(int xDir, int zDir);
}