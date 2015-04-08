using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MedusaUnit : BaseUnit
{
    //This weight is compared to the floors durability, if the weight > durability you fall through the floor
    public int CurrentWeight = 50;
    public override int Weight { get { return CurrentWeight; } }

    //Check this TRUE if you want the unit to be breakable by medusarays and other projectiles
    public bool BreaksByProjectile = false;
    public override bool BreaksByProjectileAndMedusa { get { return BreaksByProjectile; } }

	public GameObject beamPrefab;
	
	public bool shootRight = true;
	public bool shootLeft = true;
	public bool shootUp = true;
	public bool shootDown = true;
	
	public override int LayerMask { get { return (int)(Layer.Air | Layer.Ground); } }
	
	public override bool CanWalkOver { get { return false; } }
	public override bool CanWalkOn(string incomingUnitTag)
	{
	    return CanWalkOver;
	}

    public GameObject DeathBeamEffect;
    private bool _deathBeamHasBeenInitialized;

 //   private PlaygroundPresetLaserC ;

	private List<MedusaRay> _rays = new List<MedusaRay>();

    public void Start()
    {
        _deathBeamHasBeenInitialized = false;

    	if (shootRight)
			_rays.Add(new MedusaRay(Vector3.right, beamPrefab, this));
		if (shootLeft)
			_rays.Add(new MedusaRay(Vector3.left, beamPrefab, this));
		if (shootUp)
			_rays.Add(new MedusaRay(Vector3.forward, beamPrefab, this));
		if (shootDown)
			_rays.Add(new MedusaRay(Vector3.back, beamPrefab, this));
	}
	
	void Update () 
	{
        if (_deathBeamHasBeenInitialized == false)
		{
		    for (int i = 0; i < _rays.Count; i++) 
            {
			    GameObject hitObject = _rays[i].Blast();
			    Hit(hitObject);
            }
		}
	}
	
	public void Hit(GameObject hitObject) 
	{
		if (hitObject != null && hitObject.GetComponent<AvatarUnit>() != null)
		{
		    InstantiateBeam(hitObject);
            hitObject.SendMessage("KillAvatar", SendMessageOptions.DontRequireReceiver);		        
		}

	    
        if (hitObject != null && hitObject.GetComponent<BaseUnit>() != null && hitObject.GetComponent<BaseUnit>().BreaksByProjectileAndMedusa)
        {
            // InstantiateBeam(hitObject);       TODO: måste lägga till så att beamen försvinner efter den förstört ett object samt göra så att _deathBeamHasBeenInitialized blir false igen
            hitObject.GetComponent<BaseUnit>().DestroyUnit();
        }
	}

    private void InstantiateBeam(GameObject hitObject)
    {
        _deathBeamHasBeenInitialized = true;

        Vector3 hitLocation = new Vector3((float)Math.Round((decimal)hitObject.transform.position.x), 1f, (float)Math.Round((decimal)hitObject.transform.position.z));

        Vector3 shootLocation = new Vector3(transform.position.x, 1f, transform.position.z);

        Vector3 instantiationPosition = new Vector3(transform.position.x, 1.5f, transform.position.z);

        Vector3 RelativePosition = hitLocation - shootLocation;

        if (RelativePosition.x < 0)
        {
            instantiationPosition += new Vector3(-0.5f, 0f, 0f);
        }
        else if (RelativePosition.x > 0)
        {
            instantiationPosition += new Vector3(0.5f, 0f, 0f);
        }
        else if (RelativePosition.z < 0)
        {
            instantiationPosition += new Vector3(0f, 0f, -0.5f);
        }
        else if (RelativePosition.z > 0)
        {
            instantiationPosition += new Vector3(0f, 0f, 0.5f);
        }

    //    PlaygroundPresetLaserC beamSettings = DeathBeamEffect.GetComponent<PlaygroundPresetLaserC>();

    //    beamSettings.particleCount = 300;

        Instantiate(DeathBeamEffect, instantiationPosition, Quaternion.LookRotation(hitLocation - shootLocation));
    }
}
