using System.Reflection;
using UnityEngine;
using System.Collections;

public class GameLogicScript : MonoBehaviour {

    //Gridden
    public Transform FloorSquarePrefab;        //FloorSquare objektet är för GameObject (klassen som instansieras)
    public Vector3 Size;                //Storlek som sätts i unity, hur många golvrutor x och z
    public Transform FloorSquares;      //Variabel för hierarchy item AllTheFloorSquares (har alla floor squares som childs)

    //Väggarna
    public Transform WallSquarePrefab;        //WallSquare objektet är för GameObject (klassen som instansieras)
    public Transform WallSquares;      //Variabel för hierarchy item AllTheWallSquares (har alla wall squares som childs)

    //Player
    public Transform PlayerPrefab;       //Spelar objektet (klassen som instansieras)
    public Transform Player;            //Player objektet, (Den som rör sig i spelet)
    public Transform ControllerPlayerPrefab;    //Player object that can move freely
    public bool useControllerPlatyerPrefab;
    

    //Boxar som inte går att flytta
    public Transform NonMovableBoxPrefab;       //(klassen som instansieras)
    private int NumberOfBoxes;
    public Transform Boxes;              //Variabel för hierarchy-item AllTheBoxes

    //Boxar som kan flyttas
    public Transform MovableBoxesPrefab;        //(klassen som instansieras)
    private int NumberOfMovableBoxes;
    public Transform MovableBoxes;           //Variabel för hierarchy-item AllTheBoxes

    //Candles/Points    (De objekt du tar för att klara banan)
    public Transform CandlePrefab;      //(Klassen som instansieras)
    private int numberOfCandles;     //Antalet ljus som finns på en bana
    public Transform Candles;       //Variabel för hierarchy-item AllTheCandles

    //RotatableBeams som går att rotera runt en axel
    public Transform RotatableBeamPrefab;   //(Klassen som instansieras)
    private int numberOfRotatableBeams;              //Antalet AllRotatableBeams som finns på en bana
    public Transform AllBeamsList;          //Variabel för hierarchy-item AllRotatableBeams


    public
	// Use this for initialization
	void Start ()
	{
	    CreateGrid();
        createPlayer();
        createBoxes();
        CreateWalls();
        createMovableBoxes();
        CreatCandles();
        CreatRotatableBeams();
	}

    //Skapar gridden (golvet)
    void CreateGrid()
    {
        for (int i = 0; i < Size.x; i++)
        {
            //Instantiate(CellPrefab, new Vector3(i,0,0), Quaternion.identity);
            for (int j = 0; j < Size.z; j++)
            {
                Transform newFloorSquare;
                newFloorSquare = (Transform)Instantiate(FloorSquarePrefab, new Vector3(i, 0, j), Quaternion.identity);
                newFloorSquare.name = "x" + i.ToString() + "z" + j.ToString();
                newFloorSquare.parent = FloorSquares.transform;
            }
        }
        
    }

    //Skapar de yttre väggarna (runt golvet)
    void CreateWalls()
    {
        Transform newWallSquare;
        //Jobbar HÄR

        //Nedre väggarna
        for (int i = 0; i < Size.x; i++)
        {
            newWallSquare = (Transform)Instantiate(WallSquarePrefab, new Vector3(i, 1, 0), Quaternion.identity);
            newWallSquare.name = "x" + i.ToString() + "z" + "0";
            newWallSquare.parent = WallSquares.transform;
        }

        //Vänstra väggarna
        for (int i = 0; i < Size.x-1; i++)
        {
            newWallSquare = (Transform)Instantiate(WallSquarePrefab, new Vector3(0, 1, i+1), Quaternion.identity);
            newWallSquare.name = "x" + "0" + "z" + (i + 1).ToString();
            newWallSquare.parent = WallSquares.transform;
        }

        //Högra väggarna
        for (int i = 0; i < Size.x - 1; i++)
        {
            newWallSquare = (Transform)Instantiate(WallSquarePrefab, new Vector3(Size.x-1, 1, i+1), Quaternion.identity);
            newWallSquare.name = "x" + (Size.x - 1).ToString() + "z" + (i + 1).ToString();
            newWallSquare.parent = WallSquares.transform;
        }

        //Övre väggarna
        for (int i = 0; i < Size.x - 2; i++)
        {
            newWallSquare = (Transform)Instantiate(WallSquarePrefab, new Vector3(i + 1, 1, Size.z-1), Quaternion.identity);
            newWallSquare.name = "x" + (i + 1).ToString() + "z" + (Size.z - 1).ToString();
            newWallSquare.parent = WallSquares.transform;
        }
    }

    //Skapar player objektet
    void createPlayer()
    {
        Transform newPlayer;
        if (useControllerPlatyerPrefab == false)
        {
            newPlayer = (Transform) Instantiate(PlayerPrefab, new Vector3(8, 1, 8), Quaternion.identity);
        }
        else
        {
            newPlayer = (Transform)Instantiate(ControllerPlayerPrefab, new Vector3(8, 10, 8), Quaternion.identity);
        }
        newPlayer.name = "ThePlayer";
        newPlayer.parent = transform;
    }

    //Skapar boxar som ej kan flyttas
    void createBoxes()
    {
        this.NumberOfBoxes = Random.Range(1, 3);

        int X = 0;
        int Z = 0;

        for (int i = 0; i < NumberOfBoxes; i++)
        {
            X = Random.Range(2, (int)Size.x -1);
            Z = Random.Range(2, (int)Size.z -1);

            Transform newBox;
            newBox = (Transform)Instantiate(NonMovableBoxPrefab, new Vector3(X, 1, Z), Quaternion.identity);
            newBox.name = "x" + X.ToString() + "z" + Z.ToString();
            newBox.parent = Boxes.transform;
        }
        
    }

    //Skapar boxar som KAN lyttas
    void createMovableBoxes()
    {
        this.NumberOfMovableBoxes = Random.Range(1, 3);

        int X = 0;
        int Z = 0;

        for (int i = 0; i < NumberOfMovableBoxes; i++)
        {
            X = Random.Range(2, (int)Size.x - 1);
            Z = Random.Range(2, (int)Size.z - 1);

            Transform newBox;
            newBox = (Transform)Instantiate(MovableBoxesPrefab, new Vector3(X, 1, Z), Quaternion.identity);
            newBox.name = "MovableBox_x" + X.ToString() + "z" + Z.ToString();
            newBox.parent = MovableBoxes.transform;
        }

    }

    //Skapar poäng ljus
    void CreatCandles()
    {
        this.numberOfCandles = 5;

        int X = 0;
        int Z = 0;

        for (int i = 0; i < numberOfCandles; i++)
        {
            X = Random.Range(2, (int)Size.x - 1);
            Z = Random.Range(2, (int)Size.z - 1);

            Transform newCandle;
            newCandle = (Transform)Instantiate(CandlePrefab, new Vector3(X, 1, Z), Quaternion.identity);
            newCandle.name = "candle:" + "x" + X.ToString() + "z" + Z.ToString();
            newCandle.parent = Candles.transform;
        }

    }

    //Skapar RotatableBeams
    void CreatRotatableBeams()
    {
        this.numberOfRotatableBeams = 1;

        int X = 0;
        int Z = 0;

        for (int i = 0; i < numberOfRotatableBeams; i++)
        {
            X = Random.Range(2, (int)Size.x - 1);
            Z = Random.Range(2, (int)Size.z - 1);

            Transform newRotatable;
            newRotatable = (Transform)Instantiate(RotatableBeamPrefab, new Vector3(X, 1, Z), Quaternion.identity);
            newRotatable.name = "rotatableBeam:" + "x" + X.ToString() + "z" + Z.ToString();
            newRotatable.parent = AllBeamsList.transform;
        }

    }





    //Lite random testgrejer nedan

    //void SetFloorType()     //Roterar alla golv-squares
    //{
    //    foreach (Transform floor in FloorSquares.transform)
    //    {
    //       floor.Rotate(new Vector3(45,45,45));
    //    }
    //}

    // Update is called once per frame
    //void Update()
    //{
    //    //test grejer bara
    //    if (Input.GetKeyDown(KeyCode.T))
    //    {
    //        SetFloorType();
    //    }

    //    if (Input.GetKeyDown(KeyCode.K))
    //    {
    //       Transform player = transform.GetChild(0);
    //       player.Rotate(new Vector3(45, 45, 45));
    //    }

    //}
}
