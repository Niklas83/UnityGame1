using UnityEngine;
using System.Collections;

public class Floor : MonoBehaviour {

	private GridManager _gridManager = new GridManager();
	public GridManager GridManager { get { return _gridManager; } }

	// Use this for initialization
	void Awake() {
		CreateGrid();
	}

	void CreateGrid() {
		GameObject floorTiles = new GameObject("Floor Tiles"); // An object to store the tiles in the Hiearchy. Just for tidyness ;).
		floorTiles.AddComponent<LevelInfo>(); // This should be done manually for the designed floors.

		Bounds levelBounds = new Bounds();
		int nrTiles = this.transform.childCount;
		for (int i = 0; i < nrTiles; i++) {
			Transform child = transform.GetChild(i);
			if (child.renderer)
				levelBounds.Encapsulate(child.renderer.bounds);
		}

		Vector3 size = levelBounds.size;
		
		_gridManager.CreateGrid((int)size.x + 1, (int)size.z + 1);
		for (int i = 0; i < nrTiles; i++) {
			BaseTile tile = transform.GetChild(i).GetComponent<BaseTile>();
			tile.Init(_gridManager);
			_gridManager.AddTile(tile);
		}

		Object[] allUnits = Object.FindObjectsOfType<BaseUnit>();
		for (int i = 0; i < allUnits.Length; i++) {
			BaseUnit bu = allUnits[i] as BaseUnit;
			bu.Init(_gridManager);
			BaseTile tile = _gridManager.GetTile(bu.transform.position);
			DebugAux.Assert(tile != null, "Can't have a unit placed on a non-tile " + bu);
			BaseTile.TeleportTo(bu, null, tile);
		}
	}
	
	// Update is called once per frame
	void Update () {
	}
}
