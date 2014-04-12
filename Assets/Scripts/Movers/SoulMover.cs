using UnityEngine;
using System.Collections;

// Note that this class does not inherit from BaseMover, as this mover does not use the grid
public class SoulMover : MonoBehaviour 
{
    public float movementSpeed;
    public bool isMoving = false;
    
    private GameObject _targetAvatar;
    private Vector3 _velocity;
    private float _time;

    public void MoveToAvatar(GameObject avatar)
    {
		transform.parent = null;
    	_targetAvatar = avatar;
    	_time = 0;
    	if (!isMoving)
			StartCoroutine(MoveTowardsAvatar());
    }

	private IEnumerator MoveTowardsAvatar()
    {
        isMoving = true;
        bool reachedTarget = false;
		
		_velocity = Vector3.Normalize(_targetAvatar.transform.position - transform.position) * 2;
		
		while (!reachedTarget) {
			Vector3 toTarget = _targetAvatar.transform.position - transform.position;
			float distanceSquared = Vector3.SqrMagnitude(toTarget);
			reachedTarget = distanceSquared < 0.25f;
			
			if (!reachedTarget) {
				Vector3 forceDirection = Vector3.Normalize(toTarget) * 10;
				_time += Time.deltaTime;
				_velocity = Vector3.Lerp(_velocity + forceDirection * Time.deltaTime, forceDirection, Mathf.Pow(_time, 3));
				transform.position += _velocity * Time.deltaTime;
				yield return null;
			}
        }
		
		transform.parent = _targetAvatar.transform;
		
		transform.localPosition = Vector3.zero;
		transform.localRotation = Quaternion.Euler(Vector3.zero);
        
        isMoving = false;
        yield return 0;
    }
}
