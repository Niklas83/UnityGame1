using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class ProjectileUnit : BaseUnit  {

	public bool CanPassThroughUnits;
	public GameObject onHitPfx;

	public override int LayerMask { get { return (int)Layer.Air; } }
    public override bool CanWalkOver { get { return true; } }
    public override bool CanWalkOn(string incomingUnitTag)
    {
        return CanWalkOver;
    }

	public override void OnCollided(BaseUnit iUnit) {
		if (iUnit is ProjectileUnit)
			return;

		if (iUnit is AvatarUnit || (iUnit is BaseUnit && !CanPassThroughUnits)) {
			StartCoroutine(Explode(0.5f, iUnit));
		}
	}

	private IEnumerator Explode(float delay, BaseUnit hitUnit) {
		Vector3 startPosition = transform.position;
		Vector3 endPosition = hitUnit.transform.position;
		endPosition.y = startPosition.y;

		float t = 0;
		float moveSpeed = this.GetComponent<ProjectileMover>().MoveSpeed;
		while (t < 1f)
		{
			t += Time.deltaTime*moveSpeed;
			transform.position = Vector3.Lerp(startPosition, endPosition, Mathf.Clamp01(t));
			yield return null;
		}

		if (onHitPfx) {
			Helper.Instansiate(onHitPfx);
			onHitPfx.transform.position = transform.position;
		}

		if (hitUnit is AvatarUnit) {
			hitUnit.DestroyUnit();
		}

		DestroyUnit();
	}
}