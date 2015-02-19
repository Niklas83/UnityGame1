
using System.Linq;
using UnityEditor;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RedBlueButtonTile : BaseTile {

    //This Durabiliy is compared to the units weight, if the weight > durability you fall through the floor
    public int CurrentDurability = 100;
    public override int Durability { get { return CurrentDurability; } }

    public GameObject redBox;           //One of the types to be lowered/raised

    public GameObject blueBox;          //One of the types to be lowered/raised

    public bool StartColorRed;

    private List<GameObject> redObjects;        //Gets all the objects of the redBox tag
    private List<GameObject> blueObjects;       //Gets all the objects of the blueBox tag

    private EventListener[] redObjectsToNotify;

    private EventListener[] blueObjectsToNotify;

    private bool isRed = false;
    //public EventMessage message;

    protected override void OnLeaved(BaseUnit unit, BaseTile nextTile) { }

    protected override void OnArrived(BaseUnit unit, BaseTile previousTile)
    {
        base.OnArrived(unit, previousTile);

        foreach (EventListener el in redObjectsToNotify)
        {
            el.ReceiveEvent(EventMessage.ToggleUpDown);
        }

        foreach (EventListener el in blueObjectsToNotify)
        {
            el.ReceiveEvent(EventMessage.ToggleUpDown);
        }

        if (isRed)
        {
            this.renderer.material.color = Color.blue;
            isRed = false;
        }
        else
        {
            this.renderer.material.color = Color.red;
            isRed = true;
        }
    }


    // Use this for initialization
    void Start()
    {
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
        
        //string asfdasdf = "test";


        if (StartColorRed == true)
        {
            this.renderer.material.color = Color.red;
            isRed = true;
            foreach (EventListener el in blueObjectsToNotify)
                el.ReceiveEvent(EventMessage.ToggleUpDown);
        }
        else
        {
            this.renderer.material.color = Color.blue;
            isRed = false;
            foreach (EventListener el in redObjectsToNotify)
                el.ReceiveEvent(EventMessage.ToggleUpDown);
        }



    }


}



