using System.IO;
using System.Linq;
using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
public class ObstacleGeneratorWindow : EditorWindow
{
    GameObject Selected;        //The currently selected gameobject (item in the inspector)

    Editor gameObjectEditor = new Editor();     //The editor that is used to show the 3d preview window

    public Vector2 scrollPos = new Vector2(0, 0);        //The prefab list position

    GameObject[] MovableObstacles;                  //List of all movable objects
    GameObject[] NoneMovableObstacles;              //List of all non-movable
    GameObject[] RotatableObstacles;              //List of all rotatable
    GameObject[] EnemieObstacles;              //List of all enemies
    GameObject[] AvatarPrefabs;              //List of all avatars
    GameObject[] FloorPrefabs;              //List of all floors
    GameObject[] ScenePrefabs;              //List of all scene prefabs
    GameObject[] SpecialPrefabs;              //List of all scene prefabs
    GameObject[] ALLPrefabs;              //List of ALL prefabs in recources

    List<GameObject> AllObstaclesToCurrentlyShow;       //List of the objects that currently should be shown in the list

    private bool _ShowPrefabCheckBoxes = true;             //The toggle bool that determens if the checkboxes that selects prefabs should be shown

    private bool _ShowMovableObstacles;      //Tells if the movable obstacles should be shown
    private bool _ShowNonMovableObstacles;      //Tells if the non-movable obstacles should be shown
    private bool _ShowRotatableObstacles;      //Tells if the rotatable obstacles should be shown
    private bool _ShowEnemieObstacles;      //Tells if the enemie obstacles should be shown
    private bool _ShowAvatarPrefabs;      //Tells if the avatars should be shown
    private bool _ShowFloorPrefabs;      //Tells if the avatars should be shown
    private bool _ShowScenePrefabs;      //Tells if the scene prefabs should be shown
    private bool _ShowSpecialPrefabs;      //Tells if the scene prefabs should be shown
    private bool _ShowALLPrefabs;      //Tells if the scene prefabs should be shown

    private bool _PreviouslyShowMovableObstacles;       //tells if the value has changed since last code execution  (Differd vs _ShowMovableObstacles)
    private bool _PreviouslyShowNonMovableObstacles;    //tells if the value has changed since last code execution  (Differd vs _ShowNonMovableObstacles)
    private bool _PreviouslyShowRotatableObstacles;    //tells if the value has changed since last code execution  (Differd vs _ShowRotatableObstacles)
    private bool _PreviouslyShowEnemieObstacles;    //tells if the value has changed since last code execution  (Differd vs _ShowEnemieObstacles)
    private bool _PreviouslyShowAvatarPrefabs;    //tells if the value has changed since last code execution  (Differd vs _ShowEnemieObstacles)
    private bool _PreviouslyShowFloorPrefabs;    //tells if the value has changed since last code execution  (Differd vs _ShowEnemieObstacles)
    private bool _PreviouslyShowScenePrefabs;    //tells if the value has changed since last code execution  (Differd vs _ShowEnemieObstacles)
    private bool _PreviouslyShowSpecialPrefabs;    //tells if the value has changed since last code execution  (Differd vs _ShowEnemieObstacles)
    private bool _PreviouslyShowALLPrefabs;    //tells if the value has changed since last code execution  (Differd vs _ShowEnemieObstacles)


    private List<bool> _PrefabSectionsToShow = new List<bool>();    //Used to see if the list of items should be updated (by differing vs _PrefabSectionsPreviouslyShown)
    private List<bool> _PrefabSectionsPreviouslyShown;          //Used to see if the list of items should be updated(by differing vs _PrefabSectionsToShow)


    private Vector3 _LocationToPlacePrefab = new Vector3(5,1,5);

       // private int _Prefabs = 0;     (NOT IN USE)
       //private List<string> prefabTypes = new List<string>(); 
    
    [MenuItem("Window/ObstacleGenerator")]
    static void Init()
    {
        ObstacleGeneratorWindow window = (ObstacleGeneratorWindow)EditorWindow.GetWindow(typeof(ObstacleGeneratorWindow));
    }

