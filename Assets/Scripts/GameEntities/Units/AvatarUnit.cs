using UnityEditor;
using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public partial class AvatarUnit : BaseUnit
{
	public override int LayerMask { get { return (int)(Layer.Air | Layer.Ground); } }
    
    public override bool CanWalkOver { get { return false; } }
    public override bool CanWalkOn(string incomingUnitTag)
    {
        return CanWalkOver;
    }
	public bool debugTeleport = false;

	private Mover _mover;
	private Rotation _rotation;
	private PathFinder _pathFinder;
	private GridManager _gridManager;
	private Queue<Vector2> _moveQueue;
    
    private bool _isFrozen = false;							// E.g by a medusa statue.
    private bool _isActive = false;							// Is set to true when a character is selected

    // Audio related fields
    private GameObject _audioComponent;						// Holds the audio listener with constant rotation
    private GameObject _audioListener;
    private AudioListenerMover _audioListenerMover;
    
    private Quaternion _lockAudioSourceLocation;			// Sets the rotation of the character to "north" every update
    private SoundsEffects _characterSoundEffects;			// Script containing all soundrelated _moveQueue player (Move, death, _moveQueue etc)
    
	private StateMachine _stateMachine;

	public void Start()
	{ 
		_mover = GetComponent<Mover>();
		_rotation = GetComponent<Rotation>();

		Floor floor = Helper.Find<Floor>("Floor");
		_gridManager = floor.GridManager;
		_pathFinder = new PathFinder(_gridManager);
        
        //Audio related
		_lockAudioSourceLocation = this.gameObject.transform.rotation;
	    _audioComponent = this.gameObject.transform.FindChild("AudioComponent").gameObject;
        _audioListener = GameObject.FindWithTag("TheAudioListener");

	    _audioListenerMover = _audioListener.GetComponentInChildren<AudioListenerMover>();
        _characterSoundEffects = GetComponentInChildren<SoundsEffects>();
        
		AvatarStates avatarStates = new AvatarStates(gameObject);
		_stateMachine = avatarStates.GetStateMachine();
	}

	public void LateUpdate()
    {
        _audioComponent.transform.rotation = _lockAudioSourceLocation;        //Make sound location constant TODO:  (might exist some better fix)

        if (_isActive && _mover.IsMoving && _audioListenerMover.isMoving == false)
            _audioListener.transform.position = this.gameObject.transform.position;   //Sticks the listener to the selected player
    }

	public void Update()
	{
	    if (_isFrozen)
			return;
		
		_stateMachine.Update();
		
	    if (Input.GetMouseButtonUp(0))
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
	                int x = Math.Sign(dir.x);
	                int z = Math.Sign(dir.z);
	                if (Math.Abs(dir.x) > Math.Abs(dir.z))
	                    Move(x, 0);
	                else if (Math.Abs(dir.x) < Math.Abs(dir.z))
	                    Move(0, z);
	            }
	        }
	    }

        if (!_mover.IsMoving && _moveQueue != null && _moveQueue.Count > 0)
        {
            Vector2 dir = _moveQueue.Dequeue();
            Move((int) dir.x, (int) dir.y);
            if (_moveQueue.Count == 0) {
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
				
				_audioListenerMover.MoveToSelectedPlayer(this.gameObject);
				
				_characterSoundEffects.SetIdleTimeBool(false);
				_characterSoundEffects.PlaySelectedCharacterSound();
			}
			else if (hit.transform.gameObject.tag == UnitTypesEnum.Player.ToString())
			{
				_characterSoundEffects.SetIdleTimeBool(true);
				_isActive = false;
			}
		}
	}

	private void Move(int x, int z) {
		_mover.TryMove(x, z);
		if (_rotation)
			_rotation.RotateTowards(x, z);
	}

    public void MakePlayerFrozen()
    {
        _isFrozen = true;
    }

    public override void DestroyUnit()     
    {
        if (_characterSoundEffects.regularDeathAudio != null)
        {
            GameObject deathAudioGameObject = new GameObject();

            deathAudioGameObject.transform.position = this.gameObject.transform.position;
            deathAudioGameObject.AddComponent<AudioSource>();
            AudioSource deathAudioSource = deathAudioGameObject.GetComponent<AudioSource>();

            deathAudioSource.audio.clip = _characterSoundEffects.GetRandomDeathAudioClip();

            deathAudioSource.audio.volume = _characterSoundEffects.deathVol0To1;

            if (deathAudioSource.audio.clip != null)
            {
                deathAudioSource.Play();
            }
        }
        base.DestroyUnit();
    }
}