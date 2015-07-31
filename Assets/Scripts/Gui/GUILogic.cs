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
        Levels,
        LevelDialog
    }

    private GameObject[] _listOfViews;
    private int _previousView = 0;
    private int _currentView = 0;

    private Level _selectedLevelDialog;


    // Use this for initialization
    void Start()
    {
        if(_listOfViews == null)
        {
            InitInternalLists();
        }
        
    }

    //  When commin back from a level to the start scene we wanna show the level selection page.
    private void OnLevelWasLoaded()
    {
        if (_listOfViews == null)
        {
            InitInternalLists();
            LevelsPage();
        }
    }

    private void InitInternalLists()
    {
        _listOfViews = new GameObject[20];       //Höftat värde

        GameObject parent = GameObject.Find("Background");

        //Hittar även inaktiva med denna
        _listOfViews[(int)ViewEnums.MenuWithTitle] = MiscHelperMethods.FindObject(parent, "StartPage_TitleMenuWebLink");
        _listOfViews[(int)ViewEnums.Credits] = MiscHelperMethods.FindObject(parent, "Credits");
        _listOfViews[(int)ViewEnums.Levels] = MiscHelperMethods.FindObject(parent, "LevelsPage");

        _listOfViews[(int)ViewEnums.LevelDialog] = MiscHelperMethods.FindObject(_listOfViews[(int)ViewEnums.Levels], "SelectedLevelDialog");

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


    public void OpenLevelDialog(Level level)
    {
        if (level.IsActive && level.GetComponent<Toggle>().isOn)
        {
            _listOfViews[(int) ViewEnums.LevelDialog].SetActive(true);

            GameObject child =
                _listOfViews[(int) ViewEnums.LevelDialog].transform.FindChild("ResponsiveWindow(LevelDialog)")
                    .gameObject;
            GameObject banner = child.transform.FindChild("Header").gameObject;
            GameObject textObj = banner.transform.FindChild("Text").gameObject;
            textObj.GetComponent<Text>().text = level.Name;

            //To make level loadable
            _selectedLevelDialog = level;
        }
    }

    public void CloseLevelDialog()
    {
        _listOfViews[(int)ViewEnums.LevelDialog].SetActive(false);
    }

    public void PlaySelectedLevelInDialog()
    {
        _selectedLevelDialog.StartLevel();
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
