using UnityEngine;
using System.Collections;

public sealed class PortalUnit : BaseUnit
{
	public override int LayerMask { get { return (int)Layer.Ground; } }
    public override bool CanWalkOver { get { return true; } }
    public override bool CanWalkOn(string incomingUnitTag)
    {
        this.audio.enabled = true;
        return CanWalkOver;
    }

    public override void SetActive(bool iActive)
    {
        this.gameObject.renderer.enabled = true;
        var fireAnimation = this.gameObject.transform.GetChild(0);
        fireAnimation.particleSystem.enableEmission = true;
    }
}