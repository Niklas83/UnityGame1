using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public abstract class BaseUnit : BaseEntity
{
    public abstract bool CanWalkOver { get; }
	public abstract int LayerMask { get; }
	public BaseTile OccupiedTile { get; set; }

    public abstract bool CanWalkOn(string incomingUnitTag);     //Returns the CanWalkOver bool

	public virtual void OnLeaved(BaseTile iTile) {}
	public virtual void OnCollided(BaseUnit iUnit) {}
	public virtual void OnArrived(BaseTile iTile, List<BaseUnit> iPreviousUnits) {}
	public virtual void OnArrivedToMe(BaseUnit iUnit) {}

	public void DestroyUnit() {
		BaseTile.HandleOccupy(this, OccupiedTile, null); // Need to leave the grid before destroying!
		Destroy(gameObject);
	}

	protected override void OnActivated() {
		if (OccupiedTile.CanWalkOn(this)) {
			BaseTile.HandleOccupy(this, null, OccupiedTile);
		}
	}
	protected override void OnDeactivated() {
		BaseTile.HandleOccupy(this, OccupiedTile, null);
	}
}