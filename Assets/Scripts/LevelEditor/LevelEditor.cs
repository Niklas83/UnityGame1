using System;
using System.IO;
using System.Linq;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LevelEditor : MonoBehaviour {
    //Selectable objects/items from resources
    private List<GameObject> ResourceBoxObjects;              //List of all non-movable i listan från resources
    private List<GameObject> ResourcePlayerObjects;              //List of all non-movable i listan från resources
    private List<GameObject> ResourceProjectileShooterObjects;              //List of all non-movable i listan från resources
    private List<GameObject> ResourceFloorTileObjects;              //List of all non-movable i listan från resources
    private List<GameObject> ResourceSpecialObjects;              //List of all non-movable i listan från resources

    //private GameObject CurrentItemBeingCreated;


    //Game objects from map
    private GameObject[] GameObjectsInLevel;
    private List<GameObject> AllBoxesInLevel = new List<GameObject>();
    private List<GameObject> AllPlayersInLevel = new List<GameObject>();  //not implemented
    private List<GameObject> AllProjectileShootersInLevel = new List<GameObject>(); //not implemented
    private List<GameObject> AllFloorTilesInLevel = new List<GameObject>(); //not implemented
    private List<GameObject> AllSpecialObjectsInLevel = new List<GameObject>(); //not implemented

    //Game objects from file
    //private List<GameObject> AllBoxesFromFile = new List<GameObject>();

    private string _FileLocation;
    private string _FileName;

    private string JsonStringToSave;
    private string JsonStringThatHasBeenLoaded;

	// Use this for initialization
	void Start ()
	{
        ResourceBoxObjects = Resources.LoadAll<GameObject>("Boxes").ToList();           //Fixa detta med ett obstaclegenerator object, antingen en instans av den nuvarande eller genom att bryta ut från den till nya klasser
        ResourcePlayerObjects = Resources.LoadAll<GameObject>("Avatar").ToList();
        ResourceProjectileShooterObjects = Resources.LoadAll<GameObject>("Enemies").ToList();
        ResourceFloorTileObjects = Resources.LoadAll<GameObject>("Floor").ToList();
        ResourceSpecialObjects = Resources.LoadAll<GameObject>("Special").ToList();

        //Where to save and load to and from 
        _FileLocation = Application.dataPath;

        _FileLocation = _FileLocation + "/LevelEditorMaps";

        _FileName = "TheMapOnFile.json";


        //Fills all the list of items in the level
	    PopulateAllLevelObjectLists();          

	    CreateJsonString();

	    CreateJSON();

	    LoadJSON();

	    CreateGameObjectsFromJSON();

	       GameObject clonedGameObject = (GameObject)Instantiate(AllBoxesInLevel[0], new Vector3(5, 1, 5), Quaternion.identity);
	}
	
    private void CreateJsonString()
    {
        JSONObject ObjectArray = new JSONObject(JSONObject.Type.ARRAY);

        //Adds all boxes to the ObjectArray
        JSONObject jsonBoxes = new JSONObject(JSONObject.Type.ARRAY);

        ObjectArray.AddField("Boxes", jsonBoxes);

        for (int i = 0; i < AllBoxesInLevel.Count; i++)
        {

            JSONObject current = new JSONObject(JSONObject.Type.ARRAY);

            current.AddField("xPos", AllBoxesInLevel[i].transform.position.x);

            current.AddField("yPos", AllBoxesInLevel[i].transform.position.y);

            current.AddField("zPos", AllBoxesInLevel[i].transform.position.z);

            current.AddField("xRot", (int)Math.Round(AllBoxesInLevel[i].transform.eulerAngles.x));

            current.AddField("yRot", (int)Math.Round(AllBoxesInLevel[i].transform.eulerAngles.y));

            current.AddField("zRot", (int)Math.Round(AllBoxesInLevel[i].transform.eulerAngles.z));

            current.AddField("tag", AllBoxesInLevel[i].tag);

            current.AddField("name", AllBoxesInLevel[i].name);

            jsonBoxes.Add(current);

        }

        //Adds all players to the ObjectArray
        JSONObject jsonPlayers = new JSONObject(JSONObject.Type.ARRAY);

        ObjectArray.AddField("Players", jsonPlayers);

        for (int i = 0; i < AllPlayersInLevel.Count; i++)
        {

            JSONObject current = new JSONObject(JSONObject.Type.ARRAY);

            current.AddField("xPos", AllPlayersInLevel[i].transform.position.x);

            current.AddField("yPos", AllPlayersInLevel[i].transform.position.y);

            current.AddField("zPos", AllPlayersInLevel[i].transform.position.z);

            current.AddField("xRot", (int)Math.Round(AllPlayersInLevel[i].transform.eulerAngles.x));

            current.AddField("yRot", (int)Math.Round(AllPlayersInLevel[i].transform.eulerAngles.y));

            current.AddField("zRot", (int)Math.Round(AllPlayersInLevel[i].transform.eulerAngles.z));

            current.AddField("tag", AllPlayersInLevel[i].tag);

            current.AddField("name", AllPlayersInLevel[i].name);

            jsonPlayers.Add(current);

        }

        //Adds all ProjectileShooters to the ObjectArray
        JSONObject jsonProjectileShooters = new JSONObject(JSONObject.Type.ARRAY);

        ObjectArray.AddField("ProjectileShooters", jsonProjectileShooters);

        for (int i = 0; i < AllProjectileShootersInLevel.Count; i++)
        {

            JSONObject current = new JSONObject(JSONObject.Type.ARRAY);

            current.AddField("xPos", AllProjectileShootersInLevel[i].transform.position.x);

            current.AddField("yPos", AllProjectileShootersInLevel[i].transform.position.y);

            current.AddField("zPos", AllProjectileShootersInLevel[i].transform.position.z);

            current.AddField("xRot", (int)Math.Round(AllProjectileShootersInLevel[i].transform.eulerAngles.x));

            current.AddField("yRot", (int)Math.Round(AllProjectileShootersInLevel[i].transform.eulerAngles.y));

            current.AddField("zRot", (int)Math.Round(AllProjectileShootersInLevel[i].transform.eulerAngles.z));

            current.AddField("tag", AllProjectileShootersInLevel[i].tag);

            current.AddField("name", AllProjectileShootersInLevel[i].name);

            jsonProjectileShooters.Add(current);

        }

        //Adds all FloorTiles to the ObjectArray
        JSONObject jsonFloorTiles = new JSONObject(JSONObject.Type.ARRAY);

        ObjectArray.AddField("FloorTiles", jsonFloorTiles);

        for (int i = 0; i < AllFloorTilesInLevel.Count; i++)
        {

            JSONObject current = new JSONObject(JSONObject.Type.ARRAY);

            current.AddField("xPos", AllFloorTilesInLevel[i].transform.position.x);

            current.AddField("yPos", AllFloorTilesInLevel[i].transform.position.y);

            current.AddField("zPos", AllFloorTilesInLevel[i].transform.position.z);

            current.AddField("xRot", (int)Math.Round(AllFloorTilesInLevel[i].transform.eulerAngles.x));

            current.AddField("yRot", (int)Math.Round(AllFloorTilesInLevel[i].transform.eulerAngles.y));

            current.AddField("zRot", (int)Math.Round(AllFloorTilesInLevel[i].transform.eulerAngles.z));

            current.AddField("tag", AllFloorTilesInLevel[i].tag);

            current.AddField("name", AllFloorTilesInLevel[i].name);

            //unique for floor:

            if (AllFloorTilesInLevel[i].GetComponent<Renderer>() != null)       //TODO Eather remove the need for this by not needing to offset floor tiles or make sure all got a render component
            {
                current.AddField("textOffsetX", AllFloorTilesInLevel[i].renderer.material.GetTextureOffset("_MainTex").x);
                current.AddField("textOffsetY", AllFloorTilesInLevel[i].renderer.material.GetTextureOffset("_MainTex").y);

                current.AddField("textScaleX", AllFloorTilesInLevel[i].renderer.material.GetTextureScale("_MainTex").x);
                current.AddField("textScaleY", AllFloorTilesInLevel[i].renderer.material.GetTextureScale("_MainTex").y);
            }
            jsonFloorTiles.Add(current);

        }
        
        //Adds all SpecialObjects to the ObjectArray
        JSONObject jsonSpecialObjects = new JSONObject(JSONObject.Type.ARRAY);

        ObjectArray.AddField("SpecialObjects", jsonSpecialObjects);

        for (int i = 0; i < AllSpecialObjectsInLevel.Count; i++)
        {

            JSONObject current = new JSONObject(JSONObject.Type.ARRAY);

            current.AddField("xPos", AllSpecialObjectsInLevel[i].transform.position.x);

            current.AddField("yPos", AllSpecialObjectsInLevel[i].transform.position.y);

            current.AddField("zPos", AllSpecialObjectsInLevel[i].transform.position.z);

            current.AddField("xRot", (int)Math.Round(AllSpecialObjectsInLevel[i].transform.eulerAngles.x));

            current.AddField("yRot", (int)Math.Round(AllSpecialObjectsInLevel[i].transform.eulerAngles.y));

            current.AddField("zRot", (int)Math.Round(AllSpecialObjectsInLevel[i].transform.eulerAngles.z));

            current.AddField("tag", AllSpecialObjectsInLevel[i].tag);

            current.AddField("name", AllSpecialObjectsInLevel[i].name);

            jsonSpecialObjects.Add(current);

        }
        
        JsonStringToSave = ObjectArray.Print();       //Creates the string that later is saved to file
    }

    private void PopulateAllLevelObjectLists()
    {
        GameObjectsInLevel = FindObjectsOfType(typeof(GameObject)) as GameObject[];

        foreach (GameObject currentGameObject in GameObjectsInLevel)
        {
            if (currentGameObject.tag == "Box")
            {
                AllBoxesInLevel.Add(currentGameObject);
            }

            else if (currentGameObject.tag == "ProjectileShooter")
            {
                AllProjectileShootersInLevel.Add(currentGameObject);
            }
        
            else if (currentGameObject.tag == "Player")
            {
                AllPlayersInLevel.Add(currentGameObject);
            }

            else if (currentGameObject.tag == "Floor")
            {
                AllFloorTilesInLevel.Add(currentGameObject);
            }

            else if (currentGameObject.tag == "Special")        //Candles coins boost items etc etc
            {
                AllSpecialObjectsInLevel.Add(currentGameObject);
            }
        }
        
    }


    void CreateGameObjectsFromJSON()
    {
        JSONObject ArrayOfAllGameObjectsFromFile = new JSONObject(JsonStringThatHasBeenLoaded);

        JSONObject BoxObjects = ArrayOfAllGameObjectsFromFile["Boxes"];

        for (int i = 0; i < BoxObjects.Count; i++)
        {
            
            for (int j = 0; j < ResourceBoxObjects.Count; j++)
            {
                GameObject item = ResourceBoxObjects[j];
                if (item.name == GetStringFromJson(BoxObjects[i]["name"]))
                {

                    GameObject newGameObject = (GameObject)Instantiate(item, new Vector3(GetFloatFromJson(BoxObjects[i]["xPos"]), GetFloatFromJson(BoxObjects[i]["yPos"]), GetFloatFromJson(BoxObjects[i]["zPos"])), Quaternion.Euler(new Vector3(GetFloatFromJson(BoxObjects[i]["xRot"]), GetFloatFromJson(BoxObjects[i]["yRot"]), GetFloatFromJson(BoxObjects[i]["zRot"]))));

                    break;
                }
            }

        }

        //Players
        JSONObject PlayerObjects = ArrayOfAllGameObjectsFromFile["Players"];

        for (int i = 0; i < PlayerObjects.Count; i++)
        {

            for (int j = 0; j < ResourcePlayerObjects.Count; j++)
            {
                GameObject item = ResourcePlayerObjects[j];
                if (item.name == GetStringFromJson(PlayerObjects[i]["name"]))
                {

                    GameObject newGameObject = (GameObject)Instantiate(item, new Vector3(GetFloatFromJson(PlayerObjects[i]["xPos"]), GetFloatFromJson(PlayerObjects[i]["yPos"]), GetFloatFromJson(PlayerObjects[i]["zPos"])), Quaternion.Euler(new Vector3(GetFloatFromJson(PlayerObjects[i]["xRot"]), GetFloatFromJson(PlayerObjects[i]["yRot"]), GetFloatFromJson(PlayerObjects[i]["zRot"]))));

                    break;
                }
            }
        }

        //projectileshooters
        JSONObject ProjectileShooterObjects = ArrayOfAllGameObjectsFromFile["ProjectileShooters"];

        for (int i = 0; i < ProjectileShooterObjects.Count; i++)
        {

            for (int j = 0; j < ResourceProjectileShooterObjects.Count; j++)
            {
                GameObject item = ResourceProjectileShooterObjects[j];
                if (item.name == GetStringFromJson(ProjectileShooterObjects[i]["name"]))
                {

                    GameObject newGameObject = (GameObject)Instantiate(item, new Vector3(GetFloatFromJson(ProjectileShooterObjects[i]["xPos"]), GetFloatFromJson(ProjectileShooterObjects[i]["yPos"]), GetFloatFromJson(ProjectileShooterObjects[i]["zPos"])), Quaternion.Euler(new Vector3(GetFloatFromJson(ProjectileShooterObjects[i]["xRot"]), GetFloatFromJson(ProjectileShooterObjects[i]["yRot"]), GetFloatFromJson(ProjectileShooterObjects[i]["zRot"]))));

                    break;
                }
            }
        }

        //floor
        JSONObject FloorObjects = ArrayOfAllGameObjectsFromFile["FloorTiles"];

        for (int i = 0; i < FloorObjects.Count; i++)
        {

            for (int j = 0; j < ResourceFloorTileObjects.Count; j++)
            {
                GameObject item = ResourceFloorTileObjects[j];
                if (item.name == GetStringFromJson(FloorObjects[i]["name"]) || item.name + "(Clone)" == GetStringFromJson(FloorObjects[i]["name"]))     //TODO: Fixa bort alla Tile(Clone) etc så dessa inte genereras
                {

                    GameObject newGameObject = (GameObject)Instantiate(item, new Vector3(GetFloatFromJson(FloorObjects[i]["xPos"]), GetFloatFromJson(FloorObjects[i]["yPos"]), GetFloatFromJson(FloorObjects[i]["zPos"])), Quaternion.Euler(new Vector3(GetFloatFromJson(FloorObjects[i]["xRot"]), GetFloatFromJson(FloorObjects[i]["yRot"]), GetFloatFromJson(FloorObjects[i]["zRot"]))));

                    if (newGameObject.GetComponent<Renderer>() != null)   //TODO Eather remove the need for this by not needing to offset floor tiles or make sure all got a render component
                    {
                        newGameObject.renderer.material.SetTextureOffset("_MainTex", new Vector2(GetFloatFromJson(FloorObjects[i]["textOffsetX"]),GetFloatFromJson(FloorObjects[i]["textOffsetY"])));
                        newGameObject.renderer.material.SetTextureScale("_MainTex", new Vector2(GetFloatFromJson(FloorObjects[i]["textScaleX"]), GetFloatFromJson(FloorObjects[i]["textScaleY"])));
                    }
                   
                    break;
                }
            }
        }

        //Special
        JSONObject SpecialObjects = ArrayOfAllGameObjectsFromFile["SpecialObjects"];

        for (int i = 0; i < SpecialObjects.Count; i++)
        {

            for (int j = 0; j < ResourceSpecialObjects.Count; j++)
            {
                GameObject item = ResourceSpecialObjects[j];
                if (item.name == GetStringFromJson(SpecialObjects[i]["name"]) || item.name + "(Clone)" == GetStringFromJson(SpecialObjects[i]["name"]))
                {

                    GameObject newGameObject = (GameObject)Instantiate(item, new Vector3(GetFloatFromJson(SpecialObjects[i]["xPos"]), GetFloatFromJson(SpecialObjects[i]["yPos"]), GetFloatFromJson(SpecialObjects[i]["zPos"])), Quaternion.Euler(new Vector3(GetFloatFromJson(SpecialObjects[i]["xRot"]), GetFloatFromJson(SpecialObjects[i]["yRot"]), GetFloatFromJson(SpecialObjects[i]["zRot"]))));

                    break;
                }
            }
        }



    }







    // Finally our save and load methods for the file itself 
    void CreateJSON()
    {
        StreamWriter writer;
        FileInfo t = new FileInfo(_FileLocation + "\\" + _FileName);
        if (!t.Exists)
        {
            writer = t.CreateText();
        }
        else
        {
            t.Delete();
            writer = t.CreateText();
        }
        writer.Write(JsonStringToSave);
        writer.Close();
        Debug.Log("File written.");
    }

    void LoadJSON()
    {
        StreamReader r = File.OpenText(_FileLocation + "\\" + _FileName);
        string _info = r.ReadToEnd();
        r.Close();
        JsonStringThatHasBeenLoaded = _info;
        Debug.Log("File Read");
    }






















    string GetStringFromJson(JSONObject obj)
    {
        if (obj.type == JSONObject.Type.STRING)
        {
            return obj.str;
        }
        else
        {
            return "";
        }
    }

    float GetFloatFromJson(JSONObject obj)
    {
        if (obj.type == JSONObject.Type.NUMBER)
        {
            return obj.n;
        }
        else
        {
            return -1;
        }
    }

    private bool GetBoolFromJson(JSONObject obj)
    {
        if (obj.type == JSONObject.Type.BOOL)
        {
            return obj.b;
        }
        else
        {
            return false;
        }
    }



    void accessData(JSONObject obj)
    {
        switch (obj.type)
        {
            case JSONObject.Type.OBJECT:
                for (int i = 0; i < obj.list.Count; i++)
                {
                    string key = (string)obj.keys[i];
                    JSONObject j = (JSONObject)obj.list[i];
                    Debug.Log(key);
                    accessData(j);
                }
                break;
            case JSONObject.Type.ARRAY:
                foreach (JSONObject j in obj.list)
                {
                    accessData(j);
                }
                break;
            case JSONObject.Type.STRING:
                Debug.Log(obj.str);
                break;
            case JSONObject.Type.NUMBER:
                Debug.Log(obj.n);
                break;
            case JSONObject.Type.BOOL:
                Debug.Log(obj.b);
                break;
            case JSONObject.Type.NULL:
                Debug.Log("NULL");
                break;

        }
    }













}
