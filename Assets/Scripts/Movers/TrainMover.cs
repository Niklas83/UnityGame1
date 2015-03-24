using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TrainMover : Mover {

    private List<BaseTile> NeighBouringTrainTiles;  //all neighbouting tiles that has traintile set to true

    private Vector3 PreviousLocation; //save this so that the train wont go back and forth like a spasitc (so when possible the train will keep moving in a straight line before turn)


    void Update()
    {
        if (!isMoving)
        {
            NeighBouringTrainTiles = new List<BaseTile>();          //all neighbouting tiles that has traintile set to true

            List<BaseTile> NeighbouringTiles  = new List<BaseTile>();       //All neighbouring tiles


            Vector3 currentLocation = transform.position;
            if (gridManager.GetTile(transform.position + new Vector3(0, 0, 1)) != null)
            {
                NeighbouringTiles.Add(gridManager.GetTile(transform.position + new Vector3(0, 0, 1)));
            }
            if (gridManager.GetTile(transform.position + new Vector3(1, 0, 0)) != null)
            {
                NeighbouringTiles.Add(gridManager.GetTile(transform.position + new Vector3(1, 0, 0)));
            }
            if (gridManager.GetTile(transform.position + new Vector3(0, 0, -1)) != null)
            {
                NeighbouringTiles.Add(gridManager.GetTile(transform.position + new Vector3(0, 0, -1)));
            }
            if (gridManager.GetTile(transform.position + new Vector3(-1, 0, 0)) != null)
            {
                NeighbouringTiles.Add(gridManager.GetTile(transform.position + new Vector3(-1, 0, 0)));
            }

            foreach (BaseTile neighbouringTile in NeighbouringTiles)
            {
                if (neighbouringTile.TrainTile == true)
                {
                    NeighBouringTrainTiles.Add(neighbouringTile);
                }
            }

            int IndexOfTileToDelete = -1;
            
            if (PreviousLocation != null && NeighBouringTrainTiles.Count > 1)
            {
                for (int i = 0; i < NeighBouringTrainTiles.Count; i++)
                {

                    if (NeighBouringTrainTiles[i].transform.position.x == PreviousLocation.x && NeighBouringTrainTiles[i].transform.position.z == PreviousLocation.z)
                    {
                        IndexOfTileToDelete = i;
                    }
                }
            }

            if (IndexOfTileToDelete != -1)
            {
                NeighBouringTrainTiles.RemoveAt(IndexOfTileToDelete);
            }

            int randomTrainTile = Random.Range(1, NeighBouringTrainTiles.Count +1);


            BaseTile selectedBaseTile = NeighBouringTrainTiles[randomTrainTile - 1];

            PreviousLocation = currentLocation;

            Vector3 tileToMoveTo = selectedBaseTile.transform.position;

            //selectedBaseTile.renderer.material.color = Color.green;

            TryMove(Mathf.RoundToInt(tileToMoveTo.x - transform.position.x), Mathf.RoundToInt(tileToMoveTo.z - transform.position.z));

        }
		
    }


    // Checks if this mover can move in the given direction.
    public override bool TryMove(int xDir, int zDir)
    {
        if (isMoving)
            return false;

        BaseTile tile = gridManager.GetTile(transform.position + new Vector3(xDir, 0, zDir));

        if (tile == null || !tile.IsActive()) // Can't move to a non existing tile.
            return false;

        bool canMove = CanMove(tile, xDir, zDir);
        if (canMove)
            StartCoroutine(Move(xDir, zDir));

        return canMove;
    }



}