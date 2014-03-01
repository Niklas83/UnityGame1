using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Mover : BaseMover {

	public float MoveSpeed = 3f;
	
	// Checks if this mover can move in the given direction.
	public override bool TryMove(int xDir, int yDir) {
		BaseTile tile = mGridManager.GetTile(transform.position + new Vector3(xDir, 0, yDir));
		
		if (tile == null || !tile.IsActive()) // Can't move to a non existing tile.
			return false;
		
		bool canMove = true;
		bool occupied = tile.Occupied;
		if (occupied) { // Something is in the place we want to move
			BaseUnit unit = tile.GetOccupyingUnit();
			canMove = unit == mUnit || unit.CanWalkOn(gameObject.tag); // You can walk here if it's to yourself or to a "walkable" unit.
			if (!canMove && IsPusher) {
				// If not a walkable, check if you are a 'Pusher' and it can be moved.
				BaseMover mover = unit.GetComponent<BaseMover>();
				if (mover != null)
					canMove = mover.TryMove(xDir, yDir);
			}
			mUnit.OnCollided(unit);
			unit.OnCollided(mUnit);
		}
		
		if (canMove)
			StartCoroutine(Move(xDir, yDir));
		
		return canMove;
	}
	
	public IEnumerator Move(int xDir, int yDir)
	{
		DebugAux.Assert(!mIsMoving, "Can't move a unit while it is moving!");
		
		mIsMoving = true;
		float t = 0;
		Vector3 startPosition = transform.position;
		Vector3 endPosition = startPosition + new Vector3(xDir * Defines.TILE_SIZE, 0, yDir * Defines.TILE_SIZE);
		mCurrentTargetPosition = endPosition;

		BaseTile sourceTile = mGridManager.GetTile(startPosition);
		BaseTile destinationTile = mGridManager.GetTile(endPosition);
		if (destinationTile != null)
			destinationTile.Occupy(mUnit, sourceTile);
		
		while (t < 1f)
		{
			t += Time.deltaTime*(MoveSpeed / Defines.TILE_SIZE);
			transform.position = Vector3.Lerp(startPosition, endPosition, Mathf.Clamp01(t));
			yield return null;
		}
		
		transform.position = endPosition;
		if (destinationTile != null)
			destinationTile.Arrive(mUnit, sourceTile);

		mIsMoving = false;
		
		yield return 0;
	}
}