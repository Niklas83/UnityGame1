using System.Linq;
using UnityEngine;
using UnityEditor;
public class ObstacleGeneratorWindow : EditorWindow
{

    GameObject[] MovableObstacles;
    GameObject[] NoneMovableObstacles;

    GameObject[] AllObstaclesToCurrentlyShow;


    GameObject Selected;

    private bool ShowMovableObstacles;      //ska sättas som checkbox så att du kan avgöra vad som ska synas

    private bool ShowNonMovableObstacles;      // EJ ANVÄND ÄNNU   ska sättas som checkbox så att du kan avgöra vad som ska synas

    public GameObject GameObjectList;

    [MenuItem("Window/ObstacleGenerator")]
    static void Init()
    {
        ObstacleGeneratorWindow window = (ObstacleGeneratorWindow)EditorWindow.GetWindow(typeof(ObstacleGeneratorWindow));
    }

    void OnGUI()
    {

        ShowMovableObstacles = EditorGUILayout.Toggle("Show Movable", ShowMovableObstacles);
       
        NoneMovableObstacles = Resources.LoadAll<GameObject>("Boxes/Static");

        if (ShowMovableObstacles == true)
        {
            MovableObstacles = Resources.LoadAll<GameObject>("Boxes/Movables");
        }

        if (ShowMovableObstacles == true)
        {
            AllObstaclesToCurrentlyShow = MovableObstacles.Union(NoneMovableObstacles).ToArray();
        }
        else
        {
            AllObstaclesToCurrentlyShow = NoneMovableObstacles;
        }

        //GameObjectList = EditorGUILayout.ObjectField(MovableBoxes[0], typeof(GameObject), true) as GameObject;

        foreach (var item in AllObstaclesToCurrentlyShow)
        {
            EditorGUILayout.ObjectField(item.name, item, typeof(GameObject));
        }

        int numberOfItems = AllObstaclesToCurrentlyShow.Count();

        float spaceFromTop = 30f + numberOfItems*20f;

        if (Selection.activeGameObject != null)
        {
            Selected = Selection.activeGameObject;
            EditorGUI.DrawPreviewTexture(new Rect(5f, spaceFromTop, 175f, 175f),
                AssetPreview.GetAssetPreview(Selected));
        }


        //test startar


        Rect CreateButton = EditorGUILayout.BeginHorizontal("Button");
        if (GUI.Button(CreateButton, GUIContent.none))
            Debug.Log("Go here");
        GUILayout.Label("Create Prefab!");
        //GUILayout.Label("So am I");
        EditorGUILayout.EndHorizontal();




        //test slutar
    }
}



