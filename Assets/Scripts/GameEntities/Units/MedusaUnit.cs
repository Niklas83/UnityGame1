using System;
using UnityEngine;
using System.Collections;

public class MedusaUnit : BaseUnit
{
	public override int LayerMask { get { return (int)(Layer.Air | Layer.Ground); } }

    public bool ShootRight = true;
    public bool ShootLeft = true;
    public bool ShootUp = true;
    public bool ShootDown = true;

	public override bool CanWalkOver { get { return false; } }

	public override bool CanWalkOn(string incomingUnitTag)
	{
	    return CanWalkOver;
	}

    private bool StartedToShoot = false;    //If this is set, the raycasting will be done slower to enable flame animation
    private bool StartedShootAtemptSequence = false;      //To prevent the Update to constantly spam (could be given a more suitable name)
    private MedusaRay MedusaRayScript;
    private LineRenderer TheAnimatedBeam;               //The visual flame that travels towards the crate or player
    private GameObject[] PlayerAndObsticle;             //The value that gets returned from the medusa ray

    //Blasting
    private float IncrementalLength = 0;
    private GameObject PlayerGameObject;
    private GameObject ObsticleGameObject;
    private float DistanceToPlayer;         //Distance to player from from medusa
    private float DistanceToObsticle;       //Distance to box/tower from from medusa
    private Vector3 PlayerLocationToShotAt;
    private Vector3 BoxLocationToShotAt = new Vector3();
    private bool ExecuteBlastSequence = false;
    private bool StartedBurningBabySequence = false;            //Started shooting at a player
    private bool StartedBurningBoxSequence = false;         //Started shooting at a box
    private Vector3 PreviousPlayerLocationToShotAt; //If these previous locations are the same as the current location atempting to shot at it will not fire
    private Vector3 PreviousBoxLocationToShotAt;    //If these previous locations are the same as the current location atempting to shot at it will not fire
    private Mover BoxUnitMover;               //Script to check if the unit is currently moving
    private bool MissingMoverScript = false;    //If the the "Box"/Obsticle is missing a mover script it shall blast anyway
    private bool HasShooten = false;            //If the medusaunit has fired this is set so it dosent keep fireing

    private void SetStartedToShoot(bool isShooting)          //Used in medusa ray script called with SendMessage method to tell that a blast has been initialized
    {
        StartedToShoot = isShooting;
    }

    public void Start()
    {

        MedusaRayScript = GetComponent<MedusaRay>();
        TheAnimatedBeam = this.gameObject.GetComponentInChildren<LineRenderer>();
	}
	
	void Update () {
        if (StartedShootAtemptSequence == false)
	    {
            StartedShootAtemptSequence = true;
	        StartCoroutine(StartShooting(ShootRight, ShootLeft, ShootUp, ShootDown));
	    }
	}

    public IEnumerator StartShooting(bool shootRight, bool shootLeft, bool shootUp, bool shootDown)
    {
        if (StartedToShoot == true && ExecuteBlastSequence!= true)
        {
            if (TheAnimatedBeam.enabled == false)
            {
                TheAnimatedBeam.enabled = true;
            }
            ExecuteBlast();
                 
        }
        else if (StartedBurningBabySequence != true && StartedBurningBoxSequence != true)
        {
            ExecuteBlastSequence = false;
           PlayerAndObsticle = MedusaRayScript.Blast(shootRight, shootLeft, shootUp, shootDown);

            if (PlayerAndObsticle == null || PlayerAndObsticle[0] == null)
            {
                HasShooten = false;
            }

        }
        else if (StartedBurningBabySequence == true)
        {
            StartCoroutine(BurnBabyBurn());
        }
        else if (StartedBurningBoxSequence == true)
        {
            StartCoroutine(BurnBoxBurn());
        }

        StartedShootAtemptSequence = false;
        yield return 0;
    }

