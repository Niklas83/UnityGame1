using UnityEngine;
using System.Collections;

public class GridManagerExample : MonoBehaviour
{
	public GameObject FloorSquare;
	public GameObject MovableBox;
	public GameObject Player;

	public Vector2 Size;

	private GridManager mGridManager;
	public GridManager GridManager { get { return mGridManager; } }

	void Start ()
	{
		CreateGrid();
		CreateBoxes();
		CreatePlayer();
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
		mGridManager = new GridManager(grid);
	}

	void CreateBoxes() {
		GameObject boxes = new GameObject("Boxes"); // An object to store the tiles in the Hiearchy. Just for tidyness ;).
		for (int i = 0; i < Size.x; i++) {
			for (int j = 0; j < Size.y; j++) {
				if (!mGridManager.IsOccupied(i - 1, j)) { // Only creates a unit if there isn't one to the left (creates "stripes").
					BaseUnit unit = Helper.Instansiate<Unit>(MovableBox, boxes);
					unit.transform.position = new Vector3(i, 1, j);
					mGridManager.GetTile(i, j).Occupy(unit);
				}
			}
		}
	}

	void CreatePlayer()
	{
		BaseUnit unit = Helper.Instansiate<PlayerUnit>(Player);
		unit.transform.position = new Vector3(8, 1, 8);
		mGridManager.GetTile(8, 8).Occupy(unit);
	}
}

