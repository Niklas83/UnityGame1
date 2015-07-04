using System.Collections;
using Newtonsoft.Json.Serialization;
using ParticlePlayground;
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
   public abstract int Durability { get; }

    public abstract bool TrainTile { get; }

    public abstract bool IceTile { get; }

    public abstract bool TeleporterTile { get; }
    public PortalColorTypes TeleportColor;
    private GameObject _teleportGraphicsPrefab;
    protected GameObject InstantiatedPortal;
    private Light _portalLight;

    public abstract BaseTile TeleportDestinationTile { get; }

	private Dictionary<Layer, BaseUnit> _occupyingUnits;
	private List<BaseUnit> _previousUnits;
	
	public bool PROTOTYPE_UseIceFriction = false;

	public override void Init(GridManager gridManager) {
		base.Init(gridManager);
		_occupyingUnits = new Dictionary<Layer, BaseUnit>();
		_previousUnits = new List<BaseUnit>();
	}

	public void OnGUI() {
		if (!Defines.DEBUG_TILES)
			return;

		foreach (Layer l in Enum.GetValues(typeof(Layer))) {
			BaseUnit unit;
			if (_occupyingUnits.TryGetValue(l, out unit)) {
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

	public static void TeleportTo(BaseUnit unit, BaseTile sourceTile, BaseTile destinationTile) {
		Vector3 position = destinationTile.transform.position;
		position.y = unit.transform.position.y;
		unit.transform.position = position;

		HandleOccupy(unit, sourceTile, destinationTile);
		HandleArrive(unit, sourceTile, destinationTile);
	}
	public static void HandleOccupy(BaseUnit unit, BaseTile sourceTile, BaseTile destinationTile) {
		if (sourceTile != null)
			sourceTile.Unoccupy(unit);
		if (destinationTile != null)
			destinationTile.Occupy(unit);
	}
	public static void HandleArrive(BaseUnit unit, BaseTile sourceTile, BaseTile destinationTile) {
		if (sourceTile != null)
			sourceTile.Leave(unit, destinationTile);
		if (destinationTile != null)
			destinationTile.Arrive(unit, sourceTile);
	}

	private void Occupy(BaseUnit unit) {
		int mask = unit.LayerMask;
		foreach (Layer l in Enum.GetValues(typeof(Layer))) {
			int layerMask = (int)l;
			if ((layerMask & mask) == layerMask) {
				BaseUnit u;
				if (_occupyingUnits.TryGetValue(l, out u))
					_previousUnits.Add(u);

				_occupyingUnits[l] = unit;
			}
		}
		unit.OccupiedTile = this;
	}

	private void Unoccupy(BaseUnit unit) {
		int mask = unit.LayerMask;
		foreach (Layer l in Enum.GetValues(typeof(Layer))) {
			BaseUnit u;
			if (_occupyingUnits.TryGetValue(l, out u)) {
				int layerMask = (int)l;
				if ((layerMask & mask) == layerMask && unit == u)
					_occupyingUnits.Remove(l);
			}
		}
	}

	private void Arrive(BaseUnit unit, BaseTile sourceTile) {
		unit.OnArrived(this, _previousUnits);
		foreach (BaseUnit u in _previousUnits)
			u.OnArrivedToMe(unit);

		_previousUnits.Clear();
		OnArrived(unit, sourceTile);
	}

	private void Leave(BaseUnit unit, BaseTile destinationTile) {
		unit.OnLeaved(this);
		OnLeaved(unit, destinationTile);
	}
	
	public virtual bool CanWalkOn(BaseUnit unit) {
		foreach (BaseUnit u in OccupyingUnits(unit)) {
			if (u != unit && !u.CanWalkOn(unit.gameObject.tag))
				return false;
		}
		return true;
	}
	public BaseUnit GetOccupyingUnitOnLayer(Layer iLayer) {
		BaseUnit unit;
		_occupyingUnits.TryGetValue(iLayer, out unit);
		return unit;
	}

	protected abstract void OnLeaved(BaseUnit unit, BaseTile sourceTile);

    protected virtual void OnArrived(BaseUnit unit, BaseTile destinationTile)
    {
        if (unit.Weight > Durability)
        {
            GridManager.RemoveTile(this);
            Destroy(this.gameObject);

            unit.InitStartFallingRoutine();
        }   
    }

	private static Array mLayers = Enum.GetValues(typeof(Layer));
	public IEnumerable<BaseUnit> OccupyingUnits(BaseUnit unit) {
		foreach (Layer l in mLayers) {
			BaseUnit u;
			if (_occupyingUnits.TryGetValue(l, out u)) {
				int layerMask = (int)l;
				if ((layerMask & unit.LayerMask) == layerMask)
					yield return u;
			}
		}
	}

    //Used by the teleport of portal tiles
    protected void TeleportUnit(BaseUnit unit, BaseTile previousTile, BaseTile destinationTeleportTile)
    {
        if (previousTile == destinationTeleportTile || destinationTeleportTile == null)// Came from the other portal
        {
            return;
        }

        if (!destinationTeleportTile.CanWalkOn(unit))
        {
            return;
        }

        BaseTile.TeleportTo(unit, this, destinationTeleportTile);

        if (unit is AvatarUnit)
        {
            AvatarUnit avatar = (AvatarUnit)unit;
            avatar.EmptyMoveQueue();
        }
    }
    
    //Currently just used for teleporter and its graphical gameobject
    public void OnEnable()
    {
        GetPortalGraphicsPrefab(); //Tar fram den aktuella grafik prefaben

        if (TeleporterTile && _teleportGraphicsPrefab != null)
        {
            if (InstantiatedPortal == null)
            {
                InstantiatedPortal = GameObject.Instantiate(_teleportGraphicsPrefab) as GameObject;           

                InstantiatedPortal.transform.parent = this.transform;
                _portalLight = GetComponentInChildren<Light>();
            }
            else
            {
                if (!InstantiatedPortal.activeSelf)
                {
                    InstantiatedPortal.SetActive(true);
                }
                PlaygroundParticlesC particles = GetComponentInChildren<PlaygroundParticlesC>();
                particles.emit = true;
            }

            InstantiatedPortal.transform.position = new Vector3(transform.position.x, transform.position.y + 0.6f, transform.position.z);

            StartCoroutine(FadeInTeleporterLight(true));    //fade in light
        }
    }

    //Currently just used for teleporter and its graphical gameobject
    public void OnDisable()
    {
        if (TeleporterTile && _teleportGraphicsPrefab != null)
        {
            if (InstantiatedPortal != null)
            {
                if (InstantiatedPortal.activeSelf)
                {
                    PlaygroundParticlesC particles = GetComponentInChildren<PlaygroundParticlesC>();

                    if (particles != null)
                    {
                        particles.emit = false;

                        StartCoroutine(FadeInTeleporterLight(false)); //fade out light
                    }
                }
            }   
        }
    }

    private void GetPortalGraphicsPrefab()
    {
        switch (TeleportColor)
        {
            case PortalColorTypes.Blue:
                _teleportGraphicsPrefab = Resources.Load("ParticleEffects/PortalBlue") as GameObject;
                break;
            case PortalColorTypes.Green:
                _teleportGraphicsPrefab = Resources.Load("ParticleEffects/PortalGreen") as GameObject;
                break;
            case PortalColorTypes.Pink:
                _teleportGraphicsPrefab = Resources.Load("ParticleEffects/PortalPink") as GameObject;
                break;
            case PortalColorTypes.Red:
                _teleportGraphicsPrefab = Resources.Load("ParticleEffects/PortalRed") as GameObject;
                break;
            case PortalColorTypes.Yellow:
                _teleportGraphicsPrefab = Resources.Load("ParticleEffects/PortalYellow") as GameObject;
                break;
            case PortalColorTypes.Dark:
                _teleportGraphicsPrefab = Resources.Load("ParticleEffects/PortalDark") as GameObject;
                break;
        }
    }


    private IEnumerator FadeInTeleporterLight(bool fadeIn)
    {
        //TODO fixa så att ljuset slutar fadea in om spelaren lämnar den (Gäller för toggle)

        if (fadeIn)
        {
            bool firstRun = true;
            float t = 0;
            while (_portalLight.intensity < 1.6f)
            {
                if (firstRun)
                {
                    firstRun = false;
                    yield return new WaitForSeconds(0.75f); //Delay då det tar en stund för portalen att initieras visuellt
                }

                _portalLight.intensity = _portalLight.intensity + 0.10f;
                yield return new WaitForSeconds(0.075f);
            }
        }

        else
        {
            float t = 0;
            while (_portalLight.intensity > 0f)
            {
                _portalLight.intensity = _portalLight.intensity - 0.10f;
                yield return new WaitForSeconds(0.06f);
            }
        }

    }
}
