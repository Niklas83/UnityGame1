using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public sealed class AvatarUnit : BaseUnit
{
	public override int LayerMask { get { return (int)(Layer.Air | Layer.Ground); } }
    // changed so u can have method handling the return value
    public override bool CanWalkOver { get { return false; } }
    public override bool CanWalkOn(string incomingUnitTag)
    {
        return CanWalkOver;
    }
	public bool debugTeleport = false;

	private Mover mMover;
	private Rotation mRotation;
	private PathFinder mPathFinder;
	private GridManager mGridManager;
	private Queue<Vector2> mMoveQueue;
    
	
    private bool IsFrozen = false;      //Can be frozen by example a "medusa statue"

    private bool CurrentlyActivePlayer = false;     //Is set to true when a character is selected
    
    private GameObject AudioComponent;
    private AudioListener AudioListener;       //sets the audiolistener to active when selected
    private Quaternion LockAudioSourceLocation;   // Sets the rotation of the character to "north" every update
	public void Start()
	{ 
		mMover = GetComponent<Mover>();
		mRotation = GetComponent<Rotation>();

		Floor floor = Helper.Find<Floor>("Floor");
		mGridManager = floor.GridManager;
		mPathFinder = new PathFinder(mGridManager);
        
	    LockAudioSourceLocation = this.gameObject.transform.rotation;
	    AudioComponent = this.gameObject.transform.FindChild("AudioComponent").gameObject;
	    AudioListener = AudioComponent.GetComponent<AudioListener>();
	}

   public void LateUpdate()
    {
        AudioComponent.transform.rotation = LockAudioSourceLocation;        //Make sound location constant TODO:  (might exist some better fix)
    }
	public void Update()
	{
	    
	    if (IsFrozen)
			return;

	    if (Input.GetMouseButtonUp(0))
	    {
            
	        //Character selection begins
	            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
	            RaycastHit hit;

	            if (Physics.Raycast(ray, out hit, Mathf.Infinity))
	            {
                    if(hit.transform.gameObject == this.gameObject)
                    {
                        CurrentlyActivePlayer = true;
                        AudioListener.enabled = true;
                    }
                    else if (hit.transform.gameObject.tag == UnitTypesEnum.Player.ToString())
                    {
                        CurrentlyActivePlayer = false;
                        AudioListener.enabled = false;
                    }
	            }
             //Character selection ends
	        
	        if (CurrentlyActivePlayer == true)
	        {
	            Vector3 mp = Input.mousePosition;
	            Ray r = Camera.main.ScreenPointToRay(new Vector3(mp.x, mp.y, Camera.main.transform.position.y));
	            // Create a ray that starts at the camera and goes through the mouse position in world space.

	            float d = Vector3.Dot(new Vector3(0, 1, 0) - r.origin, Vector3.up)/Vector3.Dot(r.direction, Vector3.up);
	            Vector3 wp = r.origin + r.direction*d;
	            wp.y = 1; // Avoid rounding errors

	            if (debugTeleport)
	            {
	                BaseTile destination = mGridManager.GetTile(wp);
	                if (destination != null && destination.CanWalkOn(this))
	                {
	                    BaseTile source = mGridManager.GetTile(mMover.Position);
	                    BaseTile.TeleportTo(this, source, destination);
	                    return;
	                }
	            }

	            int cost;
	            Vector3 startPosition = mMover.Position;
	            // We use the mover position as start, since it can be moving (causing transform to be unreliable).
	            Vector2[] path = mPathFinder.GetPathTo(startPosition, wp, this, out cost);

	            if (path != null)
	                mMoveQueue = new Queue<Vector2>(path);
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
	    

        if (!mMover.IsMoving && mMoveQueue != null && mMoveQueue.Count > 0)
        {
            Vector2 dir = mMoveQueue.Dequeue();
            Move((int) dir.x, (int) dir.y);
            if (mMoveQueue.Count == 0)
                mMoveQueue = null;
        }
	}

	private void Move(int x, int z) {
		mMover.TryMove(x, z);
		if (mRotation)
			mRotation.RotateTowards(x, z);
	}

    public void MakePlayerFrozen()
    {
        IsFrozen = true;
    }
}