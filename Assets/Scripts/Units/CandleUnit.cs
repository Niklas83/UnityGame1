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

    public override bool CanWalkOn
    {

        get
        {
            if (this.gameObject.transform.parent.gameObject.transform.childCount == 1)
            {
                foreach (EventListener el in ObjectsToNotify)
                {
                    el.ReceiveEvent(Message);
                }
            }


            Destroy(this.gameObject);

            return true;
        }
    }

}