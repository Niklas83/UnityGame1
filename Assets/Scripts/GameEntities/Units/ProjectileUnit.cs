using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class ProjectileUnit : BaseUnit  {

    //This weight is compared to the floors durability, if the weight > durability you fall through the floor
    public int CurrentWeight = 50;
    public override int Weight { get { return CurrentWeight; } }

    //Check this TRUE if you want the unit to be breakable by medusarays and other projectiles
    public bool BreaksByProjectile = false;
    public override bool BreaksByProjectileAndMedusa { get { return BreaksByProjectile; } }

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

        if (hitUnit.BreaksByProjectileAndMedusa)
        {
            hitUnit.DestroyUnit();
        }
		DestroyUnit();
	}
}