using UnityEngine;
using System.Collections;

public class ButtonTile : BaseTile 
{
	public EventListener[] ObjectsToNotify;
	public EventMessage Message;
	
	protected override void OnLeaved(BaseUnit iUnit, BaseTile iNextTile) {}
	protected override void OnArrived(BaseUnit iUnit, BaseTile iPreviousTile) {
		foreach (EventListener el in ObjectsToNotify)
			el.ReceiveEvent(Message);
	}
}
