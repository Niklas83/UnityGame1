﻿using UnityEngine;
using System.Collections;

public class RedAndBlueUnit : BaseUnit
{
    //This weight is compared to the floors durability, if the weight > durability you fall through the floor
    public int CurrentWeight = 50;
    public override int Weight { get { return CurrentWeight; } }

    public override int LayerMask { get { return (int)(Layer.Ground | Layer.Air); } }

    private bool lowered = false;

    private bool walkOver = false;

    public bool hasBeenActivated = false;

    public override bool CanWalkOver { get { return walkOver; } }
    public override bool CanWalkOn(string incomingUnitTag)
    {
        return CanWalkOver;
    }


    public void ToggleUpDown()
    {
        if (!lowered)
        {
            this.transform.position = new Vector3(this.transform.position.x, 0.05f,
                this.transform.position.z);
            walkOver = true;
            lowered = true;

            this.OnDeactivated();
        }
        else
        {
            this.transform.position = new Vector3(this.transform.position.x, 1f, this.transform.position.z);
            walkOver = false;
            lowered = false;

            this.OnActivated();
        }
    }
}
