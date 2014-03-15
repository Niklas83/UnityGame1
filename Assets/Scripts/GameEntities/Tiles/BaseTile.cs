using UnityEngine;
using System;
using System.Collections.Generic;

[Flags]
public enum Layer {
	Ground = 1 << 0,
	Air = 1 << 1,
}

public abstract class BaseTile : BaseEntity
{
	public Dictionary<Layer, BaseUnit> mOccupyingUnits;
	private List<BaseUnit> mPreviousUnits;

	public override void Init(GridManager iGridManager) {
		base.Init(iGridManager);
		mOccupyingUnits = new Dictionary<Layer, BaseUnit>();
		mPreviousUnits = new List<BaseUnit>();
	}

	public void OnGUI() {
		if (!Defines.DEBUG_TILES)
			return;

		foreach (Layer l in Enum.GetValues(typeof(Layer))) {
			BaseUnit unit;
			if (mOccupyingUnits.TryGetValue(l, out unit)) {
				GUI.skin.label.normal.textColor = Color.black;
				if (unit != null) {
					Vector3 screen = Camera.main.WorldToScreenPoint(this.transform.position);
					GUI.Label(new Rect(screen.x-48, Screen.height-screen.y+2, 100, 100), "" + l + " " + unit);
					GUI.skin.label.normal.textColor = Color.white;
					GUI.Label(new Rect(screen.x-50, Screen.height-screen.y, 100, 100), "" + l + " " + unit);
				} else {
					Vector3 screen = Camera.main.WorldToScreenPoint(this.transform.position);
					GUI.Label(new Rect(screen.x-48, Screen.height-screen.y+2, 100, 100), "" + l + " NULL!");
					GUI.skin.label.normal.textColor = Color.white;
					GUI.Label(new Rect(screen.x-50, Screen.height-screen.y, 100, 100), "" + l + " NULL!");
				}
			}
		}
	}

	public static void TeleportTo(BaseUnit iUnit, BaseTile iSourceTile, BaseTile iDestinationTile) {
		Vector3 position = iDestinationTile.transform.position;
		position.y = 1;
		iUnit.transform.position = position;

		HandleOccupy(iUnit, iSourceTile, iDestinationTile);
		HandleArrive(iUnit, iSourceTile, iDestinationTile);
	}
	public static void HandleOccupy(BaseUnit iUnit, BaseTile iSourceTile, BaseTile iDestinationTile) {
		if (iSourceTile != null)
			iSourceTile.Unoccupy(iUnit);
		if (iDestinationTile != null)
			iDestinationTile.Occupy(iUnit);
	}
	public static void HandleArrive(BaseUnit iUnit, BaseTile iSourceTile, BaseTile iDestinationTile) {
		if (iSourceTile != null)
			iSourceTile.Leave(iUnit, iDestinationTile);
		if (iDestinationTile != null)
			iDestinationTile.Arrive(iUnit, iSourceTile);
	}

	private void Occupy(BaseUnit iUnit) {
		int mask = iUnit.LayerMask;
		foreach (Layer l in Enum.GetValues(typeof(Layer))) {
			int layerMask = (int)l;
			if ((layerMask & mask) == layerMask) {
				BaseUnit unit;
				if (mOccupyingUnits.TryGetValue(l, out unit))
					mPreviousUnits.Add(unit);

				mOccupyingUnits[l] = iUnit;
			}
		}
		iUnit.OccupiedTile = this;
	}

	private void Unoccupy(BaseUnit iUnit) {
		int mask = iUnit.LayerMask;
		foreach (Layer l in Enum.GetValues(typeof(Layer))) {
			BaseUnit unit;
			if (mOccupyingUnits.TryGetValue(l, out unit)) {
				int layerMask = (int)l;
				if ((layerMask & mask) == layerMask && unit == iUnit)
					mOccupyingUnits.Remove(l);
			}
		}
	}

	private void Arrive(BaseUnit iUnit, BaseTile iSourceTile) {
		iUnit.OnArrived(this, mPreviousUnits);
		foreach (BaseUnit unit in mPreviousUnits)
			unit.OnArrivedToMe(iUnit);

		mPreviousUnits.Clear();
		OnArrived(iUnit, iSourceTile);
	}

	private void Leave(BaseUnit iUnit, BaseTile iDestinationTile) {
		iUnit.OnLeaved(this);
		OnLeaved(iUnit, iDestinationTile);
	}
	
	public bool CanWalkOn(BaseUnit iUnit) {
		foreach (BaseUnit unit in OccupyingUnits(iUnit)) {
			if (unit != iUnit && !unit.CanWalkOn(iUnit.gameObject.tag))
				return false;
		}
		return true;
	}
	public BaseUnit GetOccupyingUnitOnLayer(Layer iLayer) {
		BaseUnit unit;
		mOccupyingUnits.TryGetValue(iLayer, out unit);
		return unit;
	}

	protected abstract void OnLeaved(BaseUnit iUnit, BaseTile iSourceTile);
	protected abstract void OnArrived(BaseUnit iUnit, BaseTile iDestinationTile);

	private static Array mLayers = Enum.GetValues(typeof(Layer));
	public IEnumerable<BaseUnit> OccupyingUnits(BaseUnit iUnit) {
		foreach (Layer l in mLayers) {
			BaseUnit unit;
			if (mOccupyingUnits.TryGetValue(l, out unit)) {
				int layerMask = (int)l;
				if ((layerMask & iUnit.LayerMask) == layerMask)
					yield return unit;
			}
		}
	}
}
