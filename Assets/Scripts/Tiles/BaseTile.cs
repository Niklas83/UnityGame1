using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public abstract class BaseTile : MonoBehaviour, IActivatable
{
	public bool Occupied { get { return mUnit != null; } }
	protected GridManager GridManager { get { return mGridManager; } }

	public bool Active = true;

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
			mUnit.OccupiedTile.Leave(this);

		BaseTile previousTile = mUnit.OccupiedTile;
		mUnit.OccupiedTile = this;
		OnArrived(mUnit, previousTile);
	}
	
	private void Leave(BaseTile iNextTile) {
		OnLeaved(mUnit, iNextTile);
		mUnit = null;
	}
	
	public BaseUnit GetOccupyingUnit() {
		return mUnit;
	}

	public void SetActive(bool iActive) {
		Active = iActive;
		GetComponent<MeshRenderer>().enabled = iActive;
		if (iActive)
			OnActivated();
		else
			OnDeactivated();
	}

	public bool IsActive() {
		return Active;
	}

	protected abstract void OnLeaved(BaseUnit iUnit, BaseTile iNextTile);
	protected abstract void OnArrived(BaseUnit iUnit, BaseTile iPreviousTile);

	protected void OnActivated() {}
	protected void OnDeactivated() {}
}
