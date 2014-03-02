using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Mover : BaseMover {

	public float MoveSpeed = 3f;

	// Checks if this mover can move in the given direction.
	public override bool TryMove(int xDir, int zDir) {
		if (mIsMoving)
			return false;

		BaseTile tile = mGridManager.GetTile(transform.position + new Vector3(xDir, 0, zDir));
		
		if (tile == null || !tile.IsActive()) // Can't move to a non existing tile.
			return false;
		
		bool canMove = CanMove(tile, xDir, zDir);
		if (canMove)
			StartCoroutine(Move(xDir, zDir));
		
		return canMove;
	}
	
	public IEnumerator Move(int xDir, int zDir)
	{
		DebugAux.Assert(!mIsMoving, "Can't move a unit while it is moving!");
		
		mIsMoving = true;
		float t = 0;
		Vector3 startPosition = transform.position;
		Vector3 endPosition = startPosition + new Vector3(xDir * Defines.TILE_SIZE, 0, zDir * Defines.TILE_SIZE);
		mCurrentTargetPosition = endPosition;

		BaseTile sourceTile = mGridManager.GetTile(startPosition);
		BaseTile destinationTile = mGridManager.GetTile(endPosition);
		BaseTile.HandleOccupy(mUnit, sourceTile, destinationTile);
		
		while (t < 1f)
		{
			t += Time.deltaTime*(MoveSpeed / Defines.TILE_SIZE);
			transform.position = Vector3.Lerp(startPosition, endPosition, Mathf.Clamp01(t));
			yield return null;
		}
		
		transform.position = endPosition;
		BaseTile.HandleArrive(mUnit, sourceTile, destinationTile);

		mIsMoving = false;
		
		yield return 0;
	}
}