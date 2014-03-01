using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public abstract class BaseUnit : MonoBehaviour
{
    public abstract bool CanWalkOver { get; }
    public virtual void SetActive(bool iActive)
    {
        throw new NotImplementedException();
    }

    public abstract bool CanWalkOn(string incomingUnitTag);     //Returns the CanWalkOver bool

	public virtual void OnLeaved(BaseTile iTile) {}
	public virtual void OnCollided(BaseUnit iUnit) {}
	public virtual void OnArrived(BaseTile iTile, BaseUnit iUnit) {}
}