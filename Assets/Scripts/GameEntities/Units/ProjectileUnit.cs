using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class ProjectileUnit : BaseUnit 
{
	public bool ResetMapIfPlayerIsKilled;
	public bool CanPassThroughUnits;

	public override int LayerMask { get { return (int)Layer.Air; } }
    public override bool CanWalkOver { get { return true; } }
    public override bool CanWalkOn(string incomingUnitTag)
    {
        return CanWalkOver;
    }

	public override void OnCollided(BaseUnit iUnit) {
		if (iUnit is AvatarUnit) //checks if the unit you are walking upon is a player
		{
			iUnit.DestroyUnit();
			this.DestroyUnit();
			if (ResetMapIfPlayerIsKilled)
			{
				Application.LoadLevel(Application.loadedLevel);
			}
		} else if (iUnit is ProjectileUnit) {
		} else if (iUnit is BaseUnit && !CanPassThroughUnits) {
			this.DestroyUnit();
		}
	}
}