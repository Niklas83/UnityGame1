using UnityEngine;
using System.Collections;
using System;

public enum Direction
{
	Up = 0,
	Right = 1,
	Down = 2,
	Left = 3,
}

[Flags]
public enum TileTypes
{
	None = 0,
	Floor = 1 << 0,
}

public static class Defines 
{ }
