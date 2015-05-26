using UnityEngine;
using System.Collections;

//No longer in use as all tiles has a checkbox to make them a portal
public class TeleportTile : BaseTile 
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

    //Not in use
    //This tile is a portal if this is true
    public override bool TeleporterTile { get { return false; } }
    //public bool IsPortalTile = false;
    public override BaseTile TeleportDestinationTile { get { return null; }}
    //public BaseTile DestinationTeleportTile;

	protected override void OnLeaved(BaseUnit unit, BaseTile nextTile) {}
	protected override void OnArrived(BaseUnit unit, BaseTile previousTile) 
	{
        base.OnArrived(unit, previousTile);

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
