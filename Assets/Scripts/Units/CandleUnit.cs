using UnityEngine;
using System.Collections;

public sealed class CandleUnit : BaseUnit
{
    public EventListener[] ObjectsToNotify;
    public EventMessage Message;

    void Start()
    {
        ObjectsToNotify = new EventListener[1];

        EventListener portal = GameObject.Find("ThePortal").GetComponent<EventListener>();

        ObjectsToNotify[0] = portal;

    }


    // changed so u can have method handling the return value
    public override bool CanWalkOver { get { return true; } }

    public override bool CanWalkOn(string incomingUnitTag)
    {
        if (incomingUnitTag.Equals(UnitTypesEnum.Player.ToString()))
        {
            if (this.gameObject.transform.parent.gameObject.transform.childCount == 1)
            {
                foreach (EventListener el in ObjectsToNotify)
                {
                    el.ReceiveEvent(Message);
                }
            }

            Destroy(this.gameObject);

            return CanWalkOver;
        }

        if (incomingUnitTag.Equals(UnitTypesEnum.Box.ToString()))
        {
            return false;
        }

        return CanWalkOver;
    }


    //public override bool CanWalkOn
    //{

    //    get
    //    {
    //        if (this.gameObject.transform.parent.gameObject.transform.childCount == 1)
    //        {
    //            foreach (EventListener el in ObjectsToNotify)
    //            {
    //                el.ReceiveEvent(Message);
    //            }
    //        }


    //        Destroy(this.gameObject);

    //        return true;
    //    }
    //}

}