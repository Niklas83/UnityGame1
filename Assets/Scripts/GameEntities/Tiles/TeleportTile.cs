using UnityEngine;
using System.Collections;

public class TeleportTile : BaseTile 
{
	protected override void OnLeaved(BaseUnit unit, BaseTile nextTile) {}
	protected override void OnArrived(BaseUnit unit, BaseTile previousTile) 
	{
		BaseTile tile = null;
		while(tile == null) 
		{
			int x = Random.Range(0, GridManager.GetLength(0));
			int y = Random.Range(0, GridManager.GetLength(1));
			tile = GridManager.GetTile(x, y);
			if (tile != null && tile != this && tile.CanWalkOn(unit)) // This should be solved better... A "is tile valid" check..
				tile = null;
		}

		BaseTile.TeleportTo(unit, previousTile, tile);
	}
}
