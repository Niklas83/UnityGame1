using UnityEngine;
using System.Collections;
using System;

public sealed class PlayerUnit : BaseUnit
{
	public override bool CanWalkOn { get { return false; } }

	private Mover mMover;
	
	public void Start()
	{ 
		mMover = GetComponent<Mover>();
	}
	
	public void Update()
	{
		if (mMover.IsMoving)
			return;
		
		int x = Math.Sign(Input.GetAxis("Horizontal"));
		int y = Math.Sign(Input.GetAxis("Vertical"));
		if (Math.Abs(x) > Math.Abs(y))
			mMover.TryMove(x, 0);
		else if (Math.Abs(x) < Math.Abs(y)) 
			mMover.TryMove(0, y);
	}
}