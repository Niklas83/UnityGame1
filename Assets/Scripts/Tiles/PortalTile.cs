using UnityEngine;
using System.Collections;

public class PortalTile : BaseTile 
{
	public PortalTile OtherPortal;

	protected override void OnLeaved(BaseUnit iUnit, BaseTile iNextTile) {}
	protected override void OnArrived(BaseUnit iUnit, BaseTile iPreviousTile) {
		if (iPreviousTile == OtherPortal) // Came from the other portal
			return;

		Vector3 position = OtherPortal.transform.position;
		position.y = 1; // This should be solved better... Like "place unit on tile" function.
		iUnit.transform.position = position;
		OtherPortal.Occupy(iUnit);
	}
}
