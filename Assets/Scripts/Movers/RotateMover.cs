using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using System.Collections;

public class RotateMover : MonoBehaviour {

    public bool IsPusher = false; // Can push stuff around

    // Private members
    private bool mIsMoving = false;

    private BaseUnit mUnit;
    private GridManager mGridManager;

    // Public properties
    public bool IsMoving { get { return mIsMoving; } }

    public void Start()
    {
        Floor floor = Helper.Find<Floor>("Floor");
        mGridManager = floor.GridManager;

        mUnit = GetComponent<BaseUnit>();
    }

    // Checks if this mover can move in the given direction.
    public bool TryMove(int xDir, int yDir, Vector3 incommingObjectPosition)
    {
        GameObject rotateBeamObject = this.gameObject.transform.parent.gameObject;

        //tilen som passeras i rotationen
        BaseTile tilePassingBy = mGridManager.GetTile(transform.position + new Vector3(xDir, 0, yDir));

        //tilen som bommen slutligen hamnar på
        BaseTile tileEndPosition = mGridManager.GetTile(rotateBeamObject.transform.position + new Vector3(xDir, 0, yDir));

        if ((tilePassingBy == null || !tilePassingBy.IsActive()) ||
            (tileEndPosition == null || !tileEndPosition.IsActive())) // Can't move to a non existing tile.
        {
            return false;
        }

        bool passingByCanMove = true;
        bool endPositionCanMove = true;

        //Kontrollerar hur vida positionen som bommen passerar samt den slutgiltiga positionen är ockuperad och hur vida den kan flytta vidare eller ej
        bool occupiedPassingBy = tilePassingBy.Occupied;
        bool occupiedEndPosition = tileEndPosition.Occupied;

        //TODO: Fixa så de som blir pushade glider åt olika håll? Just nu åker båda eventualla unit som är i vägen åt samma håll (både passingby- och endPostition objekten)

        if (occupiedPassingBy)
        { // Something is in the place where we are passing by
            BaseUnit unitPassingBy = tilePassingBy.GetOccupyingUnit();

            passingByCanMove = unitPassingBy == mUnit || unitPassingBy.CanWalkOn(this.gameObject.tag); // You can walk here if it's to yourself or to a "walkable" unit.
            if (!passingByCanMove && IsPusher)
            {
                // If not a walkable, check if you are a 'Pusher' and it can be moved.

                Mover mover = unitPassingBy.GetComponent<Mover>();
                if (mover != null)
                {
                    passingByCanMove = mover.TryMove(xDir, yDir);
                }

                //TODO: Slå ihop ovanliggande mover och rotateMover med hjälp av att göra en BaseMover eller liknande
                RotateMover rotateMover = unitPassingBy.GetComponent<RotateMover>();
                if (rotateMover != null)
                {
                    passingByCanMove = rotateMover.TryMove(xDir, yDir, this.gameObject.transform.position);
                }
            }
        }

        if (occupiedEndPosition && passingByCanMove)
        { // Something is in the place we want to move
            BaseUnit unitEndPosition = tileEndPosition.GetOccupyingUnit();

            endPositionCanMove = unitEndPosition == mUnit || unitEndPosition.CanWalkOn(this.gameObject.tag); // You can walk here if it's to yourself or to a "walkable" unit.
            if (!endPositionCanMove && IsPusher)
            {
                // If not a walkable, check if you are a 'Pusher' and it can be moved.

                Mover mover = unitEndPosition.GetComponent<Mover>();
                if (mover != null)
                {
                    endPositionCanMove = mover.TryMove(xDir, yDir);
                }

                //TODO: Slå ihop ovanliggande mover och rotateMover med hjälp av att göra en BaseMover eller liknande
                RotateMover rotateMover = unitEndPosition.GetComponent<RotateMover>();
                if (rotateMover != null)
                {
                    endPositionCanMove = rotateMover.TryMove(xDir, yDir, this.gameObject.transform.position);
                }
            }
        }

        if (passingByCanMove && endPositionCanMove && IsMoving == false)
            StartCoroutine(Move(xDir, yDir, incommingObjectPosition));

        if (passingByCanMove == false || endPositionCanMove == false)
        {
            return false;
        }
        else
        {
            return true;
        }

    }
    
    public IEnumerator Move(int xDir, int zDir, Vector3 incommingObjectPosition)
    {
        mIsMoving = true;
        GameObject rotateBeamObject = this.gameObject.transform.parent.gameObject;

        int rotateDegrees = 0;

        if (zDir == -1)
        {
            if (incommingObjectPosition.x > rotateBeamObject.transform.position.x)
            {
                rotateDegrees = 90;
            }
            else
            {
                rotateDegrees = -90;
            }
        }

        else if (zDir == 1)
        {
            if (incommingObjectPosition.x > rotateBeamObject.transform.position.x)
            {
                rotateDegrees = -90;
            }
            else
            {
                rotateDegrees = 90;
            }
        }
        else if (xDir == -1)
        {
            if (incommingObjectPosition.z > rotateBeamObject.transform.position.z)
            {
                rotateDegrees = -90;
            }
            else
            {
                rotateDegrees = 90;
            }
        }

        else if (xDir == 1)
        {
            if (incommingObjectPosition.z > rotateBeamObject.transform.position.z)
            {
                rotateDegrees = 90;
            }
            else
            {
                rotateDegrees = -90;
            }
        }

        //else          //TODO: fixa så att det görs en kontroll om man kommer från sidan av bommen för kommande liknande objekt
        //{
        //    hitSide = true;
        //}


        // Rotera smooth
        if (rotateDegrees > 0)
        {
            for (int i = 0; i < rotateDegrees; i += 6)
            {
                rotateBeamObject.transform.Rotate(0, 6, 0);
                yield return new WaitForSeconds(0.0001f);
            }
        }

        if (rotateDegrees < 0)
        {
            for (int i = 0; i > rotateDegrees; i += -6)
            {
                rotateBeamObject.transform.Rotate(0, -6, 0);
                yield return new WaitForSeconds(0.0001f);
            }
        }

        //tar fram platsen på gridden igen
        BaseTile tile = mGridManager.GetTile(this.transform.position);
        tile.Occupy(mUnit);
        mIsMoving = false;

        yield return 0;
    }
}
