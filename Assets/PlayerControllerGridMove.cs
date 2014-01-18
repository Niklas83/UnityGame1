using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//DENNA KLASS ANVÄNDS FÖR STUNDEN INTE DÅ JAG BYGGER AI PÅ PLAYER
public class PlayerControllerGridMove : MonoBehaviour
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
    private bool LocatedAllStaticBoxes = false;     //sätts till true då det gjorts en kontroll om vart alla statiska, ej flyttbara boxar är så den inte görs igen

    //walls
    private GameObject AllTheWalls;                 //Hierarchy-list objektet som har alla yttre väggboxar som childs
    private List<Vector3> AllWallPositions = new List<Vector3>();    //Listan som håller alla coordinater på yttre väggar
    private bool LocatedAllWalls = false;           //sätts till true då det gjorts en kontroll om vart alla väggar är så den inte görs igen

    //candles
    private GameObject AllTheCandles;               //Hierarchy-list objektet som har alla ljus som child
    public List<Vector3> AllCandlePositions = new List<Vector3>(); //Listan som håller alla coordinater på ljus
    private bool LocatedAllCandles = false;         //sätts till true då det gjorts en kontroll om vart alla ljus är så den inte görs igen


    //portal till nästa bana
    public Transform PortalPrefab;          //Portal objektet för GameObject (klassen som instansieras)
    public Transform Portal;            //Det instantierade portal-objektet som skapas då alla ljus är tagna

    private bool PortalPlaced = false;  //Variabel för hurvida portalen blivit placerad eller ej


    public void Update()
    {
        if (!isMoving)
        {
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
        }
    }

    public IEnumerator move(Transform transform)
    {

        bool candleHasBeenRemoved = false;      //För att inte ljuset ska försöka tas bort varje gång loopen ittereras
        isMoving = true;
        startPosition = transform.position;
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



        //Kollar om alla boxar tagits fram en gång
        if (this.LocatedAllStaticBoxes == false)
        {
            //Tar fram alla coordinater på boxar
            AllTheBoxes = GameObject.Find("AllTheBoxes");

            int NumberOfBoxChilds = AllTheBoxes.transform.childCount;

            for (int i = 0; i < NumberOfBoxChilds; i++)
            {
                Transform box = AllTheBoxes.transform.GetChild(i);
                AllBoxPositions.Add(box.position);
            }
            this.LocatedAllStaticBoxes = true;
        }

        //Kollar om alla väggar tagits fram en gång
        if (this.LocatedAllWalls == false)
        {
            //Tar fram alla coordinater på väggar
            AllTheWalls = GameObject.Find("AllTheWallSquares");

            int NumberOfWallChilds = AllTheWalls.transform.childCount;

            for (int i = 0; i < NumberOfWallChilds; i++)
            {
                Transform wall = AllTheWalls.transform.GetChild(i);
                AllWallPositions.Add(wall.position);
            }

            this.LocatedAllWalls = true;
        }

        //Kollar om alla ljus tagits fram en gång
        if (LocatedAllCandles == false)
        {
            AllTheCandles = GameObject.Find("AllTheCandles");

            int NumberOfCandles = AllTheCandles.transform.childCount;

            for (int i = 0; i < NumberOfCandles; i++)
            {
                Transform candle = AllTheCandles.transform.GetChild(i);
                AllCandlePositions.Add(candle.position);
            }

            LocatedAllCandles = true;
        }


        while (t < 1f)
        {
            

            //Kontrollerar om det finns någon vägg eller box ivägen, och om inte, så genomförs förflyttningen mot den rutan
            if (!AllBoxPositions.Contains(endPosition) && !AllWallPositions.Contains(endPosition))
            {
                //Kontrollerar om det finns ljus på rutan 
                if (AllCandlePositions.Contains(endPosition) && candleHasBeenRemoved == false)
                {
                    //Ljus objekt att ta bort
                    Transform candleToRemove = AllTheCandles.transform.GetChild(0); // Detta värde kommer alltid att sättas i foreach loopen nedan

                    //Hitta ljus objekt
                    for (int x = 0; x < AllTheCandles.transform.childCount; x++)
                    {
                        Transform candle = AllTheCandles.transform.GetChild(x);
                        if (candle.position == endPosition)
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
                    Destroy(candleToDestroy);                       //Tar bort det faktiska objektet
                    candleHasBeenRemoved = true;                    //Sätter att ljuset har tagits bort så scriptet inte försöker ta bort samma ljus in nästa loop


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
                }

                t += Time.deltaTime * (moveSpeed / gridSize) * factor;

                transform.position = Vector3.Lerp(startPosition, endPosition, t);
                yield return null;
            }

            else
            {
                t = 1f;
                yield return null;
            }

        }

        

        isMoving = false;
        yield return 0;
    }
}