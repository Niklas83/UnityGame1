using UnityEngine;
using System.Collections;

public class RedAndBlueUnit : BaseUnit
{
    //This weight is compared to the floors durability, if the weight > durability you fall through the floor
    public int CurrentWeight = 50;
    public override int Weight { get { return CurrentWeight; } }

    //Check this TRUE if you want the unit to be breakable by medusarays and other projectiles
    public bool BreaksByProjectile = false;
    public override bool BreaksByProjectileAndMedusa { get { return BreaksByProjectile; } }

    public override int LayerMask { get { return (int)(Layer.Ground | Layer.Air); } }

    private bool lowered = false;

    private bool walkOver = false;

    public bool hasBeenActivated = false;

    public ParticleSystem DestructionEffect;

    public override bool CanWalkOver { get { return walkOver; } }
    public override bool CanWalkOn(string incomingUnitTag)
    {
        return CanWalkOver;
    }


    public void ToggleUpDown()
    {
        if (!lowered)
        {
            this.transform.position = new Vector3(this.transform.position.x, 0.05f,
                this.transform.position.z);
            walkOver = true;
            lowered = true;

            this.OnDeactivated();
        }
        else
        {
            this.transform.position = new Vector3(this.transform.position.x, 1f, this.transform.position.z);
            walkOver = false;
            lowered = false;


            BaseTile currentTile = GridManager.GetTile(Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.z));

            BaseUnit OccupyingUnit = currentTile.GetOccupyingUnitOnLayer(Layer.Ground);

            if (OccupyingUnit is AvatarUnit)
            {
                AvatarUnit avatarUnit = (AvatarUnit) OccupyingUnit;
                PlayDestructionEffect();
                avatarUnit.KillAvatar();
            }
            else if(OccupyingUnit != null)
            {
                PlayDestructionEffect();
                OccupyingUnit.DestroyUnit();
            }
            
            this.OnActivated();
        }
    }

    private void PlayDestructionEffect()
    {
        //Instantiate our one-off particle system
        ParticleSystem explosionEffect = Instantiate(DestructionEffect) as ParticleSystem;

        explosionEffect.transform.position = new Vector3(transform.position.x, transform.position.y + 1, transform.position.z);

        //play it
        explosionEffect.loop = false;
        explosionEffect.Play();

        //destroy the particle system when its duration is up, right
        //it would play a second time.
        Destroy(explosionEffect.gameObject, explosionEffect.duration);
    }
}
