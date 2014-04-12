using UnityEngine;
using System.Collections;
using System;

public class ArrowTile : BaseTile  {
	protected override void OnLeaved(BaseUnit unit, BaseTile nextTile) {}
	protected override void OnArrived(BaseUnit unit, BaseTile previousTile) {}

	public override bool CanWalkOn(BaseUnit unit) {
		bool canWalkOn = base.CanWalkOn(unit);
		if (!canWalkOn)
			return false;

		Vector3 direction = Vector3.Normalize(unit.transform.position - transform.position);
		Vector3 forward = transform.rotation * Vector3.forward;
		return Vector3.Dot(direction, forward) <= 0;
	}
}
