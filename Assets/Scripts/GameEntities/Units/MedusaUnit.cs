using System;
using UnityEngine;
using System.Collections;

public class MedusaUnit : BaseUnit
{
	public override int LayerMask { get { return (int)(Layer.Air | Layer.Ground); } }

    public bool shootRight = true;
    public bool shootLeft = true;
    public bool shootUp = true;
    public bool shootDown = true;

	public override bool CanWalkOver { get { return false; } }
	public override bool CanWalkOn(string incomingUnitTag)
	{
	    return CanWalkOver;
	}

    private bool _startedToShoot = false;    //If this is set, the raycasting will be done slower to enable flame animation
    private bool _startedShootAtemptSequence = false;      //To prevent the Update to constantly spam (could be given a more suitable name)
    private MedusaRay _medusaRayScript;
    private LineRenderer _animatedBeam;               //The visual flame that travels towards the crate or player
    private GameObject[] _playerAndObsticle;             //The value that gets returned from the medusa ray

    //Blasting
    private float _incrementalLength = 0;
    private GameObject _playerGameObject;
    private GameObject _obsticleGameObject;
    private float _distanceToPlayer;         //Distance to player from from medusa
    private float _distanceToObsticle;       //Distance to box/tower from from medusa
    private Vector3 _playerLocationToShotAt;
    private Vector3 _boxLocationToShotAt = new Vector3();
    private bool _executeBlastSequence = false;
    private bool _startedBurningBabySequence = false;            //Started shooting at a player
    private bool _startedBurningBoxSequence = false;         //Started shooting at a box
    private Vector3 _previousPlayerLocationToShotAt; //If these previous locations are the same as the current location atempting to shot at it will not fire
    private Vector3 _previousBoxLocationToShotAt;    //If these previous locations are the same as the current location atempting to shot at it will not fire
    private Mover _boxUnitMover;               //Script to check if the unit is currently moving
    private bool _missingMoverScript = false;    //If the the "Box"/Obsticle is missing a mover script it shall blast anyway
    private bool _hasShooten = false;            //If the medusaunit has fired this is set so it dosent keep fireing

    private void SetStartedToShoot(bool isShooting)          //Used in medusa ray script called with SendMessage method to tell that a blast has been initialized
    {
        _startedToShoot = isShooting;
    }

    public void Start()
    {

        _medusaRayScript = GetComponent<MedusaRay>();
        _animatedBeam = this.gameObject.GetComponentInChildren<LineRenderer>();
	}
	
	void Update () {
        if (_startedShootAtemptSequence == false)
	    {
            _startedShootAtemptSequence = true;
	        StartCoroutine(StartShooting(shootRight, shootLeft, shootUp, shootDown));
	    }
	}

    public IEnumerator StartShooting(bool shootRight, bool shootLeft, bool shootUp, bool shootDown)
    {
        if (_startedToShoot == true && _executeBlastSequence!= true)
        {
            if (_animatedBeam.enabled == false)
            {
                _animatedBeam.enabled = true;
            }
            ExecuteBlast();
                 
        }
        else if (_startedBurningBabySequence != true && _startedBurningBoxSequence != true)
        {
            _executeBlastSequence = false;
           _playerAndObsticle = _medusaRayScript.Blast(shootRight, shootLeft, shootUp, shootDown);

            if (_playerAndObsticle == null || _playerAndObsticle[0] == null)
            {
                _hasShooten = false;
            }

        }
        else if (_startedBurningBabySequence == true)
        {
            StartCoroutine(BurnBabyBurn());
        }
        else if (_startedBurningBoxSequence == true)
        {
            StartCoroutine(BurnBoxBurn());
        }

        _startedShootAtemptSequence = false;
        yield return 0;
    }

