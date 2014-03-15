using UnityEngine;
using System.Collections;

public sealed class CandleUnit : BaseUnit
{
	public override int LayerMask { get { return (int)Layer.Ground; } }
    private EventListener[] ObjectsToNotify;

    void Start() {
		ObjectsToNotify = new EventListener[1];
		ObjectsToNotify[0] = GameObject.Find("Exit").GetComponent<EventListener>();
		foreach (EventListener el in ObjectsToNotify)
			el.ReceiveEvent(EventMessage.Register);
    }

    // changed so u can have method handling the return value
    public override bool CanWalkOver { get { return true; } }
    public override bool CanWalkOn(string incomingUnitTag) {
		bool isBox = incomingUnitTag.Equals(UnitTypesEnum.Box.ToString());
		return isBox ? false : CanWalkOver;
    }

	public override void OnArrivedToMe(BaseUnit iUnit) {
		if (iUnit is AvatarUnit) {
			foreach (EventListener el in ObjectsToNotify)
				el.ReceiveEvent(EventMessage.Unregister);

			DestroyUnit();
		}
	}
}