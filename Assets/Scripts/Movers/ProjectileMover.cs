using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ProjectileMover : Mover {

    private int NumberOfMovementOverNonExistingTiles = 0;

    void Update()
    {
		Vector3 forward = transform.forward;
		TryMove((int)forward.x, (int)forward.z);
    }

	// Checks if this mover can move in the given direction.
	public override bool TryMove(int xDir, int yDir)
    {
        if (mIsMoving)
			return false;

        BaseTile tile = mGridManager.GetTile(transform.position + new Vector3(xDir, 0, yDir));
		if (tile == null || !tile.IsActive()) // Projectiles can move over null tiles
        {
            NumberOfMovementOverNonExistingTiles++;
			bool canMove = NumberOfMovementOverNonExistingTiles <= 10;
			if (!canMove)
                Destroy(this.gameObject);
            else
                StartCoroutine(Move(xDir, yDir));
			return canMove;
        }
		NumberOfMovementOverNonExistingTiles = 0;
		return base.TryMove(xDir, yDir);
	}
}