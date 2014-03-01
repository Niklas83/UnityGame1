using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class ProjectileUnit : BaseUnit 
{

    public override bool CanWalkOver { get { return true; } }

    public override bool CanWalkOn(string incomingUnitTag)
    {
        return CanWalkOver;
    }

	private ProjectileMover mMover;

}