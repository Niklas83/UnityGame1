using UnityEngine;
using System.Collections;

public class ConveyorBelt : BaseTile 
{
    //This Durabiliy is compared to the units weight, if the weight > durability you fall through the floor
    public int CurrentDurability = 100;
    public override int Durability { get { return CurrentDurability; } }

    //Trainunits check if this is true, if so it may move on it
    public override bool TrainTile { get { return IsTrainTile; } }
    public bool IsTrainTile = false;

    //If this is true objects being pushed on this tile from another tile will keep sliding towards the direction it was pushed
    public override bool IceTile { get { return IsIceTile; } }
    public bool IsIceTile = false;

    //This tile is a portal if this is true
    public override bool TeleporterTile { get { return IsPortalTile; } }
    public bool IsPortalTile = false;
    public override BaseTile TeleportDestinationTile { get { return DestinationTeleportTile; }}
    public BaseTile DestinationTeleportTile;

	public float speed = 0.5f;
	
	protected override void OnLeaved(BaseUnit unit, BaseTile nextTile) {}
    protected override void OnArrived(BaseUnit unit, BaseTile previousTile)
    {
        base.OnArrived(unit, previousTile);

        if (IsPortalTile)
        {
            TeleportUnit(unit, previousTile, DestinationTeleportTile);
        }
    }
	
	void Update() {
		BaseUnit unit = GetOccupyingUnitOnLayer(Layer.Ground);
		if (unit == null)
			return;
		
		Vector3 position = unit.transform.position;
		position.x += speed * Time.deltaTime;
		unit.transform.position = position;
	}
}
