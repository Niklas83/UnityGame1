using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ProjectileMover : Mover {

    private int _numberOfMovementOverNonExistingTiles = 0;

    void Update()
    {
		Vector3 forward = transform.forward;
		TryMove(Mathf.RoundToInt(forward.x), Mathf.RoundToInt(forward.z));
    }

	// Checks if this mover can move in the given direction.
	public override bool TryMove(int xDir, int zDir)
    {
        if (isMoving)
			return false;

        BaseTile tile = gridManager.GetTile(transform.position + new Vector3(xDir, 0, zDir));
		if (tile == null || !tile.IsActive()) // Projectiles can move over null tiles
        {
            _numberOfMovementOverNonExistingTiles++;
			bool canMove = _numberOfMovementOverNonExistingTiles <= 10;
			if (!canMove)
                Destroy(this.gameObject);
            else
                StartCoroutine(Move(xDir, zDir));
			return canMove;
        }
		_numberOfMovementOverNonExistingTiles = 0;
		return base.TryMove(xDir, zDir);
	}
}