using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGridMove : MonoBehaviour
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

    private ObjectLocator objectLocator;            //Har koll på alla objekt i spelet, (GÖR TILL SINGLETON)
    
    private bool canMoveToPreviousObsticleLocation = true; //Sätts till false om ett obsictle returnerar att den inte kan gå till en viss ruta som spelaren önskat

    public void Start()
    {
        objectLocator = GameObject.FindObjectOfType<ObjectLocator>();         //Object som håller alla positioner på object
    }

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


        //Kontrollerar om det finns någon vägg eller box ivägen, och om inte, så genomförs förflyttningsförsöket mot den rutan
        if (!objectLocator.GetAllNonMovableBoxPositions().Contains(endPosition) && !objectLocator.GetAllWallPositions().Contains(endPosition))
        {
            while (t < 1f)
            {
                //Kontrollerar om det finns ljus på rutan 
                if (objectLocator.GetAllCandlePositions().Contains(endPosition) && candleHasBeenRemoved == false)
                {
                    candleHasBeenRemoved = objectLocator.RemoveCandleOnTileWalkingTo(endPosition);
                }

                //Kontrollerar om det finns en movable box på rutan
                if (objectLocator.GetAllMovableBoxPositions().Contains(endPosition))
                {
                    Transform MovingBox = objectLocator.GetBoxToPushFromTileWalkingTo(endPosition);

                    float moveBoxX = 0;
                    float moveBoxZ = 0;

                    if (input.x != 0)
                    {
                        if (input.x > 0){moveBoxX = 1;}
                        else{moveBoxX = -1;}
                    }
                    else
                    {
                        if (input.y > 0){moveBoxZ = 1;}
                        else{moveBoxZ = -1;}
                    }

                   this.canMoveToPreviousObsticleLocation = objectLocator.MoveMovableBox(MovingBox, moveBoxX, moveBoxZ);
                }

                t += Time.deltaTime * (moveSpeed / gridSize) * factor;

                if (this.canMoveToPreviousObsticleLocation == true)
                {
                    transform.position = Vector3.Lerp(startPosition, endPosition, t);
                }
                yield return null;
            }
        }
        else
        {
            t = 1f;
            yield return null;
        }


        objectLocator.UpdateAllPositions();
        isMoving = false;
        canMoveToPreviousObsticleLocation = true;   //återställer så att spelaren kan gå igen, ifall en obsticle satt den false
        yield return 0;
    }
}