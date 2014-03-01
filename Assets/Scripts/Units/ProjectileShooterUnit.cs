using System;
using UnityEditor;
using UnityEngine;
using System.Collections;

public sealed class ProjectileShooterUnit : BaseUnit
{
    //public fields
    public bool IsActive = true;            //Sets if the cannon is active (Could be disabled or something by walking on button etc)
    public float SecondsBetweenShots = 5;
    public float ProjectileSpeed = 4;
    public Vector3 PoisitionToSpawnTheProjectile;    //from where the projectile will be spawned
    public ProjectileTypeEnum ProjectileType;       //the type of projectile being launched from the cannon
    public DirectionEnum DirectionToShoot;      //The direction to shoot the projectile
    public bool CanShootThroughWalls = false;           //TODO: Must make it so you can have multiple units on each floor tile, arraylist connected to each tile for example
    public bool ResetMapIfPlayerSlain = false;

    //ONE of the below Prefab to use based on what set ProjectileType
    public GameObject FireBallPrefab;
    public GameObject ArrowPrefab;
    public GameObject IceBallPrefab;
    public GameObject SpearPrefab;
    public GameObject ToxicBallPrefab;

    private bool IsCurrentlyShooting = false;

    // changed so u can have method handling the return value
    public override bool CanWalkOver { get { return false; } }

    public override bool CanWalkOn(string incomingUnitTag)
    {
        return CanWalkOver;
    }

    void Update()
    {
        if (IsActive && !IsCurrentlyShooting)
        {
            IsCurrentlyShooting = true;
            StartCoroutine(StartShooting());
        }
    }

    IEnumerator StartShooting()
    {
        while (IsActive)
        {
            Shoot();

            yield return new WaitForSeconds(SecondsBetweenShots);
        }
        IsCurrentlyShooting = false;
    }


    void Shoot()
    {
        switch (DirectionToShoot)       //TODO:If we want to keep the spawning within the statue this can be removed this switch can be removed
        {
                //If direction to shot is set down (z-1)
            case DirectionEnum.Down:
                PoisitionToSpawnTheProjectile = new Vector3(
                    this.gameObject.transform.position.x,
                    this.gameObject.transform.position.y+1,
                    this.gameObject.transform.position.z);
                break;
                //If direction to shot is set down (x-1)
            case DirectionEnum.Left:
                PoisitionToSpawnTheProjectile = new Vector3(
                    this.gameObject.transform.position.x,
                    this.gameObject.transform.position.y + 1,
                    this.gameObject.transform.position.z);
                break;
                //If direction to shot is set down (x+1)
            case DirectionEnum.Right:
                PoisitionToSpawnTheProjectile = new Vector3(
                    this.gameObject.transform.position.x,
                    this.gameObject.transform.position.y + 1,
                    this.gameObject.transform.position.z);
                break;
                //If direction to shot is set down (z+1)
            case DirectionEnum.Up:
                PoisitionToSpawnTheProjectile = new Vector3(
                    this.gameObject.transform.position.x,
                    this.gameObject.transform.position.y + 1,
                    this.gameObject.transform.position.z);
                break;

        }
        
        var xDirection = 0;
        var zDirection = 0;
        //Direction to apply "force"
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

        var projectile = new GameObject();     //Temp value that will be set below 
		Vector3 forward = new Vector3(xDirection, 0, zDirection);
		Quaternion rotation = Quaternion.LookRotation(forward);

        //Checks if its gonna shoot a fireball
        if (ProjectileType == ProjectileTypeEnum.FireBall)
        {
			projectile = (GameObject)Instantiate(FireBallPrefab, PoisitionToSpawnTheProjectile, rotation);
        }
        //Checks if its gonna shoot a iceball
        if (ProjectileType == ProjectileTypeEnum.IceBall)
        {
			projectile = (GameObject)Instantiate(IceBallPrefab, PoisitionToSpawnTheProjectile, rotation);
        }
        //Checks if its gonna shoot a toxicball
        if (ProjectileType == ProjectileTypeEnum.ToxicBall)
        {
			projectile = (GameObject)Instantiate(ToxicBallPrefab, PoisitionToSpawnTheProjectile, rotation);
        }

        var projectileGameObject = projectile;
        ProjectileMover projectileMover = projectileGameObject.GetComponent<ProjectileMover>();
		ProjectileUnit projectileUnit = projectileGameObject.GetComponent<ProjectileUnit>();

		projectileUnit.CanPassThroughUnits = CanShootThroughWalls;
		projectileUnit.ResetMapIfPlayerIsKilled = ResetMapIfPlayerSlain;

        projectileMover.MoveSpeed = ProjectileSpeed;
    }
}