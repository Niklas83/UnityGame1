using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class ProjectileUnit : BaseUnit 
{
	public bool ResetMapIfPlayerIsKilled;
	public bool CanPassThroughUnits;

    public override bool CanWalkOver { get { return true; } }
    public override bool CanWalkOn(string incomingUnitTag)
    {
        return CanWalkOver;
    }

	public override void OnCollided(BaseUnit iUnit) {
		if (iUnit is AvatarUnit) //checks if the unit you are walking upon is a player
		{
			Destroy(iUnit.gameObject);
			Destroy(this.gameObject);
			if (ResetMapIfPlayerIsKilled)
			{
				Application.LoadLevel(Application.loadedLevel);
			}
		} else if (iUnit is Unit && !CanPassThroughUnits) {
			Destroy(gameObject);
		}
	}
}