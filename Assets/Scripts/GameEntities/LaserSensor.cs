using UnityEngine;
using System.Collections;

public class LaserSensor : MonoBehaviour {

	private EventListener[] _objectsToNotify;
	private float _hitTime = -3;
	private bool _registered;
	
	void Start() {
		_objectsToNotify = new EventListener[1];
		_objectsToNotify[0] = GameObject.Find("Exit").GetComponent<EventListener>();
		SetRegistered(true);
	}

	void SetRegistered(bool registered) {
		if (this._registered == registered)
			return;

		foreach (EventListener el in _objectsToNotify)
			el.ReceiveEvent(registered ? EventMessage.Register : EventMessage.Unregister);
		this._registered = registered;
	}

	void Update() {
		float t = Time.realtimeSinceStartup - _hitTime;
		t = 3 - Mathf.Clamp(t, 0, 3);
		GetComponentInChildren<Light>().range = t;

		if (Time.realtimeSinceStartup - _hitTime > 3) { // 3 seconds since the last hit
			SetRegistered(true);
		}
	}

	public void OnLaserHit() {
		SetRegistered(false);
		_hitTime = Time.realtimeSinceStartup;
	}
}
