using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Mover : MonoBehaviour
{
	// Settable in Editor
	public float MoveSpeed = 3f;
	public bool IsPusher = false; // Can push stuff around

	// This position will always be aligned with the grid, either transform position or the target.
	public Vector3 Position { get { return mIsMoving ? mCurrentTargetPosition : transform.position; } }

	// Private members
	private bool mIsMoving = false;
	private Vector3 mCurrentTargetPosition;
	private BaseUnit mUnit;
	private GridManager mGridManager;
	private Queue<Vector2> mMoveQueue;

	// Public properties
	public bool IsMoving { get { return mIsMoving; } }
	
	public void Start()
	{
		Floor floor = Helper.Find<Floor>("Floor");
		mGridManager = floor.GridManager;

		mUnit = GetComponent<BaseUnit>();
	}

	// Checks if this mover can move in the given direction.
	public bool TryMove(int xDir, int yDir) {
		BaseTile tile = mGridManager.GetTile(transform.position + new Vector3(xDir, 0, yDir));

		if (tile == null || !tile.IsActive()) // Can't move to a non existing tile.
			return false;

		bool canMove = true;
		bool occupied = tile.Occupied;
		if (occupied) { // Something is in the place we want to move
			BaseUnit unit = tile.GetOccupyingUnit();
			canMove = unit == mUnit || unit.CanWalkOn(this.gameObject.tag); // You can walk here if it's to yourself or to a "walkable" unit.
			if (!canMove && IsPusher) {
				// If not a walkable, check if you are a 'Pusher' and it can be moved.
				Mover mover = unit.GetComponent<Mover>();
			    if (mover != null)
			    {
			        canMove = mover.TryMove(xDir, yDir);
			    }

                //TODO: Sl� ihop ovanliggande med denna
			    RotateMover rotateMover = unit.GetComponent<RotateMover>();
			    if (rotateMover != null)
			    {
                    canMove = rotateMover.TryMove(xDir, yDir, this.gameObject.transform.position);
			    }
			}
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

        //Moved the transform of the acctual movement to before the animation so that the projectiles wont hit when u have started to move (these 3 lines used to be after the animation)
        transform.position = endPosition; // Avoid rounding errors with slerp when t == 1
        BaseTile tile = mGridManager.GetTile(Mathf.RoundToInt(startPosition.x) + xDir, Mathf.RoundToInt(startPosition.z) + yDir);
        tile.Occupy(mUnit);

		while (t < 1f)
		{
			t += Time.deltaTime*(MoveSpeed / Defines.TILE_SIZE);
			transform.position = Vector3.Lerp(startPosition, endPosition, Mathf.Clamp01(t));
			yield return null;
		}


		mIsMoving = false;

		yield return 0;
	}
}