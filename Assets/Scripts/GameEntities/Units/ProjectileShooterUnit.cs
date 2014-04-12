using System;
using UnityEngine;
using System.Collections;

public sealed class ProjectileShooterUnit : BaseUnit
{
	public override int LayerMask { get { return (int)(Layer.Ground | Layer.Air); } }

    public bool isActive = true;				// Sets if the cannon is active (Could be disabled or something by walking on button etc)
    public float secondsBetweenShots = 5;
    public float projectileSpeed = 4;
    public DirectionEnum directionToShoot;      // The direction to shoot the projectile
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
	
	private void Shoot() {
		Vector3 spawnPosition = transform.position + new Vector3(0, 1.1f, 0);

        float xDirection = 0f;
        float zDirection = 0f;

        switch (directionToShoot)
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

		_projectile = (GameObject)Instantiate(projectilePrefab, spawnPosition, rotation);

		ProjectileMover projectileMover = _projectile.GetComponent<ProjectileMover>();
		projectileMover.moveSpeed = projectileSpeed;
    }
}