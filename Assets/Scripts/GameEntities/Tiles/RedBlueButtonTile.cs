
using System.Linq;

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RedBlueButtonTile : BaseTile {

    //This Durabiliy is compared to the units weight, if the weight > durability you fall through the floor
    public int CurrentDurability = 100;
    public override int Durability { get { return CurrentDurability; } }

    //Trainunits check if this is true, if so it may move on it
    public override bool TrainTile { get { return IsTrainTile; } }
    public bool IsTrainTile = false;

    //If this is true objects being pushed on this tile from another tile will keep sliding towards the direction it was pushed
    public override bool IceTile { get { return IsIceTile; } }
    public bool IsIceTile = false;

    public GameObject redBox;           //One of the types to be lowered/raised

    public GameObject blueBox;          //One of the types to be lowered/raised

    public bool StartColorRed;

    private List<GameObject> redObjects;        //Gets all the objects of the redBox tag
    private List<GameObject> blueObjects;       //Gets all the objects of the blueBox tag

    private EventListener[] redObjectsToNotify;
    private EventListener[] blueObjectsToNotify;

    private bool isRed = false;


    private List<GameObject> redBlueButtonObjects;        //Gets all the objects of the redBox tag
    private EventListener[] RedBlueButtonTilesToNotify;
    //public EventMessage message;

    protected override void OnLeaved(BaseUnit unit, BaseTile nextTile) { }

    protected override void OnArrived(BaseUnit unit, BaseTile previousTile)
    {
        base.OnArrived(unit, previousTile);

        if (redObjectsToNotify != null)
        {
            foreach (EventListener el in redObjectsToNotify)
            {
                el.ReceiveEvent(EventMessage.ToggleUpDown);
            }

            foreach (EventListener el in blueObjectsToNotify)
            {
                el.ReceiveEvent(EventMessage.ToggleUpDown);
            }

            foreach (EventListener el in RedBlueButtonTilesToNotify)
            {
                el.ReceiveEvent(EventMessage.ToggleColor);
            }
        }
        else
        {
            Debug.Log("redObjectsToNotify has not been set");
        }
    }


    // Use this for initialization
    void Start()
    {
        redBlueButtonObjects = GameObject.FindGameObjectsWithTag(this.tag).ToList();
        RedBlueButtonTilesToNotify = new EventListener[redBlueButtonObjects.Count];
        for (int i = 0; i < redBlueButtonObjects.Count; i++)
        {
            RedBlueButtonTilesToNotify[i] = redBlueButtonObjects[i].GetComponent<EventListener>();
        }



        redObjects = GameObject.FindGameObjectsWithTag(redBox.tag).ToList();
        blueObjects = GameObject.FindGameObjectsWithTag(blueBox.tag).ToList();
        
        redObjectsToNotify = new EventListener[redObjects.Count];
        blueObjectsToNotify = new EventListener[blueObjects.Count];

        for (int i = 0; i < redObjects.Count; i++)
        {
            redObjectsToNotify[i] = redObjects[i].GetComponent<EventListener>();
        }

        for (int i = 0; i < blueObjects.Count; i++)
        {
            blueObjectsToNotify[i] = blueObjects[i].GetComponent<EventListener>();
        }   

        RedAndBlueUnit redAndBlueUnit;
        if (StartColorRed == true)
        {
            
            this.GetComponent<Renderer>().material.color = Color.red;
            isRed = true;
            foreach (EventListener el in blueObjectsToNotify)
            {
                redAndBlueUnit = el.GetComponentInParent<RedAndBlueUnit>();
                if (redAndBlueUnit.hasBeenActivated == false)
                {
                    el.ReceiveEvent(EventMessage.ToggleUpDown);
                    redAndBlueUnit.hasBeenActivated = true;
                }
            }
        }
        else
        {
            this.GetComponent<Renderer>().material.color = Color.blue;
            isRed = false;
            foreach (EventListener el in redObjectsToNotify)
            {
                redAndBlueUnit = el.GetComponentInParent<RedAndBlueUnit>();
                if (redAndBlueUnit.hasBeenActivated == false)
                {
                    el.ReceiveEvent(EventMessage.ToggleUpDown);
                    redAndBlueUnit.hasBeenActivated = true;
                }
            }
        }



    }

    public void ToggleColor()
    {
        if (isRed)
        {
            this.GetComponent<Renderer>().material.color = Color.blue;
            isRed = false;
        }
        else
        {
            this.GetComponent<Renderer>().material.color = Color.red;
            isRed = true;
        }
    }

}



