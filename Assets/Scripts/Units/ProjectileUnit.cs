using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class ProjectileUnit : BaseUnit 
{
	public override bool CanWalkOn { get { return true; } }

	private ProjectileMover mMover;

}