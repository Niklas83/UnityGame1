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

public static class Defines 
{
	public static int TILE_SIZE = 1; // Size of tile in all dimensions.

	public static float RESOLUTION_WIDTH = 2048f; // iPad with retina
	public static float RESOLUTION_HEIGHT = 1536f; // iPad with retina
}