    void OnGUI()
    {
        //Enlists all bool checkboxes
        if (_PreviouslyShowMovableObstacles != _ShowMovableObstacles || _PreviouslyShowNonMovableObstacles != _ShowNonMovableObstacles || _PreviouslyShowRotatableObstacles != _ShowRotatableObstacles || _PreviouslyShowEnemieObstacles != _ShowEnemieObstacles || _PreviouslyShowAvatarPrefabs != _ShowAvatarPrefabs || _PreviouslyShowFloorPrefabs != _ShowFloorPrefabs || _PreviouslyShowScenePrefabs != _ShowScenePrefabs || _PreviouslyShowSpecialPrefabs != _ShowSpecialPrefabs || _PreviouslyShowALLPrefabs != _ShowALLPrefabs)
        {
            _PrefabSectionsToShow = new List<bool>();
            
            _PrefabSectionsToShow.Add(_ShowMovableObstacles);
            _PrefabSectionsToShow.Add(_ShowNonMovableObstacles);
            _PrefabSectionsToShow.Add(_ShowRotatableObstacles);
            _PrefabSectionsToShow.Add(_ShowEnemieObstacles);
            _PrefabSectionsToShow.Add(_ShowAvatarPrefabs);
            _PrefabSectionsToShow.Add(_ShowFloorPrefabs);
            _PrefabSectionsToShow.Add(_ShowScenePrefabs);
            _PrefabSectionsToShow.Add(_ShowSpecialPrefabs);
            _PrefabSectionsToShow.Add(_ShowALLPrefabs);

            _PreviouslyShowMovableObstacles = _ShowMovableObstacles;
            _PreviouslyShowNonMovableObstacles = _ShowNonMovableObstacles;
            _PreviouslyShowRotatableObstacles = _ShowRotatableObstacles;
            _PreviouslyShowEnemieObstacles = _ShowEnemieObstacles;
            _PreviouslyShowAvatarPrefabs = _ShowAvatarPrefabs;
            _PreviouslyShowFloorPrefabs = _ShowFloorPrefabs;
            _PreviouslyShowScenePrefabs = _ShowScenePrefabs;
            _PreviouslyShowSpecialPrefabs = _ShowSpecialPrefabs;
            _PreviouslyShowALLPrefabs = _ShowALLPrefabs;
        }

        //Populates the diffrent sections (Checking just these two, as atleast one of these should have an item)
        if (NoneMovableObstacles == null || MovableObstacles== null)
        {
            NoneMovableObstacles = Resources.LoadAll<GameObject>("Boxes/Static");
       
            MovableObstacles = Resources.LoadAll<GameObject>("Boxes/Movables");

            RotatableObstacles = Resources.LoadAll<GameObject>("Boxes/Rotatable");

            EnemieObstacles = Resources.LoadAll<GameObject>("Enemies");

            AvatarPrefabs = Resources.LoadAll<GameObject>("Avatar");

            FloorPrefabs = Resources.LoadAll<GameObject>("Floor");

            ScenePrefabs = Resources.LoadAll<GameObject>("Scene");

            SpecialPrefabs = Resources.LoadAll<GameObject>("Special");

            ALLPrefabs = Resources.LoadAll<GameObject>("");
        }

        //DENNA KOD TAR FRAM ALLA FOLDERS I RESOURCES FOLDERN STARTAR   (NOT IN USE)
        //var dirPaths = Directory.GetFiles(Application.dataPath + "/Resources/", "*.PREFAB", SearchOption.AllDirectories);

        //if (prefabTypes.Count < 1)
        //{
        //    for (int i = 0; i < dirPaths.Count(); i++)
        //    {

        //        string folderPath = dirPaths[i].Remove(0, dirPaths[i].LastIndexOf("Resources") + 10);


        //        folderPath = folderPath.Replace("\\", "/");

        //        if (folderPath.Contains("/"))
        //        {
        //            folderPath = folderPath.Remove(folderPath.LastIndexOf("/"));
        //        }
        //        prefabTypes.Add(folderPath);
        //    }
        //}
        //DENNA KOD TAR FRAM ALLA FOLDERS I RESOURCES FOLDERN SLUTAR    (NOT IN USE)


        EditorGUILayout.BeginVertical(GUILayout.Width(300), GUILayout.MinWidth(300));

        EditorGUILayout.Space();

        //The GUI dropdown field that shows all the folders:
        //_Prefabs = EditorGUILayout.MaskField("Selected prefab folders", _Prefabs, prefabTypes.ToArray());     //(NOT IN USE)


        EditorGUILayout.Space();

        

        //Checks if the list of objects should be updated
        bool updateList = false;

        if (_PrefabSectionsPreviouslyShown == null)
        {
            updateList = true;
        }
        else if (_PrefabSectionsToShow.Count != _PrefabSectionsPreviouslyShown.Count)
        {
            updateList = true;
        }
        else 
        {
            for (int i = 0; i < _PrefabSectionsToShow.Count; i++)
            {
                if (_PrefabSectionsToShow[i] != _PrefabSectionsPreviouslyShown[i])
                {
                    updateList = true;
                }
            }
        }

        if (updateList)
        {
            AllObstaclesToCurrentlyShow = new List<GameObject>();
            if (_ShowNonMovableObstacles == true)
            {
                for (int i = 0; i < NoneMovableObstacles.Count(); i++)
                {
                    AllObstaclesToCurrentlyShow.Add(NoneMovableObstacles[i]);
                }
            }

            if (_ShowMovableObstacles == true)
            {
                for (int i = 0; i < MovableObstacles.Count(); i++)
                {
                    AllObstaclesToCurrentlyShow.Add(MovableObstacles[i]);
                }
            }

            if (_ShowRotatableObstacles == true)
            {
                for (int i = 0; i < RotatableObstacles.Count(); i++)
                {
                    AllObstaclesToCurrentlyShow.Add(RotatableObstacles[i]);
                }
            }

            if (_ShowEnemieObstacles == true)
            {
                for (int i = 0; i < EnemieObstacles.Count(); i++)
                {
                    AllObstaclesToCurrentlyShow.Add(EnemieObstacles[i]);
                }
            }

            if (_ShowAvatarPrefabs == true)
            {
                for (int i = 0; i < AvatarPrefabs.Count(); i++)
                {
                    AllObstaclesToCurrentlyShow.Add(AvatarPrefabs[i]);
                }
            }

            if (_ShowFloorPrefabs == true)
            {
                for (int i = 0; i < FloorPrefabs.Count(); i++)
                {
                    AllObstaclesToCurrentlyShow.Add(FloorPrefabs[i]);
                }
            }

            if (_ShowScenePrefabs == true)
            {
                for (int i = 0; i < ScenePrefabs.Count(); i++)
                {
                    AllObstaclesToCurrentlyShow.Add(ScenePrefabs[i]);
                }
            }

            if (_ShowSpecialPrefabs == true)
            {
                for (int i = 0; i < SpecialPrefabs.Count(); i++)
                {
                    AllObstaclesToCurrentlyShow.Add(SpecialPrefabs[i]);
                }
            }

            if (_ShowALLPrefabs == true)
            {
                for (int i = 0; i < ALLPrefabs.Count(); i++)
                {
                    AllObstaclesToCurrentlyShow.Add(ALLPrefabs[i]);
                }
            }

            _PrefabSectionsPreviouslyShown = _PrefabSectionsToShow;

        }

        //The GUI list shown in the window:
        scrollPos = EditorGUILayout.BeginScrollView(scrollPos, GUILayout.Width(300), GUILayout.MinWidth(300), GUILayout.Height(300));

        if (AllObstaclesToCurrentlyShow != null && AllObstaclesToCurrentlyShow.Any())

        for (int i = 0; i < AllObstaclesToCurrentlyShow.Count; i++)
        {
            if (MovableObstacles.Contains(AllObstaclesToCurrentlyShow[i]))
            {
                GUI.color = Color.green;
                EditorGUILayout.ObjectField(AllObstaclesToCurrentlyShow[i].name, AllObstaclesToCurrentlyShow[i], typeof(GameObject));
                GUI.color = Color.white;
            }
            else if (NoneMovableObstacles.Contains(AllObstaclesToCurrentlyShow[i]))
            {
                GUI.color = Color.Lerp(Color.white, Color.red,0.5f);
                EditorGUILayout.ObjectField(AllObstaclesToCurrentlyShow[i].name, AllObstaclesToCurrentlyShow[i], typeof(GameObject));
                GUI.color = Color.white;
            }
            else if (RotatableObstacles.Contains(AllObstaclesToCurrentlyShow[i]))
            {
                GUI.color = Color.cyan;
                EditorGUILayout.ObjectField(AllObstaclesToCurrentlyShow[i].name, AllObstaclesToCurrentlyShow[i], typeof(GameObject));
                GUI.color = Color.white;
            }
            else if (EnemieObstacles.Contains(AllObstaclesToCurrentlyShow[i]))
            {
                GUI.color = Color.red;
                EditorGUILayout.ObjectField(AllObstaclesToCurrentlyShow[i].name, AllObstaclesToCurrentlyShow[i], typeof(GameObject));
                GUI.color = Color.white;
            }
            else if (AvatarPrefabs.Contains(AllObstaclesToCurrentlyShow[i]))
            {
                GUI.color = Color.yellow;
                EditorGUILayout.ObjectField(AllObstaclesToCurrentlyShow[i].name, AllObstaclesToCurrentlyShow[i], typeof(GameObject));
                GUI.color = Color.white;
            }
            else if (FloorPrefabs.Contains(AllObstaclesToCurrentlyShow[i]))
            {
                GUI.color = Color.Lerp(Color.white, Color.blue, 0.4f);
                EditorGUILayout.ObjectField(AllObstaclesToCurrentlyShow[i].name, AllObstaclesToCurrentlyShow[i], typeof(GameObject));
                GUI.color = Color.white;
            }
            else if (ScenePrefabs.Contains(AllObstaclesToCurrentlyShow[i]))
            {
                GUI.color = Color.Lerp(Color.magenta, Color.red, 0.5f);
                EditorGUILayout.ObjectField(AllObstaclesToCurrentlyShow[i].name, AllObstaclesToCurrentlyShow[i], typeof(GameObject));
                GUI.color = Color.white;
            }
            else if (SpecialPrefabs.Contains(AllObstaclesToCurrentlyShow[i]))
            {
                GUI.color = Color.Lerp(Color.red, Color.yellow, 0.4f);
                EditorGUILayout.ObjectField(AllObstaclesToCurrentlyShow[i].name, AllObstaclesToCurrentlyShow[i], typeof(GameObject));
                GUI.color = Color.white;
            }

            else
            {
                EditorGUILayout.ObjectField(AllObstaclesToCurrentlyShow[i].name, AllObstaclesToCurrentlyShow[i], typeof(GameObject));
            }
        }

        EditorGUILayout.EndScrollView();

        EditorGUILayout.EndVertical();

        
        //The GUI Checkbox section (BEGINS):
        GUILayout.BeginArea(new Rect(5, 350, 500, 250));
        _ShowPrefabCheckBoxes = EditorGUILayout.Foldout(_ShowPrefabCheckBoxes, "Toggle prefab folders");

        EditorGUILayout.BeginHorizontal();
        if (_ShowPrefabCheckBoxes)
        {
            GUI.color = Color.green;
            _ShowMovableObstacles = EditorGUILayout.ToggleLeft("Show Movable", _ShowMovableObstacles, GUILayout.MaxWidth(100), GUILayout.Width(100));
            GUI.color = Color.white;
            GUI.color = Color.Lerp(Color.white, Color.red, 0.5f);
            _ShowNonMovableObstacles = EditorGUILayout.ToggleLeft("Show Non-Movable", _ShowNonMovableObstacles, GUILayout.MaxWidth(130), GUILayout.Width(130));
            GUI.color = Color.white;
            GUI.color = Color.cyan;
            _ShowRotatableObstacles = EditorGUILayout.ToggleLeft("Show Rotatable", _ShowRotatableObstacles, GUILayout.MaxWidth(110), GUILayout.Width(110));
            GUI.color = Color.white;
            GUI.color = Color.red;
            _ShowEnemieObstacles = EditorGUILayout.ToggleLeft("Show Enemies", _ShowEnemieObstacles, GUILayout.MaxWidth(130), GUILayout.Width(130));
            GUI.color = Color.white;

        }

        EditorGUILayout.EndHorizontal();
        EditorGUILayout.BeginHorizontal();
        if (_ShowPrefabCheckBoxes)
        {
            GUI.color = Color.yellow;
            _ShowAvatarPrefabs = EditorGUILayout.ToggleLeft("Show Avatars", _ShowAvatarPrefabs, GUILayout.MaxWidth(100), GUILayout.Width(100));
            GUI.color = Color.white;
            GUI.color = Color.Lerp(Color.white, Color.blue, 0.4f);
            _ShowFloorPrefabs = EditorGUILayout.ToggleLeft("Show floors", _ShowFloorPrefabs, GUILayout.MaxWidth(85), GUILayout.Width(85));
            GUI.color = Color.white;
            GUI.color = Color.Lerp(Color.magenta, Color.red, 0.5f);
            _ShowScenePrefabs = EditorGUILayout.ToggleLeft("Show scene stuff", _ShowScenePrefabs, GUILayout.MaxWidth(120), GUILayout.Width(120));
            GUI.color = Color.white;
            GUI.color = Color.Lerp(Color.red, Color.yellow, 0.4f);
            _ShowSpecialPrefabs = EditorGUILayout.ToggleLeft("Show specials", _ShowSpecialPrefabs, GUILayout.MaxWidth(130), GUILayout.Width(130));
            GUI.color = Color.white;

        }

        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        if (_ShowPrefabCheckBoxes)
        {
            _ShowALLPrefabs = EditorGUILayout.ToggleLeft("Show ALL in recources", _ShowALLPrefabs, GUILayout.MaxWidth(150), GUILayout.Width(150));
        }

        EditorGUILayout.EndHorizontal();
        GUILayout.EndArea();
        //The GUI Checkbox section (ENDS)

        //THE GUI label that, in text shows what item is selected:
        GUILayout.BeginArea(new Rect(350, 5, 350, 20));

        if (Selection.activeGameObject != null)
        {
            GUILayout.Label("Selected item: " + Selection.activeGameObject.name);
        }
        else
        {
            GUILayout.Label("Currently no selected item");
        }
        GUILayout.EndArea();


        //The GUI 3D preview window of the currently selected item:
        GUILayout.BeginArea(new Rect(315, 30, 250, 250));
        if (Selection.activeGameObject != null)
        {
            if (gameObjectEditor == null)
            {
                gameObjectEditor = Editor.CreateEditor(Selection.activeGameObject);
            }
            else if (gameObjectEditor.target != Selection.activeGameObject)
            {
                gameObjectEditor = Editor.CreateEditor(Selection.activeGameObject);
            }

            gameObjectEditor.OnPreviewGUI(GUILayoutUtility.GetRect(250, 270, 250, 250), EditorStyles.whiteLabel);
        }
        GUILayout.EndArea();

        
        //The GUI "create prefab button":
        GUILayout.BeginArea(new Rect(375, 285, 130, 50));

        if (GUILayout.Button("Create prefab!"))
        {
            if (Selection.activeGameObject != null)
            {
                GameObject newGameObject = (GameObject)Instantiate(Selection.activeGameObject, _LocationToPlacePrefab, Quaternion.identity);
                newGameObject.name = Selection.activeGameObject.name;
            }
        }

        GUILayout.EndArea();

        //The GUI vector3-field where you decide location to place a prefab:
        GUILayout.BeginArea(new Rect(375, 310, 130, 50));
        _LocationToPlacePrefab = EditorGUILayout.Vector3Field("Place prefab at:", _LocationToPlacePrefab);

        GUILayout.EndArea();

    }

}



