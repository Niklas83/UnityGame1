using UnityEngine;
using System.Collections;

public class PortalTile : BaseTile 
{
    //This Durabiliy is compared to the units weight, if the weight > durability you fall through the floor
    public int CurrentDurability = 100;
    public override int Durability { get { return CurrentDurability; } }

    //Trainunits check if this is true, if so it may move on it
    public override bool TrainTile { get { return IsTrainTile; } }
    public bool IsTrainTile = false;

	public BaseTile DestinationTile;

	protected override void OnLeaved(BaseUnit unit, BaseTile nextTile) {}
	protected override void OnArrived(BaseUnit unit, BaseTile previousTile) 
	{
        base.OnArrived(unit, previousTile);
		if (previousTile == DestinationTile || DestinationTile == null) // Came from the other portal
			return;

		if (!DestinationTile.CanWalkOn(unit))
			return;

		BaseTile.TeleportTo(unit, this, DestinationTile);
	}
}
