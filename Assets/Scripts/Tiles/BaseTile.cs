using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public abstract class BaseTile : MonoBehaviour
{
	public abstract TileTypes TileType { get; }

	public bool Occupied { get { return mUnit != null; } }
	private BaseUnit mUnit; 		// The unit occupying this tile

	void Start() {}
	
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
	}
	
	public void Leave()
	{
		mUnit = null;
	}
	
	public BaseUnit GetOccupyingUnit()
	{
		return mUnit;
	}
}
