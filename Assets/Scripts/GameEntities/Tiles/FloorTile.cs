using UnityEngine;
using System.Collections;
using System;

public class FloorTile : BaseTile 
{
	protected override void OnLeaved(BaseUnit iUnit, BaseTile iNextTile) {}
	protected override void OnArrived(BaseUnit iUnit, BaseTile iPreviousTile) {}
}
