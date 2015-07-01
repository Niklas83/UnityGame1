using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using System.Collections;

public class AxisRotateMover : BaseMover
{
	public bool debugDraw = false;

	private BaseTile[] _sources;
	private BaseTile[] _destinations;
	private HashSet<BaseTile> _touchingTiles;
	private float _rotationDegrees;

	new void Start() {
		base.Start();
		_sources = new BaseTile[transform.childCount];
		_destinations = new BaseTile[transform.childCount];
		_touchingTiles = new HashSet<BaseTile>();
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
		if (isMoving)
			return false;

        /*if (soundEffectPLayer != null) // TODO: Should be triggered by animation.
			soundEffectPLayer.PlayWalkingSound();*/
     
		Vector3 directionToChild = Vector3.Normalize(pushedChild.position - transform.position);
		Vector3 directionTowardsDest = (pushedChild.position + new Vector3(xDir, 0, zDir)) - transform.position;
		_rotationDegrees = Mathf.Sign(GetSignedAngleBetween(directionToChild, directionTowardsDest))*90;
		_touchingTiles.Clear();

		Vector3 position = transform.position;
		for (int i = 0; i < transform.childCount; i++) {
			Transform child = transform.GetChild(i);
			_sources[i] = gridManager.GetTile(child.position);

			Vector3 toChild = (child.position - position);
			Quaternion q = Quaternion.LookRotation(Vector3.Normalize(toChild));
			Vector3 angles = q.eulerAngles;

			float radius = toChild.magnitude * 0.95f;
			float arcDistance = (Mathf.PI * 2*radius) / 4.0f;
			float maxStepLength = 0.5f;
			float nrSteps = Mathf.CeilToInt(arcDistance / maxStepLength);
			for (int j = 0; j < nrSteps; j++) {
				angles.y += _rotationDegrees / nrSteps;

				Quaternion current = Quaternion.Euler(angles);
				Vector3 forward = current * Vector3.forward;

				_touchingTiles.Add(gridManager.GetTile(position + forward * (radius + 0.5f)));
				_touchingTiles.Add(gridManager.GetTile(position + forward * (radius - 0.5f)));

				if (debugDraw) {
					Vector3 head = position + forward * (radius+0.5f);
					Vector3 foot = position + forward * (radius-0.5f);
					float t = ((float)j)/nrSteps;
					Debug.DrawRay(foot, head-foot, new Color(t, 1-t, 0), 5);
				}
			}

			Vector3 f = Quaternion.Euler(angles) * Vector3.forward;
			_destinations[i] = gridManager.GetTile(position + f * radius);
		}

		bool canRotate = true;
		foreach (BaseTile tile in _touchingTiles) {
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

	protected bool CanRotate(BaseTile tile, int xDir, int zDir) {
		bool canRotate = true;
		
		foreach (BaseUnit u in tile.OccupyingUnits(unit)) {
			if (canRotate) // A rotatable can't be pushed.
				canRotate = u == unit || u.transform.parent == transform || u.CanWalkOn(gameObject.tag);

			unit.OnCollided(u);
			u.OnCollided(unit);
		}
		return canRotate;
	}
    
    public IEnumerator Move(int xDir, int zDir, Transform pushedChild)
    {
        isMoving = true;

		for (int i = 0; i < transform.childCount; i++) {
			Transform child = transform.GetChild(i);
		    if (child.tag != "RotationAxis")
		    {
		        BaseUnit u = child.gameObject.GetComponent<BaseUnit>();
		        BaseTile.HandleOccupy(u, _sources[i], _destinations[i]);
		    }
		}

		int direction = (int) Mathf.Sign(_rotationDegrees);
		for (int i = 0; i < Mathf.Abs(_rotationDegrees); i += 6) {
	        transform.Rotate(0, 6 * direction, 0);
	        yield return new WaitForSeconds(0.0001f);
		}

		for (int i = 0; i < transform.childCount; i++) {
			Transform child = transform.GetChild(i);
		    if (child.tag != "RotationAxis")
		    {
		        BaseUnit u = child.gameObject.GetComponent<BaseUnit>();
		        BaseTile.HandleArrive(u, _sources[i], _destinations[i]);
		    }
		}

		isMoving = false;
        yield return 0;
    }
}
