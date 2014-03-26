﻿using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using System.Collections;

public class AxisRotateMover : BaseMover
{
	public bool DebugDraw = false;

	private BaseTile[] sources;
	private BaseTile[] destinations;
	private HashSet<BaseTile> touchingTiles;
	private float rotationDegrees;

	void Start() {
		base.Start();
		sources = new BaseTile[transform.childCount];
		destinations = new BaseTile[transform.childCount];
		touchingTiles = new HashSet<BaseTile>();
	}

	public override bool TryMove(int xDir, int yDir) {
		return false;
	}

	public static float GetSignedAngleBetween(Vector3 a, Vector3 b) {
		float angle = Mathf.Atan2(a.z, a.x) - Mathf.Atan2(b.z, b.x);
		float pi = Mathf.PI;
		angle = angle < -pi ? angle+pi*2 : angle;
		angle = angle > pi ? angle-pi*2 : angle;
		return angle;
	}

	public bool RequestMove(Transform pushedChild, int xDir, int zDir) {
		if (mIsMoving)
			return false;

        if (MoveSoundEffects != null)
        {
            MoveSoundEffects.PlayWalkingSound();   // Plays move sound
        }
		Vector3 directionToChild = Vector3.Normalize(pushedChild.position - transform.position);
		Vector3 directionTowardsDest = (pushedChild.position + new Vector3(xDir, 0, zDir)) - transform.position;
		rotationDegrees = Mathf.Sign(GetSignedAngleBetween(directionToChild, directionTowardsDest))*90;
		touchingTiles.Clear();

		Vector3 position = transform.position;
		for (int i = 0; i < transform.childCount; i++) {
			Transform child = transform.GetChild(i);
			sources[i] = mGridManager.GetTile(child.position);

			Vector3 toChild = (child.position - position);
			Quaternion q = Quaternion.LookRotation(Vector3.Normalize(toChild));
			Vector3 angles = q.eulerAngles;

			float radius = toChild.magnitude;
			float arcDistance = (Mathf.PI * 2*radius) / 4.0f;
			float maxStepLength = 0.5f;
			float nrSteps = Mathf.CeilToInt(arcDistance / maxStepLength);
			for (int j = 0; j < nrSteps; j++) {
				angles.y += rotationDegrees / nrSteps;

				Quaternion current = Quaternion.Euler(angles);
				Vector3 forward = current * Vector3.forward;

				touchingTiles.Add(mGridManager.GetTile(position + forward * (radius + 0.5f)));
				touchingTiles.Add(mGridManager.GetTile(position + forward * (radius - 0.5f)));

				if (DebugDraw) {
					Vector3 head = position + forward * (radius+0.5f);
					Vector3 foot = position + forward * (radius-0.5f);
					float t = ((float)j)/nrSteps;
					Debug.DrawRay(foot, head-foot, new Color(t, 1-t, 0), 5);
				}
			}

			Vector3 f = Quaternion.Euler(angles) * Vector3.forward;
			destinations[i] = mGridManager.GetTile(position + f * radius);
		}

		bool canRotate = true;
		foreach (BaseTile tile in touchingTiles) {
			if (tile != null) {
				canRotate = CanRotate(tile, xDir, zDir);
				if (!canRotate)
					return false;
			}
		}
		
		if (canRotate)
			StartCoroutine(Move(xDir, zDir, pushedChild));
	
		return canRotate;
    }

	protected bool CanRotate(BaseTile iTile, int xDir, int zDir) {
		bool canRotate = true;
		
		foreach (BaseUnit unit in iTile.OccupyingUnits(mUnit)) {
			if (canRotate) // A rotatable can't be pushed.
				canRotate = unit == mUnit || unit.transform.parent == transform || unit.CanWalkOn(gameObject.tag);

			mUnit.OnCollided(unit);
			unit.OnCollided(mUnit);
		}
		return canRotate;
	}
    
    public IEnumerator Move(int xDir, int zDir, Transform pushedChild)
    {
        mIsMoving = true;

		for (int i = 0; i < transform.childCount; i++) {
			Transform child = transform.GetChild(i);
			BaseUnit u = child.gameObject.GetComponent<BaseUnit>();
			BaseTile.HandleOccupy(u, sources[i], destinations[i]);
		}

		int direction = (int) Mathf.Sign(rotationDegrees);
		for (int i = 0; i < Mathf.Abs(rotationDegrees); i += 6) {
	        transform.Rotate(0, 6 * direction, 0);
	        yield return new WaitForSeconds(0.0001f);
		}

		for (int i = 0; i < transform.childCount; i++) {
			Transform child = transform.GetChild(i);
			BaseUnit u = child.gameObject.GetComponent<BaseUnit>();
			BaseTile.HandleArrive(u, sources[i], destinations[i]);
		}

		mIsMoving = false;
        yield return 0;
    }
}
