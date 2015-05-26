using UnityEngine;
using System.Collections;

//No longer in use as all tiles has a checkbox to make them a portal
public class PortalTile : BaseTile 
{
    //This Durabiliy is compared to the units weight, if the weight > durability you fall through the floor
    public int CurrentDurability = 100;
    public override int Durability { get { return CurrentDurability; } }

    //Trainunits check if this is true, if so it may move on it
    public override bool TrainTile { get { return IsTrainTile; } }
    public bool IsTrainTile = false;

    //If this is true objects being pushed on this tile from another tile will keep sliding towards the direction it was pushed
    public override bool IceTile { get { return IsIceTile; } }
    public bool IsIceTile = false;

    //This tile is a portal if this is true
    public override bool TeleporterTile { get { return false; } }
    //public bool IsPortalTile = false;
    public override BaseTile TeleportDestinationTile { get { return null; }}
    //public BaseTile DestinationTeleportTile;

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
	    if (unit is AvatarUnit)
	    {
	        AvatarUnit avatar = (AvatarUnit) unit;
            avatar.EmptyMoveQueue();
	    }
	}
}
