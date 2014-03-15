using UnityEngine;
using System.Collections;
using System;

public class FallingTile : BaseTile 
{
	protected override void OnLeaved(BaseUnit iUnit, BaseTile iNextTile) {
		if (iUnit is AvatarUnit) {
			GridManager.RemoveTile(this);
			Destroy(this.gameObject);
		}
	}
	protected override void OnArrived(BaseUnit iUnit, BaseTile iPreviousTile) {}
}
