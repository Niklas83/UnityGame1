using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class Level : MonoBehaviour {

    public int SceneNr;                             //Number of the scene from file->buildsetting (the menu in unity)

    public string Name;                                                     //(Denna måste sparas)
    public bool IsPassed;                                                  //(Denna måste sparas)
    public bool IsActive;

    public int WorldPageNumber;                  //what world "page" will this list in

    public List<string> InformationList;    //Used in the info box when level is clicked

    public bool IsBonusLevel;               //Not sure if this is needed

    public int StarValue;                   //How many stars is the current level worth (1-3)

    public string WorldName;

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
        if (GetComponentInChildren<Text>() != null)
        {
            GetComponentInChildren<Text>().text = SceneNr.ToString();
        }
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


 



   


/*
    public void SaveToJson()
    {
        LevelManager.Save();
    }
    */

}
