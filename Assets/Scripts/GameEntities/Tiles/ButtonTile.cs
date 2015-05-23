using UnityEngine;
using System.Collections;

public class ButtonTile : BaseTile 
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

	public EventListener[] objectsToNotify;
	
    public EventMessage ArriveMessage;

    public EventMessage LeaveMessage;

    //sätt denna till true
    public bool UseStaticEventMethods = true;

    public StaticEventMethods StaticArriveMethod;
    public StaticEventMethods StaticLeaveMethod;

    protected override void OnLeaved(BaseUnit unit, BaseTile nextTile)
    {
        if (UseStaticEventMethods)
        {
            foreach (EventListener el in objectsToNotify)
                el.ReceiveEventMethod(StaticLeaveMethod, this.EventGetGridManager());   //Sends the gridmanager for objects who was not active at game start
        }

        else
        {
            foreach (EventListener el in objectsToNotify)
                el.ReceiveEvent(LeaveMessage);
        }
    }


	protected override void OnArrived(BaseUnit unit, BaseTile previousTile) {
        base.OnArrived(unit, previousTile);

	    if (UseStaticEventMethods)
	    {
            foreach (EventListener el in objectsToNotify)
                el.ReceiveEventMethod(StaticArriveMethod, this.EventGetGridManager());   //Sends the gridmanager for objects who was not active at game start
	    }
	    else
	    {
            foreach (EventListener el in objectsToNotify)
                el.ReceiveEvent(ArriveMessage);
	    }
	}
}