    public void ExecuteBlast()
    {
        _executeBlastSequence = true;    
        if (_playerAndObsticle != null)
        {
            if (_playerAndObsticle[0] != null && _playerAndObsticle[0].tag.Equals(UnitTypesEnum.Player.ToString()))       //checks if first object is of player type
            {
                _playerGameObject = _playerAndObsticle[0];
                _distanceToPlayer = Vector3.Distance(this.transform.position, _playerGameObject.transform.position);      
            }
            else
            {
                _hasShooten = false;
            }

            if (_playerGameObject != null && _playerAndObsticle[1] != null && (_playerAndObsticle[1].tag.Equals(UnitTypesEnum.Box.ToString()) || _playerAndObsticle[1].tag.Equals(UnitTypesEnum.ProjectileShooter.ToString())))
            {
                _obsticleGameObject = _playerAndObsticle[1];
                _boxUnitMover = _obsticleGameObject.GetComponent<Mover>();
                if (_boxUnitMover == null)
                {
                    _missingMoverScript = true;
                }
                _distanceToObsticle = Vector3.Distance(this.transform.position, _obsticleGameObject.transform.position);
            }
        }
        else
        {
            _hasShooten = false;
        }
        
        if (_playerAndObsticle != null && _playerAndObsticle[1] != null)
        {
            if (_distanceToObsticle > _distanceToPlayer)
            {
                _playerGameObject.SendMessage("MakePlayerFrozen");
                _playerLocationToShotAt = this.transform.position - _playerGameObject.transform.position;       //Target is player
            }
            else
            {
                _boxLocationToShotAt = this.transform.position - _obsticleGameObject.transform.position;     //Target is box
            }
        }
        else if (_playerAndObsticle != null)
        {
            _playerGameObject.SendMessage("MakePlayerFrozen");
            _playerLocationToShotAt = this.transform.position - _playerGameObject.transform.position;
            _previousBoxLocationToShotAt = new Vector3();
        }

        if (_playerAndObsticle != null && (_playerAndObsticle[1] == null || _boxLocationToShotAt == new Vector3() || _distanceToObsticle > _distanceToPlayer)  && (_hasShooten == false || _previousPlayerLocationToShotAt != _playerLocationToShotAt))
        {
            int roundedPlayerX = (int)Math.Round(_playerLocationToShotAt.x, 0);
            int roundedPlayerZ = (int)Math.Round(_playerLocationToShotAt.z, 0);

            int roundedBoxX = (int)Math.Round(_boxLocationToShotAt.x, 0);
            int roundedBoxZ = (int)Math.Round(_boxLocationToShotAt.z, 0);

            _previousPlayerLocationToShotAt = new Vector3(roundedPlayerX, _playerLocationToShotAt.y, roundedPlayerZ);        //saves the location that was just fired at

            _previousBoxLocationToShotAt = new Vector3(roundedBoxX, _boxLocationToShotAt.y, roundedBoxZ);        //saves the location that was just fired at

            StartCoroutine(BurnBabyBurn()); //Sends out the flame from the medusa statue towards the player
        }
                                                                                                                                                                                        //REMOVE PREVIOUS CHECK IF WE DONT WANT TO SHOOT AGAIN WHEN BOXED HAS MOVED
        else if (_playerAndObsticle != null && _playerAndObsticle[1] != null && ((_boxUnitMover != null && _boxUnitMover.IsMoving == false) || _missingMoverScript == true) && (_hasShooten == false || _previousBoxLocationToShotAt != _boxLocationToShotAt))
        {
            int roundedX = (int)Math.Round(_playerLocationToShotAt.x, 0);
            int roundedZ = (int)Math.Round(_playerLocationToShotAt.z, 0);
            
            int roundedBoxX = (int)Math.Round(_boxLocationToShotAt.x, 0);
            int roundedBoxZ = (int)Math.Round(_boxLocationToShotAt.z, 0);

            _previousPlayerLocationToShotAt = new Vector3(roundedX, _playerLocationToShotAt.y, roundedZ);        //saves the location that was just fired at

            _previousBoxLocationToShotAt = new Vector3(roundedBoxX, _boxLocationToShotAt.y, roundedBoxZ);        //saves the location that was just fired at

            StartCoroutine(BurnBoxBurn()); //Sends out the flame from the medusa statue
        }
    }

