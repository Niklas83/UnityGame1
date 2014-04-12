using UnityEngine;
using System.Collections;

public class ConveyorBelt : BaseTile 
{
	public float speed = 0.5f;
	
	protected override void OnLeaved(BaseUnit unit, BaseTile nextTile) {}
	protected override void OnArrived(BaseUnit unit, BaseTile previousTile) {}
	
	void Update() {
		BaseUnit unit = GetOccupyingUnitOnLayer(Layer.Ground);
		if (unit == null)
			return;
		
		Vector3 position = unit.transform.position;
		position.x += speed * Time.deltaTime;
		unit.transform.position = position;
	}
}
