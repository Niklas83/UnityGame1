using System.IO;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using Newtonsoft.Json;
using Assets.DustinHorne.JsonDotNetUnity.TestCases;
using Assets.DustinHorne.JsonDotNetUnity.TestCases.TestModels;


public class LevelManager : MonoBehaviour {

    public List<GameObject> ListOfAllLevels;
    private List<Level> ListOfAllLevelScriptInstances;
    private List<LevelJSON> ListOfAllLevelJSON;

    private bool JsonCopyHasValues;

    public GameObject levelGameObject;      //being instantiated on load from json

    // Use this for initialization
    void Start()
    {
  
    //    SetAllLevelScriptInstances();
      //  SaveToJson();
        LoadFromJson();
        LoadAllLevelScriptInstances();
        LoadAllLevelGameObjects();
    }

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
        LevelJSON levelJSONToAdd = new LevelJSON();

        for (int i = 0; i < ListOfAllLevelScriptInstances.Count; i++)
        {
            levelJSONToAdd.SceneNr = ListOfAllLevelScriptInstances[i].SceneNr;
            levelJSONToAdd.Name = ListOfAllLevelScriptInstances[i].Name;
            levelJSONToAdd.HasPassed = ListOfAllLevelScriptInstances[i].HasPassed;
            levelJSONToAdd.IsActive = ListOfAllLevelScriptInstances[i].IsActive;
            levelJSONToAdd.Star1 = ListOfAllLevelScriptInstances[i].Star1;
            levelJSONToAdd.Star2 = ListOfAllLevelScriptInstances[i].Star2;
            levelJSONToAdd.Star3 = ListOfAllLevelScriptInstances[i].Star3;
            levelJSONToAdd.MaxStepsFor1Star = ListOfAllLevelScriptInstances[i].MaxStepsFor1Star;
            levelJSONToAdd.MaxStepsFor2Star = ListOfAllLevelScriptInstances[i].MaxStepsFor2Star;
            levelJSONToAdd.MaxStepsFor3Star = ListOfAllLevelScriptInstances[i].MaxStepsFor3Star;
            levelJSONToAdd.NumberOfSteps = ListOfAllLevelScriptInstances[i].NumberOfSteps;
            levelJSONToAdd.NumberOfExits = ListOfAllLevelScriptInstances[i].NumberOfExits;
            levelJSONToAdd.NumberOfExitsCleared = ListOfAllLevelScriptInstances[i].NumberOfExitsCleared;
            levelJSONToAdd.CoordinatesOfCleardExits = ListOfAllLevelScriptInstances[i].CoordinatesOfCleardExits;
            levelJSONToAdd.ObjectiveList = ListOfAllLevelScriptInstances[i].ObjectiveList;

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
            levelScriptToAdd.HasPassed = ListOfAllLevelJSON[i].HasPassed;
            levelScriptToAdd.IsActive = ListOfAllLevelJSON[i].IsActive;
            levelScriptToAdd.Star1 = ListOfAllLevelJSON[i].Star1;
            levelScriptToAdd.Star2 = ListOfAllLevelJSON[i].Star2;
            levelScriptToAdd.Star3 = ListOfAllLevelJSON[i].Star3;
            levelScriptToAdd.MaxStepsFor1Star = ListOfAllLevelJSON[i].MaxStepsFor1Star;
            levelScriptToAdd.MaxStepsFor2Star = ListOfAllLevelJSON[i].MaxStepsFor2Star;
            levelScriptToAdd.MaxStepsFor3Star = ListOfAllLevelJSON[i].MaxStepsFor3Star;
            levelScriptToAdd.NumberOfSteps = ListOfAllLevelJSON[i].NumberOfSteps;
            levelScriptToAdd.NumberOfExits = ListOfAllLevelJSON[i].NumberOfExits;
            levelScriptToAdd.NumberOfExitsCleared = ListOfAllLevelJSON[i].NumberOfExitsCleared;
            levelScriptToAdd.CoordinatesOfCleardExits = ListOfAllLevelJSON[i].CoordinatesOfCleardExits;
            levelScriptToAdd.ObjectiveList = ListOfAllLevelJSON[i].ObjectiveList;

            ListOfAllLevelScriptInstances.Add(levelScriptToAdd);    
        }
    }

    //used when loading
    private void LoadAllLevelGameObjects()
    {
        //Sätter parent till panelen som håller listan (Just nu kör jag LevelSelectionBackgroundTEST) TODO detta måste bytas då koden fungerar i sin helhet
        Transform levelPanel = GameObject.FindGameObjectWithTag("LevelListPanel").transform;

        float distanceFromTopMinY = 0.78f;      //  Används för att sätta levels från toppen, sätts -0.24/ level rad
        float distanceFromTopMaxY = 0.96f;      //  Används för att sätta levels från toppen, sätts -0.24/ level rad

        for (int i = 0; i < ListOfAllLevelScriptInstances.Count; i++)
        {
            GameObject newLevelToGUI = Instantiate(levelGameObject);

            newLevelToGUI.GetComponent<Level>().SceneNr = ListOfAllLevelScriptInstances[i].SceneNr;
            newLevelToGUI.GetComponent<Level>().Name = ListOfAllLevelScriptInstances[i].Name;
            newLevelToGUI.GetComponent<Level>().HasPassed = ListOfAllLevelScriptInstances[i].HasPassed;
            newLevelToGUI.GetComponent<Level>().IsActive = ListOfAllLevelScriptInstances[i].IsActive;
            newLevelToGUI.GetComponent<Level>().Star1 = ListOfAllLevelScriptInstances[i].Star1;
            newLevelToGUI.GetComponent<Level>().Star2 = ListOfAllLevelScriptInstances[i].Star2;
            newLevelToGUI.GetComponent<Level>().Star3 = ListOfAllLevelScriptInstances[i].Star3;
            newLevelToGUI.GetComponent<Level>().MaxStepsFor1Star = ListOfAllLevelScriptInstances[i].MaxStepsFor1Star;
            newLevelToGUI.GetComponent<Level>().MaxStepsFor2Star = ListOfAllLevelScriptInstances[i].MaxStepsFor2Star;
            newLevelToGUI.GetComponent<Level>().MaxStepsFor3Star = ListOfAllLevelScriptInstances[i].MaxStepsFor3Star;
            newLevelToGUI.GetComponent<Level>().NumberOfSteps = ListOfAllLevelScriptInstances[i].NumberOfSteps;
            newLevelToGUI.GetComponent<Level>().NumberOfExits = ListOfAllLevelScriptInstances[i].NumberOfExits;
            newLevelToGUI.GetComponent<Level>().NumberOfExitsCleared = ListOfAllLevelScriptInstances[i].NumberOfExitsCleared;
            newLevelToGUI.GetComponent<Level>().CoordinatesOfCleardExits = ListOfAllLevelScriptInstances[i].CoordinatesOfCleardExits;
            newLevelToGUI.GetComponent<Level>().ObjectiveList = ListOfAllLevelScriptInstances[i].ObjectiveList;


            newLevelToGUI.name = "LevelObject_" + i;

            //Sätter parent till panelen som håller listan (Just nu kör jag LevelSelectionBackgroundTEST) TODO detta måste bytas då koden fungerar i sin helhet
            newLevelToGUI.transform.SetParent(levelPanel, false);
            

            //Positionering av level objektet i listan
            if (i > 1)
            {
                if (i%2 == 0)
                {
                    distanceFromTopMinY = distanceFromTopMinY - 0.24f;
                    distanceFromTopMaxY = distanceFromTopMaxY - 0.24f;
                }
            }

            RectTransform newRectTransform = newLevelToGUI.GetComponent<RectTransform>();
            if (i%2 == 0)
            {
                newRectTransform.anchorMin = new Vector2(0.05f, distanceFromTopMinY);
                newRectTransform.anchorMax = new Vector2(0.425f, distanceFromTopMaxY);
            }
            else
            {
                newRectTransform.anchorMin = new Vector2(0.575f, distanceFromTopMinY);
                newRectTransform.anchorMax = new Vector2(0.95f, distanceFromTopMaxY);
            }
        }
        

   
    }




    

    //Saves LevelJSON (nonunity c# files)
    void SaveToJson()
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
    void LoadFromJson()
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
