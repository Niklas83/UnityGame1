using UnityEngine;
using UnityEditor;
using System.Collections;

// CopyComponents - by Michael L. Croswell for Colorado Game Coders, LLC
// March 2010

//Modified by Kristian Helle Jespersen
//June 2011
public class ReplaceGameObjects : ScriptableWizard
{
	public bool copyValues = true;
	public GameObject NewType;
	public GameObject[] OldObjects;

    public bool SetRandomRotation = true;
	
	[MenuItem("Custom/Replace GameObjects")]
	static void CreateWizard()
	{
		ScriptableWizard.DisplayWizard("Replace GameObjects", typeof(ReplaceGameObjects), "Replace");
    }

    void OnWizardCreate()
    {
        //Transform[] Replaces;
        //Replaces = Replace.GetComponentsInChildren<Transform>();

        

        foreach (GameObject go in OldObjects)
        {
            GameObject newObject;

            newObject = (GameObject)PrefabUtility.InstantiatePrefab(NewType);
            newObject.transform.position = go.transform.position;
            newObject.transform.parent = go.transform.parent;
            if (SetRandomRotation)
            {
                newObject.transform.Rotate(0, GetRandomAxisRotation(), 0);
            }
            else
            {
                newObject.transform.rotation = go.transform.rotation;
            }
            DestroyImmediate(go);
        }
    }

    private float GetRandomAxisRotation()
    {
        int axis = Random.Range(1, 5);

        if (axis == 1)
        {
            return 0f;
        }
        else if (axis == 2)
        {
            return 90f;
        }
        else if (axis == 3)
        {
            return 270f;
        }
        else if (axis == 4)
        {
            return 0f;
        }
        else
        {
            return 0;
        }
    }
}