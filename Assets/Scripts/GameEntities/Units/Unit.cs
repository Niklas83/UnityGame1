using UnityEngine;
using System.Collections;

public sealed class Unit : BaseUnit
{
	public override int LayerMask { get { return (int)(Layer.Ground | Layer.Air); } }

    public override bool CanWalkOver { get { return false; } }
    public override bool CanWalkOn(string incomingUnitTag)
    {
        return CanWalkOver;
    }
}