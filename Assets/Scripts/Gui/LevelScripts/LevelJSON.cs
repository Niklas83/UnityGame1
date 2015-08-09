using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LevelJSON  {

    public int SceneNr;                             //Number of the scene from file->buildsetting (the menu in unity)

    public string Name;                                                     //(Denna måste sparas)
    public bool IsPassed;                                                  //(Denna måste sparas)
    public bool IsActive;

    public int WorldPageNumber;                  //what world "page" will this list in

    public List<string> InformationList;    //Used in the info box when level is clicked

    public bool IsBonusLevel;               //Not sure if this is needed

    public int StarValue;                   //How many stars is the current level worth (1-3)

    //public bool Star1;
    //public bool Star2;
    //public bool Star3;

    //public int MaxStepsFor1Star;                                            //(Denna måste spars)
    //public int MaxStepsFor2Star;                                            //(Denna måste spars)
    //public int MaxStepsFor3Star;                                            //(Denna måste spars)

    //public int NumberOfSteps;                                                 //(Denna måste sparas)

    //public int NumberOfExits;                                                   //(Denna måste spars)
    //public int NumberOfExitsCleared;        //Om man t.ex. klarat 1 av 2        

    //public List<int> CoordinatesOfCleardExits;  //ska vara vector3 inte int         (Denna måste sparas)

    //public int NumberOfObjectives;


}
