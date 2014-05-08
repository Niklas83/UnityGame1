using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NodeWorldMap : MonoBehaviour
{

    private List<MapNode> _mapNodes; 




	// Use this for initialization
	void Start () {
	    _mapNodes = new List<MapNode>();
        //Lägg in alla map nodes som är childs till detta object

	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
