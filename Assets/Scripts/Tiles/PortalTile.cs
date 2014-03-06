using UnityEngine;
using System.Collections;

public class PortalTile : BaseTile 
{
	public BaseTile DestinationTile;

	protected override void OnLeaved(BaseUnit iUnit, BaseTile iNextTile) {}
	protected override void OnArrived(BaseUnit iUnit, BaseTile iPreviousTile) {
		if (iPreviousTile == DestinationTile || DestinationTile == null) // Came from the other portal
			return;

		if (!DestinationTile.CanWalkOn(iUnit))
			return;

		BaseTile.TeleportTo(iUnit, iPreviousTile, DestinationTile);
	}
}
