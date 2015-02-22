using UnityEngine;
using System.Collections;

public sealed class CandleUnit : BaseUnit
{
    //This weight is compared to the floors durability, if the weight > durability you fall through the floor
    public int CurrentWeight = 10;
    public override int Weight { get { return CurrentWeight; } }

    //Check this TRUE if you want the unit to be breakable by medusarays and other projectiles
    public bool BreaksByProjectile = false;
    public override bool BreaksByProjectileAndMedusa { get { return BreaksByProjectile; } }

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