using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public abstract class BaseUnit : MonoBehaviour
{
	public abstract bool CanWalkOn { get; }

	private BaseTile mOccupiedTile;
	public BaseTile OccupiedTile
	{
		get { return mOccupiedTile; }
		set { mOccupiedTile = value; }
	}

	void Start() { }
}