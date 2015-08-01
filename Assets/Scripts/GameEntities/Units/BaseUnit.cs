using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public abstract class BaseUnit : BaseEntity
{
    public abstract int Weight { get; }

    //List of leave sounds (will only be played if a sound is set)
    public AudioClip[] LeaveSound;

    //List of arrive sounds (will only be played if a sound is set)
    public AudioClip[] ArriveSound;

    [Range(0.0f,1.0f)]
    public float ArriveLeaveVolume = 1f;

    public abstract bool BreaksByProjectileAndMedusa { get; } 

    public abstract bool CanWalkOver { get; }
	public abstract int LayerMask { get; }
	public BaseTile OccupiedTile { get; set; }

    public abstract bool CanWalkOn(string incomingUnitTag); // Returns the CanWalkOver bool

	public virtual void OnLeaved(BaseTile tile) {}
	public virtual void OnCollided(BaseUnit unit) {}
	public virtual void OnArrived(BaseTile tile, List<BaseUnit> previousUnits) {}
	public virtual void OnArrivedToMe(BaseUnit unit) {}

	public virtual void DestroyUnit() {
		BaseTile.HandleOccupy(this, OccupiedTile, null); // Need to leave the grid before destroying!
		Destroy(gameObject);
	}

	protected override void OnActivated() {
		if (OccupiedTile.CanWalkOn(this)) {
			BaseTile.HandleOccupy(this, null, OccupiedTile);
		}
	}
	protected override void OnDeactivated() {
		BaseTile.HandleOccupy(this, OccupiedTile, null);
	}


    public void InitStartFallingRoutine()
    {
        bool isAvatarUnit = false;

        AvatarUnit currentAvatar = null;
        
        if (this is AvatarUnit)
        {
            isAvatarUnit = true;
            currentAvatar = (AvatarUnit)this;
        }


        StartCoroutine(StartFalling(isAvatarUnit, currentAvatar));
    }

    private IEnumerator StartFalling(bool isAvatar, AvatarUnit currentAvatar)
    {
        if (isAvatar)
        {
            currentAvatar.SetIsFalling();
        }
       
        while (this.transform.position.y > -10)
        {
            this.transform.position = this.transform.position - new Vector3(0, 1, 0);
            yield return new WaitForSeconds(0.03f);
        }

        if (isAvatar)
        {
            currentAvatar.KillAvatar();
        }
        else
        {
            this.DestroyUnit();
        }
        
        yield return 0;
    }


    public void EventCallOnActivated()
    {
        OnActivated();
    }

    public void EventCallOnDeactivated()
    {
        OnDeactivated();
    }
}