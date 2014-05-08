using UnityEngine;
using System.Collections;

public class ButtonTile : BaseTile 
{
    //This Durabiliy is compared to the units weight, if the weight > durability you fall through the floor
    public int CurrentDurability = 100;
    public override int Durability { get { return CurrentDurability; } }

	public EventListener[] objectsToNotify;
	public EventMessage message;
	
	protected override void OnLeaved(BaseUnit unit, BaseTile nextTile) {}
	protected override void OnArrived(BaseUnit unit, BaseTile previousTile) {
        base.OnArrived(unit, previousTile);
		foreach (EventListener el in objectsToNotify)
			el.ReceiveEvent(message);
	}
}
