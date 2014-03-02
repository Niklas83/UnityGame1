using UnityEngine;
using System.Collections;

public class TeleportTile : BaseTile 
{
	protected override void OnLeaved(BaseUnit iUnit, BaseTile iNextTile) {}
	protected override void OnArrived(BaseUnit iUnit, BaseTile iPreviousTile) {
		BaseTile tile = null;
		while(tile == null) {
			int x = Random.Range(0, GridManager.GetLength(0));
			int y = Random.Range(0, GridManager.GetLength(1));
			tile = GridManager.GetTile(x, y);
			if (tile != null && tile != this && tile.CanWalkOn(iUnit)) // This should be solved better... A "is tile valid" check..
				tile = null;
		}
		Vector3 position = tile.transform.position;
		position.y = 1; // This should be solved better... Like "place unit on tile" function.
		iUnit.transform.position = position;
		BaseTile.TeleportTo(iUnit, tile);
	}
}
