using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using System.Collections;

public class RotateMover : BaseMover {

	public override bool TryMove(int xDir, int zDir)
	{
		if (mIsMoving)
			return false;

		Transform transform = gameObject.transform;
		Transform parent = transform.parent;
		
		BaseTile tilePassingBy = mGridManager.GetTile(transform.position + new Vector3(xDir, 0, zDir));
		BaseTile tileEndPosition = mGridManager.GetTile(parent.position + new Vector3(xDir, 0, zDir));
		
		if ((tilePassingBy == null || !tilePassingBy.IsActive()) ||
		    (tileEndPosition == null || !tileEndPosition.IsActive())) // Can't move to a non existing tile.
		{
			return false;
		}
		
		bool passingByCanMove = true;
		bool endPositionCanMove = true;
		
		//TODO: Fixa så de som blir pushade glider åt olika håll? Just nu åker båda eventualla unit som är i vägen åt samma håll (både passingby- och endPostition objekten)
		passingByCanMove = CanMove(tilePassingBy, xDir, zDir);
		if (passingByCanMove)
			endPositionCanMove = CanMove(tileEndPosition, xDir, zDir);
		
		if (passingByCanMove && endPositionCanMove)
			StartCoroutine(Move(xDir, zDir));
		
		return passingByCanMove && endPositionCanMove;
	}
	
	public static float GetSignedAngleBetween(Vector3 a, Vector3 b) {
		float angle = Mathf.Atan2(a.z, a.x) - Mathf.Atan2(b.z, b.x);
		float pi = Mathf.PI;
		angle = angle < -pi ? angle+pi*2 : angle;
		angle = angle > pi ? angle-pi*2 : angle;
		return angle;
	}
	
	public IEnumerator Move(int xDir, int zDir)
	{
		mIsMoving = true;
		
		Transform transform = gameObject.transform;
		Transform parent = transform.parent;
		
		Vector3 directionToMe = Vector3.Normalize(transform.position - parent.position);
		Vector3 wantedDirection = new Vector3(xDir, 0, zDir);
		
		int rotateDegrees = (int) (GetSignedAngleBetween(directionToMe, wantedDirection) * Mathf.Rad2Deg);

		Vector3 startPosition = transform.position;
		Vector3 endPosition = parent.position + new Vector3(xDir, 0, zDir);

		BaseTile sourceTile = mGridManager.GetTile(startPosition);
		BaseTile destinationTile = mGridManager.GetTile(endPosition);
		BaseTile.HandleOccupy(mUnit, sourceTile, destinationTile);
		
		//TODO: fixa så att det görs en kontroll om man kommer från sidan av bommen för kommande liknande objekt.
		int direction = (int) Mathf.Sign(rotateDegrees);
		if (rotateDegrees > 0 || rotateDegrees < 0) {
			for (int i = 0; i < Mathf.Abs(rotateDegrees); i += 6)
			{
				parent.Rotate(0, 6 * direction, 0);
				yield return new WaitForSeconds(0.0001f);
			}
		}

		BaseTile.HandleArrive(mUnit, sourceTile, destinationTile);
		mIsMoving = false;
		
		yield return 0;
	}
}
