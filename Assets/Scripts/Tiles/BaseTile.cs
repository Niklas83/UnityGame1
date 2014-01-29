using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public abstract class BaseTile : MonoBehaviour
{
	public bool Occupied { get { return mUnit != null; } }
	protected GridManager GridManager { get { return mGridManager; } }

	private BaseUnit mUnit; 		// The unit occupying this tile
	private GridManager mGridManager;

	public void Init(GridManager iGridManager) {
		mGridManager = iGridManager;
	}
	
	public void Occupy(BaseUnit iUnit)
	{
		if (mUnit == iUnit)
			return;
		
		if (iUnit.OccupiedTile == this)
		{
			mUnit = iUnit;
			return;
		}
		
		mUnit = iUnit;
		if (mUnit.OccupiedTile != null)
			mUnit.OccupiedTile.Leave();

		mUnit.OccupiedTile = this;
		OnArrived(mUnit);
	}
	
	private void Leave()
	{
		OnLeaved(mUnit);
		mUnit = null;
	}
	
	public BaseUnit GetOccupyingUnit()
	{
		return mUnit;
	}

	protected abstract void OnLeaved(BaseUnit iUnit);
	protected abstract void OnArrived(BaseUnit iUnit);
}
