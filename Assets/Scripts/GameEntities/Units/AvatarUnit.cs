using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public partial class AvatarUnit : BaseUnit
{
    //This weight is compared to the floors durability, if the weight > durability you fall through the floor
    public int CurrentWeight = 50;
    public override int Weight { get { return CurrentWeight; } }

    //Check this TRUE if you want the unit to be breakable by medusarays and other projectiles
    public bool BreaksByProjectile = false;
    public override bool BreaksByProjectileAndMedusa { get { return BreaksByProjectile; } }

    public int Strength = 100;

	public override int LayerMask { get { return (int)(Layer.Air | Layer.Ground); } }
    
    public override bool CanWalkOver { get { return false; } }
    public override bool CanWalkOn(string incomingUnitTag)
    {
        if (!_isDead)
        {
            return CanWalkOver;
        }
        else
        {
            return true;
        }
    }
	public bool debugTeleport = false;
	public bool debugInvincible = false;

	private Mover _mover;
	private Rotation _rotation;
	private PathFinder _pathFinder;
	private GridManager _gridManager;
	private Queue<Vector2> _moveQueue;
    
	private bool _isDead = false;
    private bool _isActive = false;							// Is set to true when a character is selected

    private GameObject _audioComponent;						// Holds the audio listener with constant rotation
    private GameObject _avatarSoul;
    private SoulMover _soulMover;
    
    private Quaternion _lockAudioSourceLocation;			// Sets the rotation of the character to "north" every update
    private SoundEffectPlayer _soundEffectPlayer;
    
	private StateMachine _stateMachine;

    private bool _isFalling = false;                    //This is set when a player is falling to his death


    //Pause function added to the move to avoid move (Start)
    private RectTransform _pauseMenuPanel;
    private float _pauseMenuStartLocation;
    //Pause function added to the move (ends)

	public void Start()
	{
	    try
	    {
            _pauseMenuPanel = Helper.Find<RectTransform>("PauseMenuPanel");
            _pauseMenuStartLocation = _pauseMenuPanel.rect.yMin;
	    }
	    catch (Exception)
	    {
            //Continue with bussiness as usual...
            //Detta händer om det inte finns någon paus-meny och knapp
	    }
        
		_mover = GetComponent<Mover>();
		_rotation = GetComponent<Rotation>();

		Floor floor = Helper.Find<Floor>("Floor");
		_gridManager = floor.GridManager;
		_pathFinder = new PathFinder(_gridManager);
        
        //Audio related
		_lockAudioSourceLocation = this.gameObject.transform.rotation;
	    _audioComponent = this.gameObject.transform.FindChild("AudioComponent").gameObject;
        _avatarSoul = GameObject.Find("AvatarSoul");

		_soulMover = _avatarSoul.GetComponentInChildren<SoulMover>();
		_soundEffectPlayer = GetComponentInChildren<SoundEffectPlayer>();
        
		AvatarStates avatarStates = new AvatarStates(gameObject);
		_stateMachine = avatarStates.GetStateMachine();
	}

	public void LateUpdate()
    {
        _audioComponent.transform.rotation = _lockAudioSourceLocation;        //Make sound location constant TODO:  (might exist some better fix)

		/*if (_isActive && _mover.IsMoving && !_soulMover.isMoving)
            _avatarSoul.transform.position = this.gameObject.transform.position;   //Sticks the listener to the selected player*/
    }

	public void Update()
	{
		_stateMachine.Update();
	
		if (_isDead)
			return;

	    if (Input.GetMouseButtonUp(0) && ((_pauseMenuPanel == null) || (_pauseMenuPanel.rect.yMin == _pauseMenuStartLocation)))
	    {
	        if (EventSystem.current == null || !EventSystem.current.IsPointerOverGameObject())      //Kollar om det är ett gui element som klickats
	        {
	      
	            SelectAvatar();

	            if (_isActive)
	            {
	                Vector3 mp = Input.mousePosition;
	                Ray r = Camera.main.ScreenPointToRay(new Vector3(mp.x, mp.y, Camera.main.transform.position.y));
	                // Create a ray that starts at the camera and goes through the mouse position in world space.

	                float d = Vector3.Dot(new Vector3(0, 1, 0) - r.origin, Vector3.up)/Vector3.Dot(r.direction, Vector3.up);
	                Vector3 wp = r.origin + r.direction*d;
	                wp.y = 1; // Avoid rounding errors

	                if (debugTeleport)
	                {
	                    BaseTile destination = _gridManager.GetTile(wp);
	                    if (destination != null && destination.CanWalkOn(this))
	                    {
	                        BaseTile source = _gridManager.GetTile(_mover.Position);
	                        BaseTile.TeleportTo(this, source, destination);
	                        return;
	                    }
	                }

	                int cost;
	                Vector3 startPosition = _mover.Position;
	                // We use the mover position as start, since it can be moving (causing transform to be unreliable).
	                Vector2[] path = _pathFinder.GetPathTo(startPosition, wp, this, out cost);

	                if (path != null)
	                {
	                    _moveQueue = new Queue<Vector2>(path);
	                }
	                else
	                {
	                    // Couldn't get there, try pushing instead..
	                    Vector3 dir = wp - startPosition;
	                    int x = (int) Mathf.Sign(dir.x);
	                    int z = (int) Mathf.Sign(dir.z);
	                    if (Mathf.Abs(dir.x) > Mathf.Abs(dir.z))
	                        Move(x, 0);
	                    else if (Mathf.Abs(dir.x) < Mathf.Abs(dir.z))
	                        Move(0, z);
	                }
	            }
	        }  
	    }
        if (!_mover.IsMoving && _moveQueue != null && _moveQueue.Count > 0 && !_isFalling)
        {
            Vector2 dir = _moveQueue.Dequeue();
            Move((int)dir.x, (int)dir.y);
            if (_moveQueue.Count == 0)
            {
                _moveQueue = null;
            }
        }
	}
	
	private bool IsMoving() {
		return (_moveQueue != null && _moveQueue.Count > 0) || _mover.IsMoving;
	}
	
	private void SelectAvatar() {
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit hit;
		
		if (Physics.Raycast(ray, out hit, Mathf.Infinity))
		{
			if(hit.transform.gameObject == this.gameObject)
			{
				_isActive = true;
				
				_soulMover.MoveToAvatar(this.gameObject);
				
				_soundEffectPlayer.SetIdleTimeBool(false);
				_soundEffectPlayer.PlayAvatarSelectedSound();
			}
			else if (hit.transform.gameObject.tag == UnitTypesEnum.Player.ToString())
			{
				_soundEffectPlayer.SetIdleTimeBool(true);
				_isActive = false;
			}
		}
	}

	private void Move(int x, int z) {
		_mover.TryMove(x, z);
		if (_rotation)
			_rotation.RotateTowards(x, z);
	}

	public void KillAvatar()
    {
		if (_isDead || debugInvincible)
			return;
		
    	_isDead = true;
		_stateMachine.ChangeState((int) AvatarState.Dead);
    }

    public void SetIsFalling()
    {
        _isFalling = true;
    }
}