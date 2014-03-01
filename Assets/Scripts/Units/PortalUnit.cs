using UnityEngine;
using System.Collections;

public sealed class PortalUnit : BaseUnit
{

    public override bool CanWalkOver { get { return true; } }

    public override bool CanWalkOn(string incomingUnitTag)
    {

        this.audio.enabled = true;
        return CanWalkOver;
    }



    //public override bool CanWalkOn
    //{

    //    get
    //    {
            
    //        this.audio.enabled = true;
    //        //Swap level or somthing :)
    //        return true;
    //    }
    //}


    public override void SetActive(bool iActive)
    {
        this.gameObject.renderer.enabled = true;
        var fireAnimation = this.gameObject.transform.GetChild(0);
        fireAnimation.particleSystem.enableEmission = true;
        //this.gameObject.GetComponentInChildren(Light);
    }

}