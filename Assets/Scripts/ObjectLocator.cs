using System.Collections.Generic;
using UnityEngine;
using System.Collections;

public class ObjectLocator : MonoBehaviour {

    //Non-movable boxes
    private GameObject AllStaticBoxes;                 //Hierarchy-list objektet som har alla boxar som childs
    private List<Vector3> AllStaticBoxPositions = new List<Vector3>();    //Listan som håller alla coordinater på boxar som ej kan flyttas
    private bool LocatedAllStaticBoxes = false;     //sätts till true då det gjorts en kontroll om vart alla statiska, ej flyttbara boxar är så den inte görs igen

    //walls
    private GameObject AllWalls;                 //Hierarchy-list objektet som har alla yttre väggboxar som childs
    private List<Vector3> AllWallPositions = new List<Vector3>();    //Listan som håller alla coordinater på yttre väggar
    private bool LocatedAllWalls = false;           //sätts till true då det gjorts en kontroll om vart alla väggar är så den inte görs igen

    //candles
    private GameObject AllCandles;               //Hierarchy-list objektet som har alla ljus som child
    private List<Vector3> AllCandlePositions = new List<Vector3>(); //Listan som håller alla coordinater på ljus
    private bool LocatedAllCandles = false;         //sätts till true då det gjorts en kontroll om vart alla ljus är så den inte görs igen

    //MovableBoxs
    private GameObject AllMovableBoxes;                 //Hierarchy-list objektet som har alla boxar som childs
    private List<Vector3> AllMovableBoxPositions = new List<Vector3>();    //Listan som håller alla coordinater på boxar som KAN flyttas
    private bool LocatedAllMovableBoxes = false;     //sätts till true då det gjorts en kontroll om vart alla flyttbara boxar är, (denna kontroll ska göras då en flyttbar box flyttats)

    //Rotatable Beams
    private GameObject AllRotatableBeams;                 //Hierarchy-list objektet som har alla Rotatable Beams som childs
    public List<Vector3> AllRotatableBeamsPositions = new List<Vector3>();    //Listan som håller alla coordinater på alla Rotatable Beams
    public List<Vector3> AllPartsOfRotatableBeamsPositions = new List<Vector3>();      //Listar alla delobjekt av rotatable beams
    private bool LocatedAllRotatableBeams = false;     //sätts till true då det gjorts en kontroll om vart alla Rotatable Beams är, (denna kontroll ska göras då en flyttbar box flyttats)

    //portal till nästa bana
    public Transform PortalPrefab;          //Portal objektet för GameObject (klassen som instansieras)
    public Transform Portal;            //Det instantierade portal-objektet som skapas då alla ljus är tagna

    private bool PortalPlaced = false;  //Variabel för hurvida portalen blivit placerad eller ej


    void Start()
    {
        UpdateAllPositions();
    }


	// Update is called once per frame
    public void UpdateAllPositions()
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

