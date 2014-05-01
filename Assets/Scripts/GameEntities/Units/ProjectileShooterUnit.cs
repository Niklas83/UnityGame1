using System;
using UnityEngine;
using System.Collections;

public sealed class ProjectileShooterUnit : BaseUnit
{
    //This weight is compared to the floors durability, if the weight > durability you fall through the floor
    public int CurrentWeight = 50;
    public override int Weight { get { return CurrentWeight; } }

	public override int LayerMask { get { return (int)(Layer.Ground | Layer.Air); } }

    public bool isActive = true;				// Sets if the cannon is active (Could be disabled or something by walking on button etc)
    public float secondsBetweenShots = 5;
    public float projectileSpeed = 4;
    public bool canShootThroughWalls = false;
    public GameObject projectilePrefab;
    
	private GameObject _projectile;
    private bool _isShooting = false;
	
    public override bool CanWalkOver { get { return false; } }
    public override bool CanWalkOn(string incomingUnitTag) {
        return CanWalkOver;
    }

    void Update() {
        if (isActive && !_isShooting) {
            _isShooting = true;
            StartCoroutine(StartShooting());
        }
    }

    private IEnumerator StartShooting() {
        while (isActive) {
            Shoot();
            yield return new WaitForSeconds(secondsBetweenShots);
        }
        _isShooting = false;
    }
	
	private void Shoot() 
	{
		Vector3 spawnPosition = transform.position + new Vector3(0, 0.6f, 0);
		Vector3 direction = transform.forward;
		Quaternion rotation = Quaternion.LookRotation(direction);

		_projectile = (GameObject)Instantiate(projectilePrefab, spawnPosition, rotation);

		ProjectileMover projectileMover = _projectile.GetComponent<ProjectileMover>();
		projectileMover.moveSpeed = projectileSpeed;
    }
}