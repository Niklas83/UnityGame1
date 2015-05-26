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

    private LevelManager LevelSelectionAndUILogic;          //save on
    
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

	void Start() {
		_sceneTransition = Helper.Find<SceneTransition>("SceneTransition");

        LevelSelectionAndUILogic = new LevelManager();
        LevelSelectionAndUILogic.LoadFromJson();
	}


	protected override void OnLeaved(BaseUnit unit, BaseTile nextTile) {}
	protected override void OnArrived(BaseUnit unit, BaseTile previousTile) 
	{
        base.OnArrived(unit, previousTile);
	    if ((_opened || debugOpen) && unit is AvatarUnit)
	    {
	        for (int i = 0; i < LevelSelectionAndUILogic.ListOfAllLevelJSON.Count; i++)
	        {
                if (LevelSelectionAndUILogic.ListOfAllLevelJSON[i].Name.Equals(Application.loadedLevelName))
	            {
                    LevelSelectionAndUILogic.ListOfAllLevelJSON[i].HasPassed = true;
                    LevelSelectionAndUILogic.ListOfAllLevelJSON[i].IsActive = true;          //its sufficient with just setting the current map to "haspassed" true, as its just first the first level that uses is active, but if future changes need em its here...
                    LevelSelectionAndUILogic.ListOfAllLevelJSON[i + 1].IsActive = true;      //same comment as above.
	            }
	        }

	        LevelSelectionAndUILogic.SaveToJson();          //saves the level progression
            StartCoroutine(_sceneTransition.LoadLevelWithDelay(0.1f, "GameStartScene"));
	    }

        if (IsPortalTile && !_opened)
        {
            TeleportUnit(unit, previousTile, DestinationTeleportTile);
        }
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
