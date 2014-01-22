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

    //Har koll på alla objekt på banan
    private ObjectLocator objectLocator;            //Har koll på alla objekt i spelet, (GÖR TILL SINGLETON)
    

    //Variabel för hurvida en låda flyttat eller ej, vilket används för att sätta true eller false return
    Vector3 startLocationOfObsticle = new Vector3();


    public void Start()
    {
        objectLocator = GameObject.FindObjectOfType<ObjectLocator>();         //Object som håller alla positioner på object
    }

    public bool MoveBox(float moveX, float moveY)
    {
        if (startLocationOfObsticle.Equals(new Vector3()))
        {
            startLocationOfObsticle = transform.position;
        }

        //tar man bort dessa position/input grejer så försvinner "push effekten" på boxen, att den skjuts frammåt från spelaren (ett mellanrum) (18 rader ner)
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
        //Ovanstående kod gör att det blir en push effekt på boxen



        //Input som används
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


        //Kontrollerar om boxen har flyttats och returnerar true om så är fallet, i annat fall returneras false då obsticle ej kunnat flytta
        if (startLocationOfObsticle.Equals(transform.position))
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

        if (!objectLocator.GetAllMovableBoxPositions().Contains(endPosition) && !objectLocator.GetAllNonMovableBoxPositions().Contains(endPosition) && !objectLocator.GetAllWallPositions().Contains(endPosition) && !objectLocator.GetAllCandlePositions().Contains(endPosition) && !objectLocator.GetAllRotatableBeamsPositions().Contains(endPosition))
        {
            
            while (t < 1f)
            {
                t += Time.deltaTime * (moveSpeed / gridSize) * factor;

                transform.position = Vector3.Lerp(startPosition, endPosition, t);
                yield return null;
            }

            startPosition = new Vector3();          //Nollställer startposition
            startLocationOfObsticle = new Vector3();          //Nollställer startLocation

        }
        else
        {
            t = 1f;
            yield return null;
        }

        isMoving = false;
        yield return 0;
    }
}