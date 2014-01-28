using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public sealed class AvatarUnit : BaseUnit
{
	public override bool CanWalkOn { get { return false; } }

	private Mover mMover;
	private PathFinder mPathFinder;
	private GridManager mGridManager;
	private Queue<Vector2> mMoveQueue;
	
	public void Start()
	{ 
		mMover = GetComponent<Mover>();

		Floor floor = Helper.Find<Floor>("Floor");
		mGridManager = floor.GridManager;
		mPathFinder = new PathFinder(mGridManager);
	}
	
	public void Update()
	{
		if (Input.GetMouseButtonUp(0)) {
			Vector3 mp = Input.mousePosition;
			Ray r = Camera.main.ScreenPointToRay(new Vector3(mp.x, mp.y, Camera.main.transform.position.y)); // Create a ray that starts at the camera and goes through the mouse position in world space.

			float d = Vector3.Dot(new Vector3(0, 1, 0) - r.origin, Vector3.up) / Vector3.Dot(r.direction, Vector3.up);
			Vector3 wp = r.origin + r.direction * d;
			wp.y = 1; // Avoid rounding errors

			int cost;
			Vector3 startPosition = mMover.Position; // We use the mover position as start, since it can be moving (causing transform to be unreliable).
			Vector2[] path = mPathFinder.GetPathTo(startPosition, wp, this, out cost);

			if (path != null)
				mMoveQueue = new Queue<Vector2>(path);
			else {
				// Couldn't get there, try pushing instead..
				Vector3 dir = wp - startPosition;
				int x = Math.Sign(dir.x);
				int z = Math.Sign(dir.z);
				if (Math.Abs(dir.x) > Math.Abs(dir.z))
					mMover.TryMove(x, 0);
				else if (Math.Abs(dir.x) < Math.Abs(dir.z)) 
					mMover.TryMove(0, z);
			}
		}

		if (!mMover.IsMoving && mMoveQueue != null) {
			Vector2 dir = mMoveQueue.Dequeue();
			mMover.TryMove((int)dir.x, (int)dir.y);
			if (mMoveQueue.Count == 0)
				mMoveQueue = null;
		}
	}
}