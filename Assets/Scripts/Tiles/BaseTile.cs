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
	
	public void Occupy(BaseUnit iUnit, BaseTile iPreviousTile) {
		mUnit = iUnit;
		if (iPreviousTile != null)
			iPreviousTile.Unoccupy(iUnit, this);
	}

	protected void Unoccupy(BaseUnit iUnit, BaseTile iPreviousTile) {
		mUnit = null;
	}

	public void Arrive(BaseUnit iUnit, BaseTile iPreviousTile) {
		iUnit.OnArrived(this, mUnit);
		if (iPreviousTile != null) {
			OnArrived(mUnit, iPreviousTile);
			iPreviousTile.Leave(iUnit, iPreviousTile);
		}
	}

	protected void Leave(BaseUnit iUnit, BaseTile iNextTile) {
		iUnit.OnLeaved(this);
		OnLeaved(iUnit, iNextTile);
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
