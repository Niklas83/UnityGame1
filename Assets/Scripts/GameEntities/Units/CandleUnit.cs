using UnityEngine;
using System.Collections;

public sealed class CandleUnit : BaseUnit
{
	public override int LayerMask { get { return (int)Layer.Ground; } }
	
    private EventListener[] _objectsToNotify;

    void Start() {
		_objectsToNotify = new EventListener[1];
		_objectsToNotify[0] = GameObject.Find("Exit").GetComponent<EventListener>();
		foreach (EventListener el in _objectsToNotify)
			el.ReceiveEvent(EventMessage.Register);
    }

    // changed so u can have method handling the return value
    public override bool CanWalkOver { get { return true; } }
    public override bool CanWalkOn(string incomingUnitTag) {
		bool isBox = incomingUnitTag.Equals(UnitTypesEnum.Box.ToString());
		return isBox ? false : CanWalkOver;
    }

	public override void OnArrivedToMe(BaseUnit unit) {
		if (unit is AvatarUnit) {
			foreach (EventListener el in _objectsToNotify)
				el.ReceiveEvent(EventMessage.Unregister);

			DestroyUnit();
		}
	}
}