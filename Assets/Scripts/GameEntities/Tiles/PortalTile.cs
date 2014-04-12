using UnityEngine;
using System.Collections;

public class PortalTile : BaseTile 
{
	public BaseTile DestinationTile;

	protected override void OnLeaved(BaseUnit unit, BaseTile nextTile) {}
	protected override void OnArrived(BaseUnit unit, BaseTile previousTile) 
	{
		if (previousTile == DestinationTile || DestinationTile == null) // Came from the other portal
			return;

		if (!DestinationTile.CanWalkOn(unit))
			return;

		BaseTile.TeleportTo(unit, this, DestinationTile);
	}
}
