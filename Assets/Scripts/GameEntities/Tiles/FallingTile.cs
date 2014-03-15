using UnityEngine;
using System.Collections;
using System;

public class FallingTile : BaseTile 
{
	protected override void OnLeaved(BaseUnit iUnit, BaseTile iNextTile) {
		GridManager.RemoveTile(this);
		Destroy(this.gameObject);
	}
	protected override void OnArrived(BaseUnit iUnit, BaseTile iPreviousTile) {}
}
