using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MiscHelperMethods : MonoBehaviour {

    /// <summary>
    /// Searches through a gameobject if it has one named equal to "name"
    /// </summary>
    /// <param name="parent"></param>
    /// <param name="name"></param>
    /// <returns></returns>
    public static GameObject FindObject(GameObject parent, string name)
    {
        List<Transform> ChildObjects = new List<Transform>();

        for (int i = 0; i < parent.transform.childCount; i++)
        {
            ChildObjects.Add(parent.transform.GetChild(i));
        }

        foreach (Transform t in ChildObjects)
        {
            if (t.name == name)
            {
                return t.gameObject;
            }
        }
        return null;
    }
}
