﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class GUILogic : MonoBehaviour
{

    private GameObject TitelAndStartButtons;        //The "page" when game loads

    private GameObject LevelSelection;        //The "page" where the levels are listed


    // Use this for initialization
    void Start()
    {
        TitelAndStartButtons = GameObject.Find("TitleAndStartButtons");

        LevelSelection = GameObject.Find("LevelSelection");

    }


    void Update()
    {



    }

    void OnDestroy()
    {

    }




    public void SelectLevelPage()
    {
        LevelSelection.transform.SetAsLastSibling();
    }

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




    /*
    private void ShowHUD()
    {
        for (int i = 0; i < HUD.transform.childCount; i++)
        {
            HUD.transform.GetChild(i).gameObject.SetActive(true);
        }
    }
    */
    



}
