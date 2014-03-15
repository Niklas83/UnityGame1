using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public abstract class BaseMover : MonoBehaviour
{
	public bool IsPusher = false; // Can push stuff around

	// Public properties
	public bool IsMoving { get { return mIsMoving; } }
	// This position will always be aligned with the grid, either transform position or the target.
	public Vector3 Position { get { return mIsMoving ? mCurrentTargetPosition : transform.position; } }
	
	// Protected members
	protected Vector3 mCurrentTargetPosition;
	protected bool mIsMoving = false;
	protected BaseUnit mUnit;
	protected GridManager mGridManager;
	protected Queue<Vector2> mMoveQueue;

	public void Start() {
		Floor floor = Helper.Find<Floor>("Floor");
		mGridManager = floor.GridManager;
		mUnit = GetComponent<BaseUnit>();
	}

	protected bool CanMove(BaseTile iTile, int xDir, int zDir) {
		bool canMove = true;

		foreach (BaseUnit unit in iTile.OccupyingUnits(mUnit)) {
			if (canMove) {
				canMove = unit == mUnit || unit.CanWalkOn(gameObject.tag); // You can walk here if it's to yourself or to a "walkable" unit.
				if (!canMove && IsPusher) {
					// If not a walkable, check if you are a 'Pusher' and it can be moved.
					BaseMover mover = unit.GetComponent<BaseMover>();
					if (mover != null) {
						canMove = mover.TryMove(xDir, zDir);
					}
				}
			}
			mUnit.OnCollided(unit);
			unit.OnCollided(mUnit);
		}
		return canMove && iTile.CanWalkOn(mUnit);
	}

	public abstract bool TryMove(int xDir, int zDir);
}