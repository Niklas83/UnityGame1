using UnityEngine;
using System.Collections;
using System;

public class FallingTile : BaseTile 
{
	protected override void OnLeaved(BaseUnit unit, BaseTile nextTile) {
		if (unit is AvatarUnit) {
			GridManager.RemoveTile(this);
			Destroy(this.gameObject);
		}
	}
	protected override void OnArrived(BaseUnit unit, BaseTile previousTile) {}
}
