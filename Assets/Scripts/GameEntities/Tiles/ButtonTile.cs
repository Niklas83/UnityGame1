﻿using UnityEngine;
using System.Collections;

public class ButtonTile : BaseTile 
{
    //This Durabiliy is compared to the units weight, if the weight > durability you fall through the floor
    public int CurrentDurability = 100;
    public override int Durability { get { return CurrentDurability; } }

    //Trainunits check if this is true, if so it may move on it
    public override bool TrainTile { get { return IsTrainTile; } }
    public bool IsTrainTile = false;

	public EventListener[] objectsToNotify;
	public EventMessage message;
	
	protected override void OnLeaved(BaseUnit unit, BaseTile nextTile) {}
	protected override void OnArrived(BaseUnit unit, BaseTile previousTile) {
        base.OnArrived(unit, previousTile);
		foreach (EventListener el in objectsToNotify)
			el.ReceiveEvent(message);
	}
}
