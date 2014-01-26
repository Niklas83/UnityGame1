using UnityEngine;
using System.Collections;

public class GridManagerExample : MonoBehaviour
{
	public GameObject FloorSquare;
	public GameObject MovableBox;
	public Vector2 Size;

	void Start ()
	{
		CreateGrid();
	}

	void CreateGrid() {
		GameObject floorTiles = new GameObject("Floor Tiles"); // An object to store the tiles in the Hiearchy. Just for tidyness ;).

		BaseTile[,] grid = new BaseTile[(int)Size.x, (int)Size.y];
		for (int i = 0; i < Size.x; i++) {
			for (int j = 0; j < Size.y; j++) {
				BaseTile newTile = Helper.Instansiate<FloorTile>(FloorSquare, floorTiles) as BaseTile;
				newTile.transform.position = new Vector3(i, 0, j);
				grid[i, j] = newTile;
			}
		}
		GridManager gm = new GridManager(grid);

		GameObject boxes = new GameObject("Boxes"); // An object to store the tiles in the Hiearchy. Just for tidyness ;).
		for (int i = 0; i < Size.x; i++) {
			for (int j = 0; j < Size.y; j++) {
				if (!gm.IsOccupied(i - 1, j)) { // Only creates a unit if there isn't one to the left (creates "stripes").
					BaseUnit newUnit = Helper.Instansiate<Unit>(MovableBox, boxes);
					newUnit.transform.position = new Vector3(i, 1, j);
					gm.GetTile(i, j).Occupy(newUnit);
				}
			}
		}
	}

	// Update is called once per frame
	void Update ()
	{

	}
}

