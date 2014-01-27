using UnityEngine;
using System.Collections;

public sealed class Unit : BaseUnit
{
	public override bool CanWalkOn { get { return false; } }
}