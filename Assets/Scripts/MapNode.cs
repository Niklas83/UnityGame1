using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;
using System.Collections;

public enum MapType
{
    NotSet,
    Graveyard,
    Desert,
    Water,
    Ice,
    Forest,
    Hell,
    Heaven,
    Halloween,
    Castle
}



public class MapNode : MonoBehaviour
{
    public bool IsFirstLevel;

    //public string SceneName;      Need to add this functionallity to scenetransition

    public MapType MapType;     //The maptype of the level

    public int SceneNr;         //Number of the scene from file->buildsetting (the menu in unity)

    public bool IsActive;       //If the level is selectable

    public bool IsCleared;      //If the level has been cleared CHECK THIS IN SOME SAVE FILE

    public int WorldGoldValue;     //How many "World gold" a map is worth solving, to leave one world area you need to have a amount of gold to enter the next world

    public int MinimumGoldToAccess;     //How much world gold a play need to access this level

    public List<GameObject> BranchingNodes;       //Contains a list of all neighbouring nodes, used to check if the current node is active by checking 
                                            //if their is a active node linked to it, also used to create line renderer between the nodes (needs to be added manually)


    private bool _isSeleced = false;            //Gets set when a level is selected


    //Linerenderer fields starts

    //private LineRenderer _lineRenderer;
    //private float _counter;
    //private float _distance;

    public Material ActiveLineMaterial;
    public Material InactiveLineMaterial;

    //public float LineDrawSpeed =6f;


    //Linerenderer fields ends

    private void Start()
    {

        SetGraphicalConnectionsBetweenBranches();
        
    //Initiate iteration to set maps active
        if (IsFirstLevel)
        {
            IsActive = true;
            SetGraphicalConnectionsBetweenBranches();
            SetBranchNodesActive();
        }

        if (IsActive && IsCleared)
        {
            this.gameObject.GetComponent<Renderer>().materials[0].SetColor("_TintColor", new Color(0.141f, 1.000f, 0.811f, 0.502f));
        }
        else if (IsActive && !IsCleared)
        {
            this.gameObject.GetComponent<Renderer>().materials[0].SetColor("_TintColor", new Color(0.147f, 0.259f, 1.000f, 0.502f));
        }
        else
        {
            this.gameObject.GetComponent<Renderer>().materials[0].SetColor("_TintColor", new Color(1.000f, 0.297f, 0.102f, 0.502f));
        }


        
    }

    private void Update()
    {
        if (Input.GetMouseButtonUp(0))
        {
            SelectNode();

            if (_isSeleced)
            {
                LoadMap();
            }
        }

        
        
    }

    public void SetGraphicalConnectionsBetweenBranches()
    {
        for (int i = 0; i < BranchingNodes.Count; i++)
        {
            GameObject BranchNode = BranchingNodes[i];

            GameObject LineRendererHolder = new GameObject("LineRenderHolder");

            LineRenderer _lineRenderer = LineRendererHolder.AddComponent<LineRenderer>();

            LineRendererHolder.transform.parent = this.gameObject.transform;


           // _lineRenderer.material = new Material(Shader.Find("Particles/Additive"));

            if (IsCleared)
            {
                _lineRenderer.material = ActiveLineMaterial;
                _lineRenderer.SetColors(Color.red, Color.yellow);
            }
            else
            {
                _lineRenderer.material = InactiveLineMaterial;
                _lineRenderer.SetColors(Color.gray, Color.gray);
            }
            _lineRenderer.SetPosition(0, this.gameObject.transform.position);
            _lineRenderer.SetWidth(0.2f, 0.2f);


            _lineRenderer.SetPosition(1, BranchNode.transform.position);
            


        }
    }

    public void SetBranchNodesActive()
    {

        if (IsCleared)
        {
            for (int i = 0; i < BranchingNodes.Count; i++)
                {
                    GameObject BranchNode = BranchingNodes[i];
                    MapNode BranchNodeScript = BranchNode.GetComponent<MapNode>();

                    BranchNodeScript.IsActive = true;

                    BranchNodeScript.SetBranchNodesActive();
                }
        }
        else
        {
            //Play some animation to highlight that this is not cleared
        }
    }




	


    private void LoadMap()
    {
        if(IsActive == true)
        {
            SceneTransition st = Helper.Find<SceneTransition>("SceneTransition");

            if (SceneNr != 0)
            {
                st.NextScene(SceneNr);
            }

            //TODO: Add the avalility in SceneTransition that you can load a map from the scene name aswell
            //else if (SceneName != null && SceneName != "")
            //{
            //    st.NextScene(SceneName);
            //}
        }
    }



    private void SelectNode()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity))
        {
            if (hit.transform.gameObject == this.gameObject)
            {
                _isSeleced = true;
            }
            else
            {
                _isSeleced = false;
            }
        }
    }



    //private void CheckIfActive()
    //{
    //    //TODO: Kolla i sav-filen om denna är aktiv
    //    this.IsActive = true;
    //}
}
