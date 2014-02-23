using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class ProjectileMover : MonoBehaviour
{
    
	// Settable in Editor
	public float MoveSpeed = 3f;
	public bool CanPassThroughUnits = false; // Can pass through units when true, else it will stop
    public bool ResetMapIfPlayerIsKilled = false;


	// This position will always be aligned with the grid, either transform position or the target.
	public Vector3 Position { get { return mIsMoving ? mCurrentTargetPosition : transform.position; } }

	// Private members
	private bool mIsMoving = false;
	private Vector3 mCurrentTargetPosition;
	private BaseUnit mUnit;
	private GridManager mGridManager;

    //Moving path
    public int xDir = 0;
    public int yDir = 0;

	// Public properties
	public bool IsMoving { get { return mIsMoving; } }
	
	public void init()
	{
		Floor floor = Helper.Find<Floor>("Floor");
		mGridManager = floor.GridManager;

		mUnit = GetComponent<BaseUnit>();
	}

    void Update()
    {
        TryMove();
    }

	// Checks if this mover can move in the given direction.
	public void TryMove() {

        //Controlls that the tile the projectile is currently on has a unity it will be destroyed
        BaseTile currenTile = mGridManager.GetTile(transform.position);
        if (currenTile == null || !currenTile.IsActive()) // Can't move to a non existing tile.
        {
            Destroy(this.gameObject);
            return;
        }
        
		BaseTile tile = mGridManager.GetTile(transform.position + new Vector3(xDir, 0, yDir));

	    if (tile == null || !tile.IsActive()) // Can't move to a non existing tile.
	    {
	        Destroy(this.gameObject);
	        return;
	    }
            
		bool canMove = true;
		bool occupied = tile.Occupied;
		if (occupied) { // Something is in the place we want to move
			BaseUnit unit = tile.GetOccupyingUnit();
			canMove = unit == mUnit || unit.CanWalkOn; // You can walk here if it's to yourself or to a "walkable" unit.

		    if (unit is AvatarUnit)         //checks if the unit you are walking upon is a player
		    {
		        Destroy(unit.gameObject);
                Destroy(this.gameObject);
		        if (ResetMapIfPlayerIsKilled)
		        {
		            Application.LoadLevel(Application.loadedLevel);
		        }
		    }


            if (!canMove && CanPassThroughUnits)        
            {
                canMove = true;
            }
            else
            {
                Destroy(this.gameObject);
            }
		}

        if (canMove && !mIsMoving)
	    {
	        StartCoroutine(Move(xDir, yDir));
	    }
        
	}

	public IEnumerator Move(int xDir, int yDir)
	{
        mIsMoving = true;

		float t = 0;
		Vector3 startPosition = transform.position;
		Vector3 endPosition = startPosition + new Vector3(xDir * Defines.TILE_SIZE, 0, yDir * Defines.TILE_SIZE);
		mCurrentTargetPosition = endPosition;

		while (t < 1f)
		{
			t += Time.deltaTime*(MoveSpeed / Defines.TILE_SIZE);
			transform.position = Vector3.Lerp(startPosition, endPosition, Mathf.Clamp01(t));
			yield return null;
		}

		transform.position = endPosition; // Avoid rounding errors with slerp when t == 1
		mIsMoving = false;

		yield return 0;
	}
     
}