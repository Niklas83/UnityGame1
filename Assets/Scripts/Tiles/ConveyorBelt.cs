using UnityEngine;
using System.Collections;

public class ConveyorBelt : BaseTile 
{
	public float Speed = 0.5f;
	
	protected override void OnLeaved(BaseUnit iUnit, BaseTile iNextTile) {}
	protected override void OnArrived(BaseUnit iUnit, BaseTile iPreviousTile) {}
	
	void Update() {
		BaseUnit unit = GetOccupyingUnit();
		if (unit == null)
			return;
		
		Vector3 position = unit.transform.position;
		position.x += Speed * Time.deltaTime;
		unit.transform.position = position;
	}
}
