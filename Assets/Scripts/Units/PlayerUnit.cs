using UnityEngine;
using System.Collections;
using System;

public sealed class PlayerUnit : BaseUnit
{
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
			StartCoroutine(mMover.move(x, 0));
		else if (Math.Abs(x) < Math.Abs(y)) 
			StartCoroutine(mMover.move(0, y));
	}
}