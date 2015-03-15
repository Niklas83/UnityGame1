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
        Time.timeScale = 0;



        // HUD = GameObject.Find("_HUD");

    }


    void Update()
    {



    }

    void OnDestroy()
    {

    }


    //Before scene selection (Start menu + level selection)

    public void LoadScene(int sceneNumber, bool sceneIsActive)
    {
        if (sceneIsActive == true)
        {
            SceneTransition st = Helper.Find<SceneTransition>("SceneTransition");

            if (sceneNumber != 0)
            {
                st.NextScene(sceneNumber);
            }

            //TODO: Add the avalility in SceneTransition that you can load a map from the scene name aswell
            //else if (SceneName != null && SceneName != "")
            //{
            //    st.NextScene(SceneName);
            //}
        }

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