        //Kollar om alla RotatableBeams tagits fram
        if (this.LocatedAllRotatableBeams == false)
        {
            AllRotatableBeams = GameObject.Find("AllRotatableBeams");       //kan flytta denna till start metoden

            int NumberOfRotatableBeamsChilds = AllRotatableBeams.transform.childCount;

            for (int i = 0; i < NumberOfRotatableBeamsChilds; i++)
            {
                //Listar den specifika rotatableBeam
                Transform RotatableBeam = AllRotatableBeams.transform.GetChild(i);
                AllRotatableBeamsPositions.Add(RotatableBeam.position);

                //Listar alla childObjects i den specifika rotatableBeam
                int NumberOfChildsInCurrentRotatableBeam = RotatableBeam.childCount;

                for (int j = 0; j < NumberOfChildsInCurrentRotatableBeam; j++)
                {
                    Transform RotatableBeamPart = RotatableBeam.transform.GetChild(j);

                    AllPartsOfRotatableBeamsPositions.Add(RotatableBeamPart.position);
                }

            }

            this.LocatedAllRotatableBeams = true;
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

    //kan kanske ta bort denna
    //public List<Vector3> GetAllRotatableBeamsPositions()
    //{
    //    return this.AllRotatableBeamsPositions;
    //}

    public List<Vector3> GetAllPartsOfRotatableBeamsPositions()
    {
        return this.AllPartsOfRotatableBeamsPositions;
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

    public void SetLocatedAllRotatableBeamsFalse()
    {
        this.LocatedAllMovableBoxes = false;
    }




    //Remove Candle from coordinate your walking to
    public bool RemoveCandleOnTileWalkingTo(Vector3 EndPositionOfPlayer)
    {
        bool candleHasBeenRemoved = false;      //Värdet som kommer returneras

        //Ljus objekt att ta bort
        Transform candleToRemove = AllCandles.transform.GetChild(0); // Detta värde kommer alltid att sättas i foreach loopen nedan

        //Hitta ljus objekt
        for (int x = 0; x < AllCandles.transform.childCount; x++)
        {
            Transform candle = AllCandles.transform.GetChild(x);
            if (candle.position == EndPositionOfPlayer)
            {
                candleToRemove = candle;
                continue;
            }
        }

        //Ljus koordinat att ta bort
        Vector3 postionToRemove = new Vector3();

        //Hitta ljus koordinat
        for (int j = 0; j < AllCandlePositions.Count; j++)
        {
            Vector3 position = AllCandlePositions[j];

            if (position == candleToRemove.position)
            {
                postionToRemove = position;
                continue;
            }
        }

        AllCandlePositions.Remove(postionToRemove);     //Tar bort från listan med coordinater på ljus

        GameObject candleToDestroy = GameObject.Find(candleToRemove.name);
        if (candleToDestroy != null)
        {
            Destroy(candleToDestroy); //Tar bort det faktiska objektet
            candleHasBeenRemoved = true;    //Sätter att ljuset har tagits bort och returnerar true
        }

        //skapa portal om candle antalet blir noll
        if (AllCandlePositions.Count == 0 && PortalPlaced != true)
        {
            //Kontrollerar sedan om vart alla ljus finns på banan (Detta gör jag efter för att minska onödigt loopande)
            var theFloor = GameObject.Find("AllTheFloorSquares");

            int numberOfFloorTiles = theFloor.transform.childCount;


            Transform randomFloorTile = theFloor.transform.GetChild(Random.Range(1, numberOfFloorTiles));
            Transform pentagram;
            float heightY = (float)0.5;
            pentagram = (Transform)Instantiate(PortalPrefab, new Vector3(randomFloorTile.position.x, heightY, randomFloorTile.position.z), Quaternion.identity);
            pentagram.name = "ThePortal";

        }

        return candleHasBeenRemoved;
    }


    // MOVABLE BOX METHODS //
    //Find movable box
    public Transform GetBoxToPushFromTileWalkingTo(Vector3 positionWalkingTo)
    {
        Transform MovingBox = transform;     //Har portal här för att scriptet inte ska gnälla om att det kan vara null längre ner
        for (int i = 0; i < AllMovableBoxes.transform.childCount; i++)
        {
            Transform theBoxToMove = AllMovableBoxes.transform.GetChild(i);
            if (theBoxToMove.position == positionWalkingTo)
            {
                MovingBox = theBoxToMove;
            }
        }
        return MovingBox;
    }

    //Move movable box
    public bool MoveMovableBox(Transform movingBox, float moveBoxX, float moveBoxZ)
    {

            GameObject MovableBox = GameObject.Find(movingBox.name);
            BoxMove BoxMoveScript = MovableBox.GetComponent<BoxMove>();
            bool canMoveToPreviousObsticleLocation = BoxMoveScript.MoveBox(moveBoxX, moveBoxZ);
            LocatedAllMovableBoxes = false;
            AllMovableBoxPositions = new List<Vector3>();

            return canMoveToPreviousObsticleLocation;
    }


    // ROTATABLE BEAMS METHODS //
    // find the rotatable beam
    public Transform GetRotatableBeamToTurnFromTileWalkingTo(Vector3 positionWalkingTo)
    {
        Transform RotatingBeam = transform;     //Har portal här för att scriptet inte ska gnälla om att det kan vara null längre ner

        for (int i = 0; i < AllRotatableBeams.transform.childCount; i++)
        {
            Transform theBeamToTurn = AllRotatableBeams.transform.GetChild(i);

            for (int j = 0; j < theBeamToTurn.childCount; j++)
            {
                Transform partOfTheBeamToTurn = theBeamToTurn.GetChild(j);

                if (partOfTheBeamToTurn.position == positionWalkingTo)
                {
                    RotatingBeam = theBeamToTurn;
                }
            }
        }

        return RotatingBeam;
    }

    //Rotate the rotatable beam
    public bool RotateRotatableBeam(Transform rotatableBeam, float moveBoxX, float moveBoxZ)
    {
        GameObject RotatableBeam = GameObject.Find(rotatableBeam.name);
        RotatableBeamMove RotatableBeamMoveScript = RotatableBeam.GetComponent<RotatableBeamMove>();
        bool canMoveToPreviousObsticleLocation = RotatableBeamMoveScript.MoveBeam(moveBoxX, moveBoxZ);
        this.LocatedAllRotatableBeams = false;
        this.AllRotatableBeamsPositions = new List<Vector3>();
        this.AllPartsOfRotatableBeamsPositions = new List<Vector3>();

        return canMoveToPreviousObsticleLocation;
    }
}
