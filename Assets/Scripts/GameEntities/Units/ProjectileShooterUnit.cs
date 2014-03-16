using System;
using UnityEngine;
using System.Collections;

public sealed class ProjectileShooterUnit : BaseUnit
{
	public override int LayerMask { get { return (int)(Layer.Ground | Layer.Air); } }

    public bool IsActive = true;				// Sets if the cannon is active (Could be disabled or something by walking on button etc)
    public float SecondsBetweenShots = 5;
    public float ProjectileSpeed = 4;
    public DirectionEnum DirectionToShoot;      // The direction to shoot the projectile
    public bool CanShootThroughWalls = false;

    public GameObject projectilePrefab;
	private GameObject projectile;
    private bool IsCurrentlyShooting = false;
	
    public override bool CanWalkOver { get { return false; } }
    public override bool CanWalkOn(string incomingUnitTag) {
        return CanWalkOver;
    }

    void Update() {
        if (IsActive && !IsCurrentlyShooting) {
            IsCurrentlyShooting = true;
            StartCoroutine(StartShooting());
        }
    }

    private IEnumerator StartShooting() {
        while (IsActive) {
            Shoot();
            yield return new WaitForSeconds(SecondsBetweenShots);
        }
        IsCurrentlyShooting = false;
    }
	
	private void Shoot(){
		Vector3 spawnPosition = transform.position + new Vector3(0, 1.1f, 0);

        float xDirection = 0f;
        float zDirection = 0f;

        switch (DirectionToShoot)
        {
            case DirectionEnum.Down:
                zDirection = -1;
                break;
            case DirectionEnum.Left:
                xDirection = -1;
                break;
            case DirectionEnum.Right:
                xDirection = 1;
                break;
            case DirectionEnum.Up:
                zDirection = 1;
                break;
        }

		Vector3 forward = new Vector3(xDirection, 0, zDirection);
		Quaternion rotation = Quaternion.LookRotation(forward);

		projectile = (GameObject)Instantiate(projectilePrefab, spawnPosition, rotation);

		ProjectileMover projectileMover = projectile.GetComponent<ProjectileMover>();
		projectileMover.MoveSpeed = ProjectileSpeed;
    }
}