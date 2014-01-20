using System.Collections.Generic;
using UnityEngine;
using System.Collections;

public class ObjectLocator : MonoBehaviour {

    //Non-movable boxes
    private GameObject AllStaticBoxes;                 //Hierarchy-list objektet som har alla boxar som childs
    public List<Vector3> AllStaticBoxPositions = new List<Vector3>();    //Listan som håller alla coordinater på boxar som ej kan flyttas
    private bool LocatedAllStaticBoxes = false;     //sätts till true då det gjorts en kontroll om vart alla statiska, ej flyttbara boxar är så den inte görs igen

    //walls
    private GameObject AllWalls;                 //Hierarchy-list objektet som har alla yttre väggboxar som childs
    public List<Vector3> AllWallPositions = new List<Vector3>();    //Listan som håller alla coordinater på yttre väggar
    private bool LocatedAllWalls = false;           //sätts till true då det gjorts en kontroll om vart alla väggar är så den inte görs igen

    //candles
    private GameObject AllCandles;               //Hierarchy-list objektet som har alla ljus som child
    public List<Vector3> AllCandlePositions = new List<Vector3>(); //Listan som håller alla coordinater på ljus
    private bool LocatedAllCandles = false;         //sätts till true då det gjorts en kontroll om vart alla ljus är så den inte görs igen

    //MovableBoxs
    public GameObject AllMovableBoxes;                 //Hierarchy-list objektet som har alla boxar som childs
    public List<Vector3> AllMovableBoxPositions = new List<Vector3>();    //Listan som håller alla coordinater på boxar som KAN flyttas
    public bool LocatedAllMovableBoxes = false;     //sätts till true då det gjorts en kontroll om vart alla flyttbara boxar är, (denna kontroll ska göras då en flyttbar box flyttats)
	
	// Update is called once per frame
    void Update()
    {
        //Kollar om alla boxar tagits fram en gång
        if (this.LocatedAllStaticBoxes == false)
        {
            //Tar fram alla coordinater på boxar
            AllStaticBoxes = GameObject.Find("AllTheBoxes");

            int NumberOfBoxChilds = AllStaticBoxes.transform.childCount;

            for (int i = 0; i < NumberOfBoxChilds; i++)
            {
                Transform box = AllStaticBoxes.transform.GetChild(i);
                AllStaticBoxPositions.Add(box.position);
            }
            this.LocatedAllStaticBoxes = true;
        }

        //Kollar om alla flyttbara boxar tagits fram en gång
        if (this.LocatedAllMovableBoxes == false)
        {
            //Tar fram alla coordinater på boxar
            this.AllMovableBoxes = GameObject.Find("AllMovableBoxes");

            int NumberOfBoxChilds = AllMovableBoxes.transform.childCount;

            for (int i = 0; i < NumberOfBoxChilds; i++)
            {
                Transform box = AllMovableBoxes.transform.GetChild(i);
                AllMovableBoxPositions.Add(box.position);
            }
            this.LocatedAllMovableBoxes = true;
        }

        //Kollar om alla väggar tagits fram en gång
        if (this.LocatedAllWalls == false)
        {
            //Tar fram alla coordinater på väggar
            AllWalls = GameObject.Find("AllTheWallSquares");

            int NumberOfWallChilds = AllWalls.transform.childCount;

            for (int i = 0; i < NumberOfWallChilds; i++)
            {
                Transform wall = AllWalls.transform.GetChild(i);
                AllWallPositions.Add(wall.position);
            }

            this.LocatedAllWalls = true;
        }

        //Kollar om alla ljus tagits fram en gång
        if (LocatedAllCandles == false)
        {
            AllCandles = GameObject.Find("AllTheCandles");

            int NumberOfCandles = AllCandles.transform.childCount;

            for (int i = 0; i < NumberOfCandles; i++)
            {
                Transform candle = AllCandles.transform.GetChild(i);
                AllCandlePositions.Add(candle.position);
            }

            LocatedAllCandles = true;
        }


    }


    //Return object type methods
    public List<Vector3> GetAllNonMovableBoxPositions()
    {
        return this.AllStaticBoxPositions;
    }

    public List<Vector3> GetAllWallPositions()
    {
        return this.AllWallPositions;
    }

    public List<Vector3> GetAllCandlePositions()
    {
        return this.AllCandlePositions;
    }

    public List<Vector3> GetAllMovableBoxPositions()
    {
        return this.AllMovableBoxPositions;
    }



    //Set bools that the location has been moved of some type of item

    public void SetLocatedAllStaticBoxesFalse()
    {
        this.LocatedAllStaticBoxes = false;
    }

    public void SetLocatedAllWallsFalse()
    {
        this.LocatedAllWalls = false;
    }

    public void SetLocatedAllCandlesFalse()
    {
        this.LocatedAllCandles = false;
    }

    public void SetLocatedAllMovableBoxesFalse()
    {
        this.LocatedAllMovableBoxes = false;
    }












   // Kör FindObjectOfType för att hitta objectLocator instansen
}
