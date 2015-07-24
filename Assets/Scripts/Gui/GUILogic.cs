using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEditor;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class GUILogic : MonoBehaviour
{
    //Just used from some maps atm
    private GameObject TitelAndStartButtons;        //The "page" when game loads            
    private GameObject LevelSelection;        //The "page" where the levels are listed

    private enum ViewEnums
    {
        MenuWithTitle,
        Credits,
        Options,
        Levels
    }

    private GameObject[] _listOfViews;
    private int _previousView = 0;
    private int _currentView = 0;

    // Use this for initialization
    void Start()
    {
        _listOfViews = new GameObject[20];       //Höftat värde

        GameObject parent = GameObject.Find("Background");

        //Hittar även inaktiva med denna
        _listOfViews[(int)ViewEnums.MenuWithTitle] = MiscHelperMethods.FindObject(parent, "StartPage_TitleMenuWebLink");
        _listOfViews[(int)ViewEnums.Credits] = MiscHelperMethods.FindObject(parent, "Credits");
        _listOfViews[(int)ViewEnums.Levels] = MiscHelperMethods.FindObject(parent, "LevelsPage");
        //Sätter alla vyer inactive förutom title
        //for (int i = 1; i < _listOfViews.Count(); i++)
        //{
        //    if (_listOfViews[i] != null)
        //    _listOfViews[i].SetActive(false);
        //}



        //OLD, TO BE REMOVED
        TitelAndStartButtons = GameObject.Find("TitleAndStartButtons");
        //OLD, TO BE REMOVED
        LevelSelection = GameObject.Find("LevelSelection");
    }




    public void QuitGame()
    {
        if (Application.isEditor)       //Om i editor
        {
            EditorApplication.isPlaying = false;
        }
        else                        //Om build      //TODO Kan måsta lägga till lite mer finmaskig quit för mobile
        {
            Application.Quit();
        } 
    }


    public void Credits()
    {
        _previousView = (int) ViewEnums.MenuWithTitle;
        _currentView = (int) ViewEnums.Credits;
        _listOfViews[(int) ViewEnums.MenuWithTitle].SetActive(false);
        _listOfViews[(int)ViewEnums.Credits].SetActive(true);
    }

    public void LevelsPage()
    {
        _previousView = (int)ViewEnums.MenuWithTitle;
        _currentView = (int)ViewEnums.Levels;
        _listOfViews[(int)ViewEnums.MenuWithTitle].SetActive(false);
        _listOfViews[(int)ViewEnums.Levels].SetActive(true);
    }


    public void BackOneStep()
    {
        _listOfViews[_currentView].SetActive(false);
        _listOfViews[_previousView].SetActive(true);
    }


    public void OpenWebsite(string url)
    {
        Application.OpenURL(url);
    }




    


















    //OLD, TO BE REMOVED
    public void SelectLevelPage()
    {
        LevelSelection.transform.SetAsLastSibling();
    }
    //OLD, TO BE REMOVED
    public void SelectStartPage()
    {
        TitelAndStartButtons.transform.SetAsLastSibling();
    }








    //IN SCENE (After scene selection)

    public void StartLevel()
    {
        GameObject startMenu = GameObject.Find("LevelStart");

        startMenu.SetActive(false);

        Time.timeScale = 1;
    }




 


}
