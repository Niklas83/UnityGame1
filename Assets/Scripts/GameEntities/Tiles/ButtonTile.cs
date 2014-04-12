using UnityEngine;
using System.Collections;

public class ButtonTile : BaseTile 
{
	public EventListener[] objectsToNotify;
	public EventMessage message;
	
	protected override void OnLeaved(BaseUnit unit, BaseTile nextTile) {}
	protected override void OnArrived(BaseUnit unit, BaseTile previousTile) {
		foreach (EventListener el in objectsToNotify)
			el.ReceiveEvent(message);
	}
}
