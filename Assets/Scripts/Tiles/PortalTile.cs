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

		Vector3 position = DestinationTile.transform.position;
		position.y = 1; // This should be solved better... Like "place unit on tile" function.
		iUnit.transform.position = position;
		BaseTile.TeleportTo(iUnit, DestinationTile);
	}
}
