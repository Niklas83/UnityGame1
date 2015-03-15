using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LevelJSON  {

    public int SceneNr;                             //Number of the scene from file->buildsetting (the menu in unity)

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
}
