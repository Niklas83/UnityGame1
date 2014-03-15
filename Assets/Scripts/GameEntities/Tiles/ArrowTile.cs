using UnityEngine;
using System.Collections;
using System;

public class ArrowTile : BaseTile  {
	protected override void OnLeaved(BaseUnit iUnit, BaseTile iNextTile) {}
	protected override void OnArrived(BaseUnit iUnit, BaseTile iPreviousTile) {}

	public override bool CanWalkOn(BaseUnit iUnit) {
		bool canWalkOn = base.CanWalkOn(iUnit);
		if (!canWalkOn)
			return false;

		Vector3 direction = Vector3.Normalize(iUnit.transform.position - transform.position);
		Vector3 forward = transform.rotation * Vector3.forward;
		return Vector3.Dot(direction, forward) <= 0;
	}
}
