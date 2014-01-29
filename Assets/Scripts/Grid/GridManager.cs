using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class GridManager
{
	private BaseTile[,] mGrid;
	private List<BaseTile> mTilesForQueries;
	private Iterator mIterator;

	public GridManager() {}

	public void CreateGrid(int iSizeX, int iSizeY)
	{
		mGrid = new BaseTile[iSizeX, iSizeY];
		mTilesForQueries = new List<BaseTile>(iSizeX * iSizeY);
	}

	public void PositionToIndices(Vector3 iPosition, out int oXindex, out int oYindex) {
		oXindex = Mathf.RoundToInt(iPosition.x);
		oYindex = Mathf.RoundToInt(iPosition.z);
	}

	public void AddTile(BaseTile iTile) {
		int x, y;
		PositionToIndices(iTile.transform.position, out x, out y);
		mGrid[x, y] = iTile;
	}
	public void RemoveTile(BaseTile iTile) {
		int x, y;
		PositionToIndices(iTile.transform.position, out x, out y);
		mGrid[x, y] = null;
	}

	public bool InRange(Vector3 iPosition) {
		int x, y;
		PositionToIndices(iPosition, out x, out y);
		return InRange(x, y);
	}
	public bool InRange(int iXindex, int iYindex) 
	{
		return iXindex < mGrid.GetLength(0) && iXindex >= 0 &&
			   iYindex < mGrid.GetLength(1) && iYindex >= 0;
	}

	public BaseTile GetTile(Vector3 iPosition)
	{
		int x, y;
		PositionToIndices(iPosition, out x, out y);
		return GetTile(x, y);
	}
	public BaseTile GetTile(int iXindex, int iYindex)
	{
		if (InRange(iXindex, iYindex))
			return mGrid[iXindex, iYindex];

		return null;
	}

	public Vector2 PositionToIndices(Vector3 iPosition)
	{
		return new Vector2(Mathf.RoundToInt(iPosition.x), Mathf.RoundToInt(iPosition.z));
	}

	// Gets the tile at the given indices
	protected void RemoveTile(int iXindex, int iYindex)
	{
		mGrid[iXindex, iYindex] = default(BaseTile);
	}
	
	public int GetLength(int iDimension)
	{
		return mGrid.GetLength(iDimension);
	}

	public bool IsOccupied(int iXindex, int iYindex)
	{
		if (!InRange(iXindex, iYindex)) return true; // Is no tile at all occupied or not? ;)

		BaseTile t = GetTile(iXindex, iYindex);
		return t.Occupied;
	}

	public void GetAll<T>(out List<BaseTile> oTiles) where T : BaseTile
	{
		mTilesForQueries.Clear();
		
		Iterator it = GetIterator();
		while (it.HasNext())
		{
			BaseTile t = it.Next();
			if (t != null && t is T)
			{
				mTilesForQueries.Add(t);	
			}
		}
		
		oTiles = mTilesForQueries;
	}

	public Iterator GetIterator()
	{
		if (mIterator == null)
		{
			mIterator = new Iterator();
			mIterator.Initialize(mGrid);
		}
		
		mIterator.Reset();
		return mIterator;
	}
	
	public class Iterator
	{
		private int mCursorX = 0;
		private int mCursorY = 0;
		
		private int mSizeX;
		private int mSizeY;
		
		private BaseTile[,] mGrid;
		
		public void Initialize(BaseTile[,] iGrid)
		{
			mGrid = iGrid;
			mSizeX = mGrid.GetLength(0);
			mSizeY = mGrid.GetLength(1);
		}
		
		public void Reset()
		{
			mCursorX = 0;
			mCursorY = 0;
		}
		
		public BaseTile Next()
		{
			mCursorX++;
			if (mCursorX >= mSizeX)
			{
				mCursorX = 0;
				mCursorY++;
				if (mCursorY >= mSizeY)
					return default(BaseTile);
			}
			return mGrid[mCursorX, mCursorY];
		}
		
		public bool HasNext()
		{
			return mCursorY < mGrid.GetLength(1);
		}
	}
}
