using UnityEngine;
using System.Collections;

public class ExitTile : BaseTile {

    //This Durabiliy is compared to the units weight, if the weight > durability you fall through the floor
    public int CurrentDurability = 100;
    public override int Durability { get { return CurrentDurability; } }

	private SceneTransition _sceneTransition;
	private bool _opened;
	private int _nrAliveKeys;
	
	public bool debugOpen = false;

	void Start() {
		_sceneTransition = Helper.Find<SceneTransition>("SceneTransition");
	}

	protected override void OnLeaved(BaseUnit unit, BaseTile nextTile) {}
	protected override void OnArrived(BaseUnit unit, BaseTile previousTile) 
	{
        base.OnArrived(unit, previousTile);
		if ((_opened || debugOpen) && unit is AvatarUnit)
			_sceneTransition.NextScene();
	}

	public void Register() 
	{
		_nrAliveKeys += 1;
		SetExitOpen(_nrAliveKeys == 0);
	}
	public void Unregister() 
	{
		_nrAliveKeys -= 1;
		SetExitOpen(_nrAliveKeys == 0);
	}

	private void SetExitOpen(bool open) 
	{
		Transform t = transform.FindChild("OpenEffects");
		t.gameObject.SetActive(open);
		_opened = open;
	}
}
