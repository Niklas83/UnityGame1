using UnityEngine;
using System.Collections;

public class RedAndBlueUnit : BaseUnit
{
    //This weight is compared to the floors durability, if the weight > durability you fall through the floor
    public int CurrentWeight = 50;
    public override int Weight { get { return CurrentWeight; } }

    public override int LayerMask { get { return (int)(Layer.Ground | Layer.Air); } }

    public bool lowered = false;

    public bool walkOver = false;

    public override bool CanWalkOver { get { return walkOver; } }
    public override bool CanWalkOn(string incomingUnitTag)
    {
        return CanWalkOver;
    }


    public void ToggleUpDown()
    {
        if (!lowered)
        {
            this.transform.position = new Vector3(this.transform.position.x, (this.transform.position.y - 0.99f),
                this.transform.position.z);
            walkOver = true;
            lowered = true;
        }
        else
        {
            this.transform.position = new Vector3(this.transform.position.x, 1f, this.transform.position.z);
            walkOver = false;
            lowered = false;
        }
    }
}
