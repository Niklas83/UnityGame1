using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class ProjectileUnit : BaseUnit  {

	public bool canPassThroughUnits;
	public GameObject onHitPfx;

	public override int LayerMask { get { return (int)Layer.Air; } }
    public override bool CanWalkOver { get { return true; } }
    public override bool CanWalkOn(string incomingUnitTag)
    {
        return CanWalkOver;
    }

	public override void OnCollided(BaseUnit unit) {
		if (unit is ProjectileUnit)
			return;

		if (unit is AvatarUnit || (unit is BaseUnit && !canPassThroughUnits)) {
			StartCoroutine(Explode(unit));
		}
	}

	private IEnumerator Explode(BaseUnit hitUnit) {
		Vector3 startPosition = transform.position;
		Vector3 endPosition = startPosition + transform.rotation * Vector3.forward * 0.5f;
		endPosition.y = startPosition.y;
		BaseTile.HandleOccupy(this, OccupiedTile, null);

		float t = 0;
		float moveSpeed = this.GetComponent<ProjectileMover>().moveSpeed;
		while (t < 1f)
		{
			t += Time.deltaTime*moveSpeed;
			transform.position = Vector3.Lerp(startPosition, endPosition, Mathf.Clamp01(t));
			yield return null;
		}

		if (onHitPfx) {
			GameObject.Instantiate(onHitPfx, transform.position, Quaternion.identity);
		}

		if (hitUnit is AvatarUnit) {
			hitUnit.SendMessage("KillAvatar");
		}

		DestroyUnit();
	}
}