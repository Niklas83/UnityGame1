using System;
using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

public class ObstacleGenerator : ScriptableWizard
{
    public GameObject Selected;
    //public GameObject[] MovableBoxes = Resources.LoadAll("Assets/Prefabs/Boxes/Movables/") as GameObject[];

    public GameObject[] MovableBoxesTemp;

    public AssetPreview selectedIMage;
    //public Editor testtest = new Editor();

    public Texture2D PreviewTexture;

    //public List<GameObject> MovableBoxes;

    //"Assets/Prefabs/Boxes/Movables/Crate.prefab"

    [MenuItem("Custom/Generate obstacle")]
    static void CreateWizard()
    {
        ScriptableWizard.DisplayWizard("Generate obstacle", typeof(ObstacleGenerator), "Generate");
    }

    
    
    
    void OnWizardUpdate()
    {

        MovableBoxesTemp = Resources.LoadAll<GameObject>("Boxes");
            
        
            //AssetDatabase.LoadAllAssetsAtPath("Assets/Resources/Boxes/") as GameObject[];
               // Resources.LoadAll<GameObject>("Boxes");
                
    }

    public void Update()
    {
        if (Selection.activeGameObject != null)
        {
            Selected = Selection.activeGameObject;

            //docs.unity3d.com/Documentation/ScriptReference/AssetPreview.html
            AssetPreview.GetAssetPreview(Selected);

           var pic = AssetDatabase.GetCachedIcon(AssetDatabase.GetAssetPath(Selected));
            var tetesttest = 1;

            var piccz = AssetPreview.GetAssetPreview(Selected);

            var tetesttedfgsfsdst = 1;


        }

        else
        {
            Selected = null;
        }
    }

    void OnGUI()
    {
        var piccz = AssetPreview.GetAssetPreview(Selected);
        EditorGUI.DrawPreviewTexture(new Rect(122f, 150f, 150f, 150f), piccz);
    }

    //Skapa en on wizard other? så att den inte stängs då man skapar nått


    // Det som händer då man trycker "Generate"
    void OnWizardCreate()
    {
        if (Selected != null)
        {
            GameObject newObject;
            newObject = (GameObject) EditorUtility.InstantiatePrefab(Selected);
            newObject.transform.position = new Vector3(7, 4, 7);
            newObject.transform.rotation = newObject.transform.rotation;
            //newObject.transform.parent = newObject.transform.parent;
        }
        else
        {
            Debug.Log("You must have a selected prefab (the one in the inspectator window)");
        }
    }
}
