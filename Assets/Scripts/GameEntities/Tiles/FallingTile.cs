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