    public IEnumerator BurnBabyBurn()
    {
        _startedBurningBabySequence = true;

        var roundedIncrementalLength = _incrementalLength * 0.95;         //As of the raycasting hitting the collider, some adjustment regardinga ranges need to be made to make it shoot every time
       
        //Shoots down on PLAYER
        if ((_playerLocationToShotAt.x > -0.2 && _playerLocationToShotAt.x < 0.2) && _playerLocationToShotAt.z > 0)
        {
            if (roundedIncrementalLength >= (_playerLocationToShotAt.z + 1) * -1)
            {
                _animatedBeam.SetPosition(0, new Vector3(_playerLocationToShotAt.x * -1, 0, _incrementalLength));
                _incrementalLength = this._incrementalLength - 1f;
            }
            else
            {
                _playerGameObject.SendMessage("DestroyUnit");     //Destroys the player
            }
            yield return new WaitForSeconds(0.25f);
        }

        //Shoots up on PLAYER
        if ((_playerLocationToShotAt.x > -0.2 && _playerLocationToShotAt.x < 0.2) && _playerLocationToShotAt.z < 0)
        {
            if (roundedIncrementalLength <= (_playerLocationToShotAt.z - 1) * -1)
            {
                _animatedBeam.SetPosition(0, new Vector3(_playerLocationToShotAt.x * -1, 0, _incrementalLength));
                _incrementalLength = this._incrementalLength + 1f;
            }
            else
            {
                _playerGameObject.SendMessage("DestroyUnit");     //Destroys the player
            }
            yield return new WaitForSeconds(0.25f);
        }

        //Shoots right on PLAYER
        if ((_playerLocationToShotAt.z > -0.2 && _playerLocationToShotAt.z < 0.2) && _playerLocationToShotAt.x < 0)
        {
            if (roundedIncrementalLength <= (_playerLocationToShotAt.x - 1) * -1)
            {
                _animatedBeam.SetPosition(0, new Vector3(_incrementalLength, 0, _playerLocationToShotAt.z * -1));
                _incrementalLength = this._incrementalLength + 1f;
            }
            else
            {
                _playerGameObject.SendMessage("DestroyUnit");     //Destroys the player
            }
            yield return new WaitForSeconds(0.25f);
        }

        //Shoots left on PLAYER
        if ((_playerLocationToShotAt.z > -0.2 && _playerLocationToShotAt.z < 0.2) && _playerLocationToShotAt.x > 0)
        {
            if (roundedIncrementalLength >= (_playerLocationToShotAt.x + 1) * -1)
            {
                _animatedBeam.SetPosition(0, new Vector3(_incrementalLength, 0, _playerLocationToShotAt.z * -1));
                _incrementalLength = this._incrementalLength - 1f;
            }
            else
            {
                _playerGameObject.SendMessage("DestroyUnit");     //Destroys the player
            }
            yield return new WaitForSeconds(0.25f);
        }
        
        _hasShooten = true; 

        ResetFields();
    }



    public IEnumerator BurnBoxBurn()
    {
        _startedBurningBoxSequence = true;

        //Shoots down on BOX
        if ((_boxLocationToShotAt.x > -0.2 && _boxLocationToShotAt.x < 0.2) && _boxLocationToShotAt.z > 0)
        {
            if (_incrementalLength > (_boxLocationToShotAt.z + 1) * -1)
            {
                _animatedBeam.SetPosition(0, new Vector3(_boxLocationToShotAt.x * -1, 0, _incrementalLength));
                _incrementalLength = this._incrementalLength - 1f;
            }

            //Place box damage code here (if we want that)

            yield return new WaitForSeconds(0.25f);
        }

        //Shoots up on BOX
        if ((_boxLocationToShotAt.x > -0.2 && _boxLocationToShotAt.x < 0.2) && _boxLocationToShotAt.z < 0)
        {
            if (_incrementalLength < (_boxLocationToShotAt.z ) * -1)
            {
                _animatedBeam.SetPosition(0, new Vector3(_boxLocationToShotAt.x * -1, 0, _incrementalLength));
                _incrementalLength = this._incrementalLength + 1f;
            }

            //Place box damage code here (if we want that)

            yield return new WaitForSeconds(0.25f);
        }

        //Shoots left on BOX
        if ((_boxLocationToShotAt.z > -0.2 && _boxLocationToShotAt.z < 0.2) && _boxLocationToShotAt.x > 0)
        {
            if (_incrementalLength > (_boxLocationToShotAt.x + 1) * -1)
            {
                _animatedBeam.SetPosition(0, new Vector3(_incrementalLength, 0, _boxLocationToShotAt.z * -1));
                _incrementalLength = this._incrementalLength - 1f;
            }

            //Place box damage code here (if we want that)

            yield return new WaitForSeconds(0.25f);
        }

        //Shoots right on BOX
        if ((_boxLocationToShotAt.z > -0.2 && _boxLocationToShotAt.z < 0.2) && _boxLocationToShotAt.x < 0)
        {
            if (_incrementalLength < (_boxLocationToShotAt.x) * -1)
            {
                _animatedBeam.SetPosition(0, new Vector3(_incrementalLength, 0, _boxLocationToShotAt.z * -1));
                _incrementalLength = this._incrementalLength + 1f;
            }

            //Place box damage code here (if we want that)

            yield return new WaitForSeconds(0.25f);
        }


        _hasShooten = true;
        ResetFields();
    }

    private void ResetFields()      //Resets all the values to prepare for the next blast
    {
        _animatedBeam.enabled = false;
        _incrementalLength = 0;
        _animatedBeam.SetPosition(0, new Vector3());
        _startedBurningBabySequence = false;
        _startedBurningBoxSequence = false;
        _executeBlastSequence = false;
        _playerGameObject = null;
        _obsticleGameObject = null;
        _playerAndObsticle = null;
        _distanceToObsticle = 10000;                 
        _distanceToPlayer = 10000;
        _playerLocationToShotAt = new Vector3();
        _boxLocationToShotAt = new Vector3();
        SetStartedToShoot(false);
        _missingMoverScript = false;
    }



































}
