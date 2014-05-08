using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MedusaUnit : BaseUnit
{
    //This weight is compared to the floors durability, if the weight > durability you fall through the floor
    public int CurrentWeight = 50;
    public override int Weight { get { return CurrentWeight; } }

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

	private List<MedusaRay> _rays = new List<MedusaRay>();

    public void Start()
    {
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
		for (int i = 0; i < _rays.Count; i++) {
			GameObject hitObject = _rays[i].Blast();
			Hit(hitObject);
		}
	}
	
	public void Hit(GameObject hitObject) 
	{
		if (hitObject != null && hitObject.GetComponent<AvatarUnit>() != null) {
			hitObject.SendMessage("KillAvatar", SendMessageOptions.DontRequireReceiver);
		}
	}
}
