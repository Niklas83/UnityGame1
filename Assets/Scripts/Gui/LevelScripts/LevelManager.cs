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
    public List<LevelJSON> ListOfAllLevelJSON;


    private Dictionary<int,List<Level>> _worldLists;  
    private List<Level> ListOfAllLevelScriptInstances;
   
    private int _currentWorldPage;      //USED BY THE GUI

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
            NEWLoadAllLevelGameObjects(1);  //Loads first world page
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
            levelJSONToAdd.WorldName = ListOfAllLevelScriptInstances[i].WorldName;

            ListOfAllLevelJSON.Add(levelJSONToAdd);
        }
    }


    //used when loading
    private void LoadAllLevelScriptInstances()
    {
        ListOfAllLevelScriptInstances = new List<Level>();
        
        _worldLists = new Dictionary<int, List<Level>>();

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
            levelScriptToAdd.WorldName = ListOfAllLevelJSON[i].WorldName;

            if (!_worldLists.ContainsKey(ListOfAllLevelJSON[i].WorldPageNumber))
            {
                List<Level> levelList = new List<Level>();
                levelList.Add(levelScriptToAdd);
                _worldLists.Add(ListOfAllLevelJSON[i].WorldPageNumber, levelList);

                ListOfAllLevelScriptInstances.Add(levelScriptToAdd);  
            }

            else
            {
                List<Level> levelList;
                _worldLists.TryGetValue(ListOfAllLevelJSON[i].WorldPageNumber, out levelList);
                levelList.Add(levelScriptToAdd);

                ListOfAllLevelScriptInstances.Add(levelScriptToAdd);  
            }
        }
    }


    private void NEWLoadAllLevelGameObjects(int LevelPageNum)
    {
        //This iteration is needed to access the levelgrid cuz the main parent object is inactive
        GameObject parent = GameObject.Find("Background");
        GameObject LevelPage = MiscHelperMethods.FindObject(parent,"LevelsPage");
        GameObject WindowsLevels = MiscHelperMethods.FindObject(LevelPage,"WindowLevels");
        GameObject Content = MiscHelperMethods.FindObject(WindowsLevels,"Content");
        GameObject ScrollView = MiscHelperMethods.FindObject(Content,"ScrollView");

        GameObject WorldName = MiscHelperMethods.FindObject(Content, "WorldName");
        GameObject ButtonPrev = MiscHelperMethods.FindObject(Content, "Button_Prev");
        GameObject ButtonNext = MiscHelperMethods.FindObject(Content, "Button_Next");

        GameObject LevelsGrid = MiscHelperMethods.FindObject(ScrollView,"LevelsGrid");

        //To add event listener
        GUILogic _guiLogic = transform.GetComponent<GUILogic>();


        //CLEARS THE GAME OBJECTS OF THE LIST of previous page
        foreach (Transform Child in LevelsGrid.transform)
        {
            Destroy(Child.gameObject);
        }

        //Loads the level to load from the dictionary
        List<Level> levelsToLoad;
        _worldLists.TryGetValue(LevelPageNum, out levelsToLoad);

        //Sets the current loaded page
        _currentWorldPage = LevelPageNum;

        //Sets the "world name"
        WorldName.GetComponent<Text>().text = levelsToLoad[0].WorldName;

        //check if "previous button" shall be visible
        if (!_worldLists.ContainsKey(LevelPageNum - 1))
        {
            ButtonPrev.SetActive(false);
        }
        else
        {
            ButtonPrev.SetActive(true);
        }

        //check if "next button" shall be visible
        if (!_worldLists.ContainsKey(LevelPageNum + 1))
        {
            ButtonNext.SetActive(false);
        }
        else
        {
            ButtonNext.SetActive(true);
        }


        for (int i = 0; i < levelsToLoad.Count; i++)
        {

            GameObject newLevelToGUI = Instantiate(levelGameObject);
            Level newLevelToGUIScript = newLevelToGUI.GetComponent<Level>();


            newLevelToGUIScript.SceneNr = levelsToLoad[i].SceneNr;
            newLevelToGUIScript.Name = levelsToLoad[i].Name;
            newLevelToGUIScript.IsPassed = levelsToLoad[i].IsPassed;
            newLevelToGUIScript.IsActive = levelsToLoad[i].IsActive;
            newLevelToGUIScript.InformationList = levelsToLoad[i].InformationList;
            newLevelToGUIScript.IsBonusLevel = levelsToLoad[i].IsBonusLevel;
            newLevelToGUIScript.StarValue = levelsToLoad[i].StarValue;
            newLevelToGUIScript.WorldPageNumber = levelsToLoad[i].WorldPageNumber;
            newLevelToGUIScript.WorldName = levelsToLoad[i].WorldName;

            newLevelToGUI.name = "LevelObject_" + i;

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
    



    //              *****       GUI USING THE BELOW CODE        ****


    

    public void LoadNextWorldPage()
    {
        NEWLoadAllLevelGameObjects(_currentWorldPage + 1);
    }


    public void LoadPreviousWorldPage()
    {
        NEWLoadAllLevelGameObjects(_currentWorldPage - 1);
    }
    
}
