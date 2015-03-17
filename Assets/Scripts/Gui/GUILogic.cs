using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class GUILogic : MonoBehaviour
{

    private GameObject HUD;

    // Use this for initialization
    void Start()
    {
        Time.timeScale = 1;



        // HUD = GameObject.Find("_HUD");

    }


    void Update()
    {



    }

    void OnDestroy()
    {

    }











    //IN SCENE (After scene selection)

    public void StartLevel()
    {
        GameObject startMenu = GameObject.Find("LevelStart");

        startMenu.SetActive(false);

        Time.timeScale = 1;
    }


    private void ShowHUD()
    {
        for (int i = 0; i < HUD.transform.childCount; i++)
        {
            HUD.transform.GetChild(i).gameObject.SetActive(true);
        }
    }

    



}
