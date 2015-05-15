using UnityEngine;
using System.Collections;
using System;

public class FallingTile : BaseTile 
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

	protected override void OnLeaved(BaseUnit unit, BaseTile nextTile) {
		if (unit is AvatarUnit) {
			GridManager.RemoveTile(this);
			Destroy(this.gameObject);
		}
	}

    protected override void OnArrived(BaseUnit unit, BaseTile previousTile)
    {
        base.OnArrived(unit, previousTile);
    }
}
