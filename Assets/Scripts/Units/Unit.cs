using UnityEngine;
using System.Collections;

public sealed class Unit : BaseUnit
{

    public override bool CanWalkOver { get { return false; } }

    public override bool CanWalkOn(string incomingUnitTag)
    {
        return CanWalkOver;
    }
}