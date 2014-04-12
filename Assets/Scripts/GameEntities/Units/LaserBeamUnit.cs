using System;
using UnityEngine;
using System.Collections;

public class LaserBeamUnit : BaseUnit
{
	public override int LayerMask { get { return (int)(Layer.Air | Layer.Ground); } }
	public override bool CanWalkOver { get { return false; } }
	public override bool CanWalkOn(string incomingUnitTag) { return CanWalkOver; }

    public void Start() {
		// LaserBeam = GetComponentInChildren<LaserBeam>();
	}
}
