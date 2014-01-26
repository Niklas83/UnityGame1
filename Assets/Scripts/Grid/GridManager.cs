using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class GridManager
{
	private BaseTile[,] mGrid;
	private List<BaseTile> mTilesForQueries;
	private Iterator mIterator;
	
	public GridManager(BaseTile[,] iGrid)
	{
		mGrid = iGrid;
		mTilesForQueries = new List<BaseTile>(GetLength(0) * GetLength(1));
	}

	public bool InRange(int iXindex, int iYindex) 
	{
		return iXindex < mGrid.GetLength(0) && iXindex >= 0 &&
			   iYindex < mGrid.GetLength(1) && iYindex >= 0;
	}

	// Gets the tile at the given indices
	public BaseTile GetTile(Vector2 iIndices)
	{
		int x = (int) iIndices.x;
		int y = (int) iIndices.y;
		return GetTile(x, y);
	}
	public BaseTile GetTile(int iXindex, int iYindex)
	{
		if (!InRange(iXindex, iYindex))
		{
			throw new ArgumentOutOfRangeException("Trying to get tile outside grid!");
		}

		return mGrid[iXindex, iYindex];
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

	public void GetAll(TileTypes iType, out List<BaseTile> oTiles)
	{
		mTilesForQueries.Clear();
		
		Iterator it = GetIterator();
		while (it.HasNext())
		{
			BaseTile t = it.Next();
			if (t != null && t.TileType == iType)
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
