using System;
using UnityEngine;
using System.Collections;

public class LaserBeamUnit : BaseUnit
{
    //This weight is compared to the floors durability, if the weight > durability you fall through the floor
    public int CurrentWeight = 50;
    public override int Weight { get { return CurrentWeight; } }

    //Check this TRUE if you want the unit to be breakable by medusarays and other projectiles
    public bool BreaksByProjectile = false;
    public override bool BreaksByProjectileAndMedusa { get { return BreaksByProjectile; } }

	public override int LayerMask { get { return (int)(Layer.Air | Layer.Ground); } }
	public override bool CanWalkOver { get { return false; } }
	public override bool CanWalkOn(string incomingUnitTag) { return CanWalkOver; }

    public void Start() {
		// LaserBeam = GetComponentInChildren<LaserBeam>();
	}
}
