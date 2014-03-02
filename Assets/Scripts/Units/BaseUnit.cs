using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public abstract class BaseUnit : MonoBehaviour
{
    public abstract bool CanWalkOver { get; }
	public abstract int LayerMask { get; }
	public BaseTile OccupiedTile { get; set; }

    public virtual void SetActive(bool iActive)
    {
        throw new NotImplementedException();
    }

    public abstract bool CanWalkOn(string incomingUnitTag);     //Returns the CanWalkOver bool

	public virtual void OnLeaved(BaseTile iTile) {}
	public virtual void OnCollided(BaseUnit iUnit) {}
	public virtual void OnArrived(BaseTile iTile, List<BaseUnit> iPreviousUnits) {}
	public virtual void OnArrivedToMe(BaseUnit iUnit) {}

	public void DestroyUnit() {
		BaseTile.HandleOccupy(this, OccupiedTile, null); // Need to leave the grid before destroying!
		Destroy(gameObject);
	}
}