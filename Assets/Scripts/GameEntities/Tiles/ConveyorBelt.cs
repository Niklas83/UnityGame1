using UnityEngine;
using System.Collections;

public class ConveyorBelt : BaseTile 
{
    //This Durabiliy is compared to the units weight, if the weight > durability you fall through the floor
    public int CurrentDurability = 100;
    public override int Durability { get { return CurrentDurability; } }

	public float speed = 0.5f;
	
	protected override void OnLeaved(BaseUnit unit, BaseTile nextTile) {}
    protected override void OnArrived(BaseUnit unit, BaseTile previousTile)
    {
        base.OnArrived(unit, previousTile);
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
