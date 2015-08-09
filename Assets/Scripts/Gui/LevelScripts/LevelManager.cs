using System.IO;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using Newtonsoft.Json;
using Assets.DustinHorne.JsonDotNetUnity.TestCases;
using Assets.DustinHorne.JsonDotNetUnity.TestCases.TestModels;
using UnityEngine.UI;


public class LevelManager : MonoBehaviour {

    public List<GameObject> ListOfAllLevels;
    private List<Level> ListOfAllLevelScriptInstances;
    public List<LevelJSON> ListOfAllLevelJSON;

    private bool JsonCopyHasValues;

    public GameObject levelGameObject;      //being instantiated on load from json
    public Sprite LockedGUIimage;

    //TEMP fix to keep the levels logic and not having them all over the screen
    public bool LoadLevelsFromJSON = false;

    // Use this for initialization
    void Start()
    {
  
        //SetAllLevelScriptInstances();
        //SetAllJSONLevelInstances();
        //SaveToJson();

        if (LoadLevelsFromJSON)
        {
            LoadFromJson();
            LoadAllLevelScriptInstances();
            NEWLoadAllLevelGameObjects();
            //LoadAllLevelGameObjects();
        } 
    }

    ////  When commin back from a level to the start scene we wanna show the level selection page.
    //void OnLevelWasLoaded()
    //{
    //    //GameObject levelSelection = GameObject.Find("LevelSelection");
    //    //levelSelection.transform.SetAsLastSibling();
    //    _guiLogic = transform.GetComponent<GUILogic>();
    //    _guiLogic.LevelsPage();
    //}


    //used when saving
    private void SetAllLevelScriptInstances()
    {
        ListOfAllLevelScriptInstances = new List<Level>();
        Level levelToAdd;

        for (int i = 0; i < ListOfAllLevels.Count; i++)
        {
            levelToAdd = ListOfAllLevels[i].GetComponent<Level>();

            ListOfAllLevelScriptInstances.Add(levelToAdd);
        }
    }

    //used when saving 
    private void SetAllJSONLevelInstances()
    {
        ListOfAllLevelJSON = new List<LevelJSON>();
        
        for (int i = 0; i < ListOfAllLevelScriptInstances.Count; i++)
        {
            LevelJSON levelJSONToAdd = new LevelJSON();

            levelJSONToAdd.SceneNr = ListOfAllLevelScriptInstances[i].SceneNr;
            levelJSONToAdd.Name = ListOfAllLevelScriptInstances[i].Name;
            levelJSONToAdd.IsBonusLevel = ListOfAllLevelScriptInstances[i].IsBonusLevel;
            levelJSONToAdd.IsActive = ListOfAllLevelScriptInstances[i].IsActive;
            levelJSONToAdd.IsPassed = ListOfAllLevelScriptInstances[i].IsPassed;
            levelJSONToAdd.InformationList = ListOfAllLevelScriptInstances[i].InformationList;
            levelJSONToAdd.StarValue = ListOfAllLevelScriptInstances[i].StarValue;
            levelJSONToAdd.WorldPageNumber = ListOfAllLevelScriptInstances[i].WorldPageNumber;

            ListOfAllLevelJSON.Add(levelJSONToAdd);
        }
    }


    //used when loading
    private void LoadAllLevelScriptInstances()
    {
        ListOfAllLevelScriptInstances = new List<Level>();
        
        

        for (int i = 0; i < ListOfAllLevelJSON.Count; i++)
        {
            Level levelScriptToAdd = new Level();

            levelScriptToAdd.SceneNr = ListOfAllLevelJSON[i].SceneNr;
            levelScriptToAdd.Name = ListOfAllLevelJSON[i].Name;
            levelScriptToAdd.IsActive = ListOfAllLevelJSON[i].IsActive;
            levelScriptToAdd.IsPassed = ListOfAllLevelJSON[i].IsPassed;
            levelScriptToAdd.InformationList = ListOfAllLevelJSON[i].InformationList;
            levelScriptToAdd.IsBonusLevel = ListOfAllLevelJSON[i].IsBonusLevel;
            levelScriptToAdd.StarValue = ListOfAllLevelJSON[i].StarValue;
            levelScriptToAdd.WorldPageNumber = ListOfAllLevelJSON[i].WorldPageNumber;


            ListOfAllLevelScriptInstances.Add(levelScriptToAdd);    
        }
    }


