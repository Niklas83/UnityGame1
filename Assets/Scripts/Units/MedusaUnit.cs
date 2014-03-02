using UnityEngine;
using System.Collections;

public class MedusaUnit : BaseUnit
{

    public bool ShootRight = true;
    public bool ShootLeft = true;
    public bool ShootUp = true;
    public bool ShootDown = true;


    


        public override bool CanWalkOver { get { return false; } }

        public override bool CanWalkOn(string incomingUnitTag)
        {
            return CanWalkOver;
        }

        //private ProjectileMover mMover;


    private MedusaRay MedusaRayScript;

    public void Start()
	{
        MedusaRayScript = GetComponent<MedusaRay>();
	}
	
	// Update is called once per frame
	void Update () {
        MedusaRayScript.Blast(ShootRight, ShootLeft, ShootUp, ShootDown);
	}
}
