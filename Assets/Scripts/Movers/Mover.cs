using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Mover : BaseMover {

	public float moveSpeed = 3f;
	
	public bool PROTOTYPE_IceFriction = false;

	// Checks if this mover can move in the given direction.
	public override bool TryMove(int xDir, int zDir) {
		if (isMoving)
			return false;

		BaseTile tile = gridManager.GetTile(transform.position + new Vector3(xDir, 0, zDir));
		
		if (tile == null || !tile.IsActive()) // Can't move to a non existing tile.
			return false;
		
		bool canMove = CanMove(tile, xDir, zDir);
		if (canMove)
			StartCoroutine(Move(xDir, zDir));
		
		return canMove;
	}
	
	public IEnumerator Move(int xDir, int zDir)
	{
	    DebugAux.Assert(!isMoving, "Can't move a unit while it is moving!");
		
		isMoving = true;
		float t = 0;
		Vector3 startPosition = transform.position;
		Vector3 endPosition = startPosition + new Vector3(xDir * Defines.TILE_SIZE, 0, zDir * Defines.TILE_SIZE);
		currentTargetPosition = endPosition;

		BaseTile sourceTile = gridManager.GetTile(startPosition);
		BaseTile destinationTile = gridManager.GetTile(endPosition);

        if (soundEffectPLayer != null) // Reactivated this code as it was not working properly in the AvatarStates script
            soundEffectPLayer.PlayWalkingSound(destinationTile.tag);

		BaseTile.HandleOccupy(unit, sourceTile, destinationTile);
		
		while (t < 1f)
		{
			t += Time.deltaTime*(moveSpeed / Defines.TILE_SIZE);
			transform.position = Vector3.Lerp(startPosition, endPosition, Mathf.Clamp01(t));
			yield return null;
		}
		
		transform.position = endPosition;
		BaseTile.HandleArrive(unit, sourceTile, destinationTile);

		isMoving = false;
		
		//Sätt in ice logic here!!
	    if (SlidesOnIce && destinationTile.IceTile)
	    {
            Vector3 nextPosition = new Vector3(xDir,0,zDir);


	        if (gridManager.GetTile(destinationTile.transform.position + nextPosition).IceTile)
	        {
	            TryMove(xDir, zDir);
	        }
	    }
        
        
        if (PROTOTYPE_IceFriction && destinationTile.PROTOTYPE_UseIceFriction) {
			TryMove(xDir, zDir);
		}
		
		yield return 0;
	}
}