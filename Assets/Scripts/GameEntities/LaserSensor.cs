using UnityEngine;
using System.Collections;

public class LaserSensor : MonoBehaviour {

	private EventListener[] ObjectsToNotify;
	private float hitTime = -3;
	private bool registered;
	
	void Start() {
		ObjectsToNotify = new EventListener[1];
		ObjectsToNotify[0] = GameObject.Find("Exit").GetComponent<EventListener>();
		SetRegistered(true);
	}

	void SetRegistered(bool registered) {
		if (this.registered == registered)
			return;

		foreach (EventListener el in ObjectsToNotify)
			el.ReceiveEvent(registered ? EventMessage.Register : EventMessage.Unregister);
		this.registered = registered;
	}

	void Update() {
		float t = Time.realtimeSinceStartup - hitTime;
		t = 3 - Mathf.Clamp(t, 0, 3);
		GetComponentInChildren<Light>().range = t;

		if (Time.realtimeSinceStartup - hitTime > 3) { // 3 seconds since the last hit
			SetRegistered(true);
		}
	}

	public void OnLaserHit() {
		SetRegistered(false);
		hitTime = Time.realtimeSinceStartup;
	}
}
