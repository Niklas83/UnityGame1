using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Mover : MonoBehaviour
{
	// Settable in Editor
	public float MoveSpeed = 3f;

	// Private members
	private bool mIsMoving = false;
	private BaseUnit mUnit;
	private GridManager mGridManager;

	// Public properties
	public bool IsMoving { get { return mIsMoving; } }
	
	public void Start()
	{
		GridManagerExample gme = Helper.Find<GridManagerExample>("GridManagerExample");
		mGridManager = gme.GridManager;

		mUnit = GetComponent<BaseUnit>();
	}
	
	public IEnumerator move(int xDir, int yDir)
	{
		Vector3 startPosition = transform.position;
		// TODO: It crashes when trying to get "out of bounds". This should be fixed when adding walls.
		BaseTile tile = mGridManager.GetTile(Mathf.RoundToInt(startPosition.x) + xDir, Mathf.RoundToInt(startPosition.z) + yDir);

		if (!tile.Occupied) {
			mIsMoving = true;
			float t = 0;
			Vector3 endPosition = startPosition + new Vector3(xDir * Defines.TILE_SIZE, 0, yDir * Defines.TILE_SIZE);
			
			while (t < 1f)
			{
				t += Time.deltaTime*(MoveSpeed / Defines.TILE_SIZE);
				transform.position = Vector3.Slerp(startPosition, endPosition, t);
				yield return null;
			}

			tile.Occupy(mUnit);
			mIsMoving = false;
		}
		yield return 0;
	}
}