    public void ExecuteBlast()
    {
        ExecuteBlastSequence = true;    
        if (PlayerAndObsticle != null)
        {
            if (PlayerAndObsticle[0] != null && PlayerAndObsticle[0].tag.Equals(UnitTypesEnum.Player.ToString()))       //checks if first object is of player type
            {
                PlayerGameObject = PlayerAndObsticle[0];
                DistanceToPlayer = Vector3.Distance(this.transform.position, PlayerGameObject.transform.position);      
            }
            else
            {
                HasShooten = false;
            }

            if (PlayerGameObject != null && PlayerAndObsticle[1] != null && (PlayerAndObsticle[1].tag.Equals(UnitTypesEnum.Box.ToString()) || PlayerAndObsticle[1].tag.Equals(UnitTypesEnum.ProjectileShooter.ToString())))
            {
                ObsticleGameObject = PlayerAndObsticle[1];
                BoxUnitMover = ObsticleGameObject.GetComponent<Mover>();
                if (BoxUnitMover == null)
                {
                    MissingMoverScript = true;
                }
                DistanceToObsticle = Vector3.Distance(this.transform.position, ObsticleGameObject.transform.position);
            }
        }
        else
        {
            HasShooten = false;
        }
        
        if (PlayerAndObsticle != null && PlayerAndObsticle[1] != null)
        {
            if (DistanceToObsticle > DistanceToPlayer)
            {
                PlayerGameObject.SendMessage("MakePlayerFrozen");
                PlayerLocationToShotAt = this.transform.position - PlayerGameObject.transform.position;       //Target is player
            }
            else
            {
                BoxLocationToShotAt = this.transform.position - ObsticleGameObject.transform.position;     //Target is box
            }
        }
        else if (PlayerAndObsticle != null)
        {
            PlayerGameObject.SendMessage("MakePlayerFrozen");
            PlayerLocationToShotAt = this.transform.position - PlayerGameObject.transform.position;
            PreviousBoxLocationToShotAt = new Vector3();
        }

        if (PlayerAndObsticle != null && (PlayerAndObsticle[1] == null || BoxLocationToShotAt == new Vector3() || DistanceToObsticle > DistanceToPlayer)  && (HasShooten == false || PreviousPlayerLocationToShotAt != PlayerLocationToShotAt))
        {
            int roundedPlayerX = (int)Math.Round(PlayerLocationToShotAt.x, 0);
            int roundedPlayerZ = (int)Math.Round(PlayerLocationToShotAt.z, 0);

            int roundedBoxX = (int)Math.Round(BoxLocationToShotAt.x, 0);
            int roundedBoxZ = (int)Math.Round(BoxLocationToShotAt.z, 0);

            PreviousPlayerLocationToShotAt = new Vector3(roundedPlayerX, PlayerLocationToShotAt.y, roundedPlayerZ);        //saves the location that was just fired at

            PreviousBoxLocationToShotAt = new Vector3(roundedBoxX, BoxLocationToShotAt.y, roundedBoxZ);        //saves the location that was just fired at

            StartCoroutine(BurnBabyBurn()); //Sends out the flame from the medusa statue towards the player
        }
                                                                                                                                                                                        //REMOVE PREVIOUS CHECK IF WE DONT WANT TO SHOOT AGAIN WHEN BOXED HAS MOVED
        else if (PlayerAndObsticle != null && PlayerAndObsticle[1] != null && ((BoxUnitMover != null && BoxUnitMover.IsMoving == false) || MissingMoverScript == true) && (HasShooten == false || PreviousBoxLocationToShotAt != BoxLocationToShotAt))
        {
            int roundedX = (int)Math.Round(PlayerLocationToShotAt.x, 0);
            int roundedZ = (int)Math.Round(PlayerLocationToShotAt.z, 0);
            
            int roundedBoxX = (int)Math.Round(BoxLocationToShotAt.x, 0);
            int roundedBoxZ = (int)Math.Round(BoxLocationToShotAt.z, 0);

            PreviousPlayerLocationToShotAt = new Vector3(roundedX, PlayerLocationToShotAt.y, roundedZ);        //saves the location that was just fired at

            PreviousBoxLocationToShotAt = new Vector3(roundedBoxX, BoxLocationToShotAt.y, roundedBoxZ);        //saves the location that was just fired at

            StartCoroutine(BurnBoxBurn()); //Sends out the flame from the medusa statue
        }
    }

    public IEnumerator BurnBabyBurn()
    {
        StartedBurningBabySequence = true;

        var roundedIncrementalLength = IncrementalLength * 0.95;         //As of the raycasting hitting the collider, some adjustment regardinga ranges need to be made to make it shoot every time
       
        //Shoots down on PLAYER
        if ((PlayerLocationToShotAt.x > -0.2 && PlayerLocationToShotAt.x < 0.2) && PlayerLocationToShotAt.z > 0)
        {
            if (roundedIncrementalLength >= (PlayerLocationToShotAt.z + 1) * -1)
            {
                TheAnimatedBeam.SetPosition(0, new Vector3(PlayerLocationToShotAt.x * -1, 0, IncrementalLength));
                IncrementalLength = this.IncrementalLength - 1f;
            }
            else
            {
                PlayerGameObject.SendMessage("DestroyUnit");     //Destroys the player
            }
            yield return new WaitForSeconds(0.25f);
        }

        //Shoots up on PLAYER
        if ((PlayerLocationToShotAt.x > -0.2 && PlayerLocationToShotAt.x < 0.2) && PlayerLocationToShotAt.z < 0)
        {
            if (roundedIncrementalLength <= (PlayerLocationToShotAt.z - 1) * -1)
            {
                TheAnimatedBeam.SetPosition(0, new Vector3(PlayerLocationToShotAt.x * -1, 0, IncrementalLength));
                IncrementalLength = this.IncrementalLength + 1f;
            }
            else
            {
                PlayerGameObject.SendMessage("DestroyUnit");     //Destroys the player
            }
            yield return new WaitForSeconds(0.25f);
        }

        //Shoots right on PLAYER
        if ((PlayerLocationToShotAt.z > -0.2 && PlayerLocationToShotAt.z < 0.2) && PlayerLocationToShotAt.x < 0)
        {
            if (roundedIncrementalLength <= (PlayerLocationToShotAt.x - 1) * -1)
            {
                TheAnimatedBeam.SetPosition(0, new Vector3(IncrementalLength, 0, PlayerLocationToShotAt.z * -1));
                IncrementalLength = this.IncrementalLength + 1f;
            }
            else
            {
                PlayerGameObject.SendMessage("DestroyUnit");     //Destroys the player
            }
            yield return new WaitForSeconds(0.25f);
        }

        //Shoots left on PLAYER
        if ((PlayerLocationToShotAt.z > -0.2 && PlayerLocationToShotAt.z < 0.2) && PlayerLocationToShotAt.x > 0)
        {
            if (roundedIncrementalLength >= (PlayerLocationToShotAt.x + 1) * -1)
            {
                TheAnimatedBeam.SetPosition(0, new Vector3(IncrementalLength, 0, PlayerLocationToShotAt.z * -1));
                IncrementalLength = this.IncrementalLength - 1f;
            }
            else
            {
                PlayerGameObject.SendMessage("DestroyUnit");     //Destroys the player
            }
            yield return new WaitForSeconds(0.25f);
        }
        
        HasShooten = true; 

        ResetFields();
    }



    public IEnumerator BurnBoxBurn()
    {
        StartedBurningBoxSequence = true;

        //Shoots down on BOX
        if ((BoxLocationToShotAt.x > -0.2 && BoxLocationToShotAt.x < 0.2) && BoxLocationToShotAt.z > 0)
        {
            if (IncrementalLength > (BoxLocationToShotAt.z + 1) * -1)
            {
                TheAnimatedBeam.SetPosition(0, new Vector3(BoxLocationToShotAt.x * -1, 0, IncrementalLength));
                IncrementalLength = this.IncrementalLength - 1f;
            }

            //Place box damage code here (if we want that)

            yield return new WaitForSeconds(0.25f);
        }

        //Shoots up on BOX
        if ((BoxLocationToShotAt.x > -0.2 && BoxLocationToShotAt.x < 0.2) && BoxLocationToShotAt.z < 0)
        {
            if (IncrementalLength < (BoxLocationToShotAt.z ) * -1)
            {
                TheAnimatedBeam.SetPosition(0, new Vector3(BoxLocationToShotAt.x * -1, 0, IncrementalLength));
                IncrementalLength = this.IncrementalLength + 1f;
            }

            //Place box damage code here (if we want that)

            yield return new WaitForSeconds(0.25f);
        }

        //Shoots left on BOX
        if ((BoxLocationToShotAt.z > -0.2 && BoxLocationToShotAt.z < 0.2) && BoxLocationToShotAt.x > 0)
        {
            if (IncrementalLength > (BoxLocationToShotAt.x + 1) * -1)
            {
                TheAnimatedBeam.SetPosition(0, new Vector3(IncrementalLength, 0, BoxLocationToShotAt.z * -1));
                IncrementalLength = this.IncrementalLength - 1f;
            }

            //Place box damage code here (if we want that)

            yield return new WaitForSeconds(0.25f);
        }

        //Shoots right on BOX
        if ((BoxLocationToShotAt.z > -0.2 && BoxLocationToShotAt.z < 0.2) && BoxLocationToShotAt.x < 0)
        {
            if (IncrementalLength < (BoxLocationToShotAt.x) * -1)
            {
                TheAnimatedBeam.SetPosition(0, new Vector3(IncrementalLength, 0, BoxLocationToShotAt.z * -1));
                IncrementalLength = this.IncrementalLength + 1f;
            }

            //Place box damage code here (if we want that)

            yield return new WaitForSeconds(0.25f);
        }


        HasShooten = true;
        ResetFields();
    }

    private void ResetFields()      //Resets all the values to prepare for the next blast
    {
        TheAnimatedBeam.enabled = false;
        IncrementalLength = 0;
        TheAnimatedBeam.SetPosition(0, new Vector3());
        StartedBurningBabySequence = false;
        StartedBurningBoxSequence = false;
        ExecuteBlastSequence = false;
        PlayerGameObject = null;
        ObsticleGameObject = null;
        PlayerAndObsticle = null;
        DistanceToObsticle = 10000;                 
        DistanceToPlayer = 10000;
        PlayerLocationToShotAt = new Vector3();
        BoxLocationToShotAt = new Vector3();
        SetStartedToShoot(false);
        MissingMoverScript = false;
    }



































}
