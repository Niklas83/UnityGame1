using UnityEngine;
using System.Collections;
using System;

public class FloorTile : BaseTile
{
    //This Durabiliy is compared to the units weight, if the weight > durability you fall through the floor
    public int CurrentDurability = 100;
    public override int Durability { get { return CurrentDurability; } }

	protected override void OnLeaved(BaseUnit unit, BaseTile nextTile) {}

    protected override void OnArrived(BaseUnit unit, BaseTile previousTile)
    {   
        base.OnArrived(unit,  previousTile);
    }
}
