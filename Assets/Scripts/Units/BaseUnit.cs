using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public abstract class BaseUnit : MonoBehaviour
{
	private BaseTile mOccupiedTile;
	public BaseTile OccupiedTile
	{
		get { return mOccupiedTile; }
		set { mOccupiedTile = value; }
	}

	void Start() { }
}