using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class Level : MonoBehaviour {

    public int SceneNr;                    //TODO kan nog tabort denna         //Number of the scene from file->buildsetting (the menu in unity)

    public string Name;                                                     //(Denna måste sparas)
    public bool HasPassed;                                                  //(Denna måste sparas)
    public bool IsActive;
    public bool Star1;
    public bool Star2;
    public bool Star3;

    public int MaxStepsFor1Star;                                            //(Denna måste spars)
    public int MaxStepsFor2Star;                                            //(Denna måste spars)
    public int MaxStepsFor3Star;                                            //(Denna måste spars)

    public int NumberOfSteps;                                                 //(Denna måste sparas)

    public int NumberOfExits;                                                   //(Denna måste spars)
    public int NumberOfExitsCleared;        //Om man t.ex. klarat 1 av 2        

    public List<int> CoordinatesOfCleardExits;  //ska vara vector3 inte int         (Denna måste sparas)

    //public int NumberOfObjectives;

    public List<string> ObjectiveList;


    //public int LevelID;             //Samma som antalet som finns i levellistan då kartan addas, funkar bara om ingen karta blir bortplockad, får lägga till detta om vi implementerar en DB, typ sqlite

    // Use this for initialization
    void Start()
    {
        //Set values
        SetLevelNameInUI();
    }

    // Update is called once per frame
    void Update()
    {

    }


    private void SetLevelNameInUI()
    {
        //GetComponentInChildren<Text>().text = Name; //OLD
        GetComponentInChildren<Text>().text = SceneNr.ToString();
    }



    //OLD TO BE REMOVED
    //used when LOADING to set script to use on creation
    public void SetLevelToLoad()
    {
        Button button = GetComponentInChildren<Button>();

        button.onClick.AddListener(StartLevel);
    }



    public void StartLevel()
    {
        Debug.Log("LoggarInneIKnappForLevel: " + Name);       
        if (IsActive)
        {
            SceneTransition st = Helper.Find<SceneTransition>("SceneTransition");

            if (SceneNr != 0)
            {
                st.NextSceneString(Name);
            }

            //TODO: Add the avalility in SceneTransition that you can load a map from the scene name aswell
            //else if (SceneName != null && SceneName != "")
            //{
            //    st.NextScene(SceneName);
            //}
        }     
    }

    private void AddExitsToCleared(int coordinate)  //ska vara inte vara int utan vector3
    {
        bool ExitHasAlreadyBeenCleared = false;

        for (int i = 0; i < CoordinatesOfCleardExits.Count; i++)
        {
            if (CoordinatesOfCleardExits[i] == coordinate)
            {
                ExitHasAlreadyBeenCleared = true;
            }
        }

        if (!ExitHasAlreadyBeenCleared)
        {
            CoordinatesOfCleardExits.Add(coordinate);
        }

        setValues();
    }




    private void setValues()
    {
        NumberOfExitsCleared = CoordinatesOfCleardExits.Count;      //Sätter antalet exits som klarats till antalat som finns i coordinat listan 

        SetTheStars();

    }



    private void SetTheStars()
    {
        if (NumberOfSteps <= MaxStepsFor3Star)
        {
            Star1 = true;
            Star2 = true;
            Star3 = true;
        }
        else if (NumberOfSteps <= MaxStepsFor2Star)
        {
            Star1 = true;
            Star2 = true;
            Star3 = false;
        }
        else if (NumberOfSteps <= MaxStepsFor1Star)
        {
            Star1 = true;
            Star2 = false;
            Star3 = false;
        }
        else
        {
            Star1 = false;
            Star2 = false;
            Star3 = false;
        }
    }






/*
    public void SaveToJson()
    {
        LevelManager.Save();
    }
    */

}