    private void NEWLoadAllLevelGameObjects()
    {
        //This iteration is needed to access the levelgrid cuz the main parent object is inactive
        GameObject parent = GameObject.Find("Background");
        GameObject LevelPage = MiscHelperMethods.FindObject(parent,"LevelsPage");
        GameObject WindowsLevels = MiscHelperMethods.FindObject(LevelPage,"WindowLevels");
        GameObject Content = MiscHelperMethods.FindObject(WindowsLevels,"Content");
        GameObject ScrollView = MiscHelperMethods.FindObject(Content,"ScrollView");
        GameObject LevelsGrid = MiscHelperMethods.FindObject(ScrollView,"LevelsGrid");

        //To add event listener
        GUILogic _guiLogic = transform.GetComponent<GUILogic>();
       

        bool previousMapWasPassed = false;      //If the map handled before the current had been passed (in the loop) then set the next as 
        for (int i = 0; i < ListOfAllLevelScriptInstances.Count; i++)
        {
            if (ListOfAllLevelScriptInstances[i].WorldPageNumber == 1)      //Laddar alla värld 1 till gridden på start up
            {
            GameObject newLevelToGUI = Instantiate(levelGameObject);
            Level newLevelToGUIScript = newLevelToGUI.GetComponent<Level>();


            newLevelToGUIScript.SceneNr = ListOfAllLevelScriptInstances[i].SceneNr;
            newLevelToGUIScript.Name = ListOfAllLevelScriptInstances[i].Name;
            newLevelToGUIScript.IsPassed = ListOfAllLevelScriptInstances[i].IsPassed;
            //if (i > 0)
            //{
            //    newLevelToGUIScript.IsActive = previousMapWasPassed;
            //}
            //else
            //{
            newLevelToGUIScript.IsActive = ListOfAllLevelScriptInstances[i].IsActive;
            //}
            newLevelToGUIScript.InformationList = ListOfAllLevelScriptInstances[i].InformationList;
            newLevelToGUIScript.IsBonusLevel = ListOfAllLevelScriptInstances[i].IsBonusLevel;
            newLevelToGUIScript.StarValue = ListOfAllLevelScriptInstances[i].StarValue;
            newLevelToGUIScript.WorldPageNumber = ListOfAllLevelScriptInstances[i].WorldPageNumber;

            newLevelToGUI.name = "LevelObject_" + i;

            //previousMapWasPassed = ListOfAllLevelScriptInstances[i].IsPassed;

            //Sätter parent till panelen som håller listan (Just nu kör jag LevelSelectionBackgroundTEST)
            
                newLevelToGUI.transform.SetParent(LevelsGrid.transform, false);

                //Attaches the level to the grid
                Toggle newLevelToggle = newLevelToGUI.GetComponent<Toggle>();
                newLevelToggle.group = LevelsGrid.GetComponent<ToggleGroup>();

                //Adds a method to the toggle in the gui
                newLevelToggle.onValueChanged.AddListener(
                    delegate
                    {
                        _guiLogic.OpenLevelDialog(newLevelToGUIScript);
                    }
                    );

                //Sets lock icon on none unlocked levels
                SetLockedIcons(newLevelToGUI, newLevelToGUIScript.IsActive);
            }    
        }
    }







    //private void SetLevelColor(GameObject LevelObject, bool isActive)
    //{
    //   Image[] buttonImage = LevelObject.GetComponentsInChildren<Image>();

    //   foreach (var imag in buttonImage)
    //   {
    //       if (imag.name == "LevelButton")
    //       {
    //           if (!isActive)
    //           {
    //               imag.color = new Color(255f, 0f, 0f, 255f);
    //           }
    //           else
    //           {
    //               imag.color = new Color(0f, 255f, 0f, 255f);
    //           }
    //       }
    //   }  
    //}

    private void SetLockedIcons(GameObject LevelObject, bool isActive)
    {
        if (!isActive)
        {
            Image guiImage = LevelObject.transform.GetComponent<Image>();
            guiImage.sprite = LockedGUIimage;

            for (int i = 0; i < LevelObject.transform.childCount; i++)
            {
                LevelObject.transform.GetChild(i).gameObject.SetActive(false);
            }
        }
    }




    //Saves LevelJSON (nonunity c# files)
    public void SaveToJson()
    {

        string serialized = JsonConvert.SerializeObject(ListOfAllLevelJSON, Formatting.Indented,
                        new JsonSerializerSettings()
                        { 
                            ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                        });


        //Skriv till fil

        File.WriteAllText("save.json", JsonConvert.SerializeObject(ListOfAllLevelJSON, Formatting.Indented,
                        new JsonSerializerSettings()
                        { 
                            ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                        }));

        using (StreamWriter fileToWriteTo = File.CreateText("save.json"))
        {
            JsonSerializer serializer = new JsonSerializer();
            serializer.Serialize(fileToWriteTo, ListOfAllLevelJSON);
        }
    }


    //Loads LevelJSON (nonunity c# files)
    public void LoadFromJson()
    {
        //Ladda från fil

        ListOfAllLevelJSON = JsonConvert.DeserializeObject<List<LevelJSON>>(File.ReadAllText("save.json"));

        using (StreamReader fileToReadFrom = File.OpenText("save.json"))
        {
            JsonSerializer serializer = new JsonSerializer();
            ListOfAllLevelJSON =
                (List<LevelJSON>)serializer.Deserialize(fileToReadFrom, typeof(List<LevelJSON>));
        }

        if (ListOfAllLevelJSON == null)      //Om det inte finns någon sparning gjord till savefilen
        {
            ListOfAllLevelScriptInstances = JsonConvert.DeserializeObject<List<Level>>(File.ReadAllText("OriginalLevels.json"));

            using (StreamReader fileToReadFrom = File.OpenText("OriginalLevels.json"))
            {
                JsonSerializer serializer = new JsonSerializer();
                ListOfAllLevelJSON =
                    (List<LevelJSON>)serializer.Deserialize(fileToReadFrom, typeof(List<LevelJSON>));
            }
        }

    }
    




    /*
    public void AddLevelToList(Level levelToAdd)
    {
        bool levelHasAlreadyBeenAdded = false;

        if (ListOfAllLevels == null)
        {
            ListOfAllLevels = new List<Level>();
        }

        for (int i = 0; i < ListOfAllLevels.Count; i++)
        {
            if (ListOfAllLevels[i].Name.Equals(levelToAdd.Name))            //TODO Ska vara "AcctualLevel" istället för namn
            {
                levelHasAlreadyBeenAdded = true;
            }
        }

        if (!levelHasAlreadyBeenAdded)
        {
            ListOfAllLevels.Add(levelToAdd);
        }


  //      Save();     //Sparar i json den tillagdafilen
    }


    public void RemoveLevelFromList(Level LevelToRemove)
    {
        for (int i = 0; i < ListOfAllLevels.Count; i++)
        {
            if (ListOfAllLevels[i].Name.Equals(LevelToRemove.Name))         //TODO Ska vara "AcctualLevel" istället för namn
            {
                ListOfAllLevels.Remove(LevelToRemove);
            }
        }
    }

    */
    
}
