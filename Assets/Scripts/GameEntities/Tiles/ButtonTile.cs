using System.Linq;
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

    //This tile is a portal if this is true
    public override bool TeleporterTile { get { return IsPortalTile; } }
    public bool IsPortalTile = false;
    public override BaseTile TeleportDestinationTile { get { return DestinationTeleportTile; }}
    public BaseTile DestinationTeleportTile;

    public EventMessage ArriveMessage;

    public EventMessage LeaveMessage;

    //sätt denna till true för att använda "statiska event metoder"
    public bool UseStaticEventMethods = true;

    //Object Setup 1
    public EventListener[] objectsToNotify_1;
    public StaticEventMethods StaticArriveMethod_1;
    public StaticEventMethods StaticLeaveMethod_1;

    //Object Setup 2
    public EventListener[] objectsToNotify_2;
    public StaticEventMethods StaticArriveMethod_2;
    public StaticEventMethods StaticLeaveMethod_2;



    protected override void OnArrived(BaseUnit unit, BaseTile previousTile)
    {
        base.OnArrived(unit, previousTile);

        if (UseStaticEventMethods && (objectsToNotify_1.Any() || objectsToNotify_2.Any()))
        {
            foreach (EventListener el in objectsToNotify_1)
            {
                el.ReceiveEventMethod(StaticArriveMethod_1, this.EventGetGridManager()); //Sends the gridmanager for objects who was not active at game start
            }
            foreach (EventListener el in objectsToNotify_2)
            {
                el.ReceiveEventMethod(StaticArriveMethod_2, this.EventGetGridManager()); //Sends the gridmanager for objects who was not active at game start
            }
        }
        else
        {
            foreach (EventListener el in objectsToNotify_1)
                el.ReceiveEvent(ArriveMessage);
        }
    }

    protected override void OnLeaved(BaseUnit unit, BaseTile nextTile)
    {
        if (UseStaticEventMethods && (objectsToNotify_1.Any() || objectsToNotify_2.Any()))
        {
            foreach (EventListener el in objectsToNotify_1)
            {
                el.ReceiveEventMethod(StaticLeaveMethod_1, this.EventGetGridManager()); //Sends the gridmanager for objects who was not active at game start
            }
            foreach (EventListener el in objectsToNotify_2)
            {
                el.ReceiveEventMethod(StaticLeaveMethod_2, this.EventGetGridManager()); //Sends the gridmanager for objects who was not active at game start          
            }
        }

        else
        {
            foreach (EventListener el in objectsToNotify_1)
                el.ReceiveEvent(LeaveMessage);
        }
    }



}
