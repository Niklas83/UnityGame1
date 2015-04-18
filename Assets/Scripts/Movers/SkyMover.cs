using UnityEngine;
using System.Collections;

public class SkyMover : MonoBehaviour
{

    public float XRotationSpeed = 0.02f;

    public float YRotationSpeed = 0.02f;

    public float ZRotationSpeed = 0.02f;

    private Transform _trans;

	// Use this for initialization
	void Start ()
	{
	    _trans = GetComponent<Transform>();
	}
	
	// Update is called once per frame
	void Update ()
	{
	    RotateSky();
	}


    public void RotateSky()
    {
        _trans.Rotate(XRotationSpeed, YRotationSpeed, ZRotationSpeed);
    }


}
