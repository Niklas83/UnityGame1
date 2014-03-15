using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using System.Collections;

public class AxisRotateProxyMover : BaseMover {
	public override bool TryMove(int xDir, int yDir) {
		Transform parent = gameObject.transform.parent;
		AxisRotateMover arm = parent.gameObject.GetComponent<AxisRotateMover>();
		return arm.RequestMove(gameObject.transform, xDir, yDir);
    }
}
