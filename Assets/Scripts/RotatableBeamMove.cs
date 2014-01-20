using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Regular monster move, walls etc apply
public class RotatableBeamMove : MonoBehaviour
{
    private float moveSpeed = 3f;
    private float gridSize = 1f;
    private enum Orientation
    {
        Horizontal,
        Vertical
    };
    private Orientation gridOrientation = Orientation.Horizontal;
    private bool allowDiagonals = false;
    private bool correctDiagonalSpeed = true;
    private Vector2 input;
    private bool isMoving = false;
    private Vector3 startPosition;
    public Vector3 endPosition;
    private float t;
    private float factor;


    
    //boxes
    private GameObject AllTheBoxes;                 //Hierarchy-list objektet som har alla boxar som childs
    private List<Vector3> AllBoxPositions = new List<Vector3>();    //Listan som håller alla coordinater på boxar som ej kan flyttas

    //walls
    private GameObject AllTheWalls;                 //Hierarchy-list objektet som har alla yttre väggboxar som childs
    private List<Vector3> AllWallPositions = new List<Vector3>();    //Listan som håller alla coordinater på yttre väggar

    //MovableBoxs
    public GameObject AllMovableBoxes;                 //Hierarchy-list objektet som har alla boxar som childs
    public List<Vector3> AllMovableBoxPositions = new List<Vector3>();    //Listan som håller alla coordinater på boxar som KAN flyttas
    public bool LocatedAllMovableBoxes = false;     //sätts till true då det gjorts en kontroll om vart alla flyttbara boxar är, (denna kontroll ska göras då en flyttbar box flyttats)
    
    //candles
    private GameObject AllTheCandles;               //Hierarchy-list objektet som har alla ljus som child
    public List<Vector3> AllCandlePositions = new List<Vector3>(); //Listan som håller alla coordinater på ljus


    //Variabel för hurvida en låda flyttat eller ej, vilket används för att sätta true eller false return
    Vector3 startLocation = new Vector3();

    public bool MoveBox(float moveX, float moveY)
    {
        if (startLocation.Equals(new Vector3()))
        {
            startLocation = transform.position;
        }

        //tar man bort dessa position grejer så försvinner "push effekten" på boxen, att den skjuts frammåt från spelaren (ett mellanrum)
        input = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
                if (!allowDiagonals)
                {
                    if (Mathf.Abs(input.x) > Mathf.Abs(input.y))
                    {
                        input.y = 0;
                    }
                    else
                    {
                        input.x = 0;
                    }
                }

                if (input != Vector2.zero)
                {
                    StartCoroutine(move(transform));
                }

                input = new Vector2(moveX, moveY);

            if (!allowDiagonals)
            {
                if (Mathf.Abs(input.x) > Mathf.Abs(input.y))
                {
                    input.y = 0;
                }
                else
                {
                    input.x = 0;
                }
            }

            if (input != Vector2.zero)
            {
                StartCoroutine(move(transform));
            }


        if (startLocation.Equals(transform.position))
        {
            return false;
        }
        else
        {
            return true;
        }
       
    }

    public IEnumerator move(Transform transform)
    {
        isMoving = true;
        if (startPosition.Equals(new Vector3()))
        {
            startPosition = transform.position;
        }
        t = 0;

        if (gridOrientation == Orientation.Horizontal)
        {
            endPosition = new Vector3(startPosition.x + System.Math.Sign(input.x) * gridSize,
                startPosition.y, startPosition.z + System.Math.Sign(input.y) * gridSize);
        }
        else
        {
            endPosition = new Vector3(startPosition.x + System.Math.Sign(input.x) * gridSize,
                startPosition.y + System.Math.Sign(input.y) * gridSize, startPosition.z);
        }

        if (allowDiagonals && correctDiagonalSpeed && input.x != 0 && input.y != 0)
        {
            factor = 0.7071f;
        }
        else
        {
            factor = 1f;
        }

        //Sätt boolska värden på alla rutor i gridden som sedan uppdateras, då det flyttas något dit? (Tror inte det)


        //Nollställer alla positioner           
        AllBoxPositions = new List<Vector3>();
        AllWallPositions = new List<Vector3>();
        AllCandlePositions = new List<Vector3>();

        //Tar fram alla coordinater på boxar
        AllTheBoxes = GameObject.Find("AllTheBoxes");

        int NumberOfBoxChilds = AllTheBoxes.transform.childCount;

        for (int i = 0; i < NumberOfBoxChilds; i++)
        {
            Transform box = AllTheBoxes.transform.GetChild(i);
            AllBoxPositions.Add(box.position);
        }

        //Tar fram alla coordinater på väggar
        AllTheWalls = GameObject.Find("AllTheWallSquares");

        int NumberOfWallChilds = AllTheWalls.transform.childCount;

        for (int i = 0; i < NumberOfWallChilds; i++)
        {
            Transform wall = AllTheWalls.transform.GetChild(i);
            AllWallPositions.Add(wall.position);
        }


            //Tar fram alla coordinater på boxar
            this.AllMovableBoxes = GameObject.Find("AllMovableBoxes");

            int NumberOfMovableBoxChilds = AllMovableBoxes.transform.childCount;

            for (int i = 0; i < NumberOfMovableBoxChilds; i++)
            {
                Transform box = AllMovableBoxes.transform.GetChild(i);
                AllMovableBoxPositions.Add(box.position);
            }

       
        //Kontrollerar sedan om vart alla ljus finns på banan (Detta gör jag efter för att minska onödigt loopande)
        AllTheCandles = GameObject.Find("AllTheCandles");

        int NumberOfCandles = AllTheCandles.transform.childCount;

        for (int i = 0; i < NumberOfCandles; i++)
        {
            Transform candle = AllTheCandles.transform.GetChild(i);
            AllCandlePositions.Add(candle.position);
        }


        if (!AllBoxPositions.Contains(endPosition) && !AllWallPositions.Contains(endPosition) && !AllCandlePositions.Contains(endPosition) && !AllMovableBoxPositions.Contains(endPosition)) 
        {
            
        while (t < 1f)
        {
           t += Time.deltaTime * (moveSpeed / gridSize) * factor;

                transform.position = Vector3.Lerp(startPosition, endPosition, t);
                yield return null;
            }

            startPosition = new Vector3();          //Nollställer startposition
            startLocation = new Vector3();          //Nollställer startLocation
            AllMovableBoxPositions = new List<Vector3>();    //Nollställer AllMovableBoxPositions så att dessa inte stackar upp med de gamla coordinaterna kvar
            AllCandlePositions = new List<Vector3>();    //Nollställer AllCandlePositions så att dessa inte stackar upp med de gamla coordinaterna kvar


        }

        else{
                t = 1f;
                yield return null;
            }

        isMoving = false;
        yield return 0;
    }
}