using UnityEngine;
using System.Collections;

public class Rotator : MonoBehaviour {

	public GameObject parent;
	public float rotationMultiplier = 1f;
	
	private float _parentAngle;

	public void Start() {
		_parentAngle = parent.transform.rotation.eulerAngles.y;
	}

	public void Update() {
		float previousParentAngle = _parentAngle;
		_parentAngle = parent.transform.rotation.eulerAngles.y;

		float delta = _parentAngle - previousParentAngle;

		Quaternion rotation = transform.rotation;
		Vector3 current = rotation.eulerAngles;
		current.y += delta * rotationMultiplier;
		rotation.eulerAngles = current;
		transform.rotation = rotation;
	}
}
