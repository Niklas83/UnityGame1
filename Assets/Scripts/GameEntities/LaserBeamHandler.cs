using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LaserBeamHandler : MonoBehaviour
{
	public GameObject LaserBeamPrefab;
	public EventMessage onHitMessage;
	private List<LineRenderer> beamCache;

    public void Start() {
		beamCache = new List<LineRenderer>();
		GetBeam(0);
	}

	private LineRenderer GetBeam(int index) {
		LineRenderer lr = null;
		if (index >= beamCache.Count) {
			GameObject go = GameObject.Instantiate(LaserBeamPrefab) as GameObject;
			go.transform.parent = transform.parent;
			go.transform.localPosition = Vector3.zero;
			lr = go.GetComponent<LineRenderer>();
			beamCache.Add(lr);
		} else
			lr = beamCache[index];

		return lr;
	}
	
	void Update() {
		foreach(LineRenderer lr in beamCache)
			lr.enabled = false;

		Vector3 forward = transform.rotation * Vector3.forward;
		ShootBeam(transform, forward, 0);
	}

	public void ShootBeam(Transform t, Vector3 direction, int index) {
		Vector3 position = t.position;

		if (index > 100)
			return;

		LineRenderer lr = GetBeam(index);
		lr.enabled = true;
		lr.SetPosition(0, position);
		
		RaycastHit hit;
		if (Physics.Raycast(position, direction, out hit)) {
			Transform hitTrans = hit.collider.transform;

			direction = Vector3.Reflect(direction, hit.normal); // hitTrans.rotation * Vector3.forward;
			lr.SetPosition(1, hit.point); // hitTrans.position);

			EventListener el = hitTrans.gameObject.GetComponent<EventListener>();
			if (el)
				el.ReceiveEvent(onHitMessage);

			if (hitTrans.gameObject.GetComponent<BeamReflector>())
				ShootBeam(hitTrans, direction, index+1);
		} else {
			lr.SetPosition(1, position + direction * 50);
		}
	}
}
