using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Regular monster move, walls etc apply
public class BoxMove : MonoBehaviour
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

    
    //candles
    private GameObject AllTheCandles;               //Hierarchy-list objektet som har alla ljus som child
    public List<Vector3> AllCandlePositions = new List<Vector3>(); //Listan som håller alla coordinater på ljus


    public void MoveBox(float moveX, float moveY)
    {
        //if (!isMoving)
        //{

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
            











            //float x = 0;
            //float y = 0;
            //Debug.Log(x);
            //Debug.Log(y);

            //int randomHorizontalOrVertical = Random.Range(1, 3); //om det ska vara 1 steg vertikalt eller horisontiellt (1=horzontiellt (x), 2=vertikalt(y))
            //int randomDirection = Random.Range(1, 3); // om X eller Y ska vara -1 eller 1 (1=1, 2=-1)

            //if (randomHorizontalOrVertical == 1) //om det ska vara x
            //{
            //    if (randomDirection == 1)    //om det ska vara +1
            //    {
            //        x = 1;
            //    }
            //    else
            //    {
            //        x = -1;
            //    }
            //}
            //else
            //{
            //    if (randomDirection == 1)    //om det ska vara +1
            //    {
            //        y = 1;
            //    }
            //    else
            //    {
            //        y = -1;
            //    }
            //}



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

            moveX = 0;
            moveY = 0;
       // }
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
  //      AllCandlePositions = new List<Vector3>();


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

        
        //Kontrollerar sedan om vart alla ljus finns på banan (Detta gör jag efter för att minska onödigt loopande)
        AllTheCandles = GameObject.Find("AllTheCandles");

        int NumberOfCandles = AllTheCandles.transform.childCount;

        for (int i = 0; i < NumberOfCandles; i++)
        {
            Transform candle = AllTheCandles.transform.GetChild(i);
            AllCandlePositions.Add(candle.position);
        }


        if (!AllBoxPositions.Contains(endPosition) && !AllWallPositions.Contains(endPosition) && !AllCandlePositions.Contains(endPosition))
        {
            
        while (t < 1f)
        {
            

            //Kontrollerar om det finns någon vägg eller box ivägen, och om inte, så genomförs förflyttningen mot den rutan
            
                /*
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
                */
                t += Time.deltaTime * (moveSpeed / gridSize) * factor;

                transform.position = Vector3.Lerp(startPosition, endPosition, t);
                yield return null;
            }

            startPosition = new Vector3();          //Nollställer startposition

        }

        else{
                t = 1f;
                yield return null;
            }

        isMoving = false;
        yield return 0;
    }
}