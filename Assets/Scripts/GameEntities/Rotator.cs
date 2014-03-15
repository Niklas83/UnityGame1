using UnityEngine;
using System.Collections;

public class Rotator : MonoBehaviour {

	public GameObject parent;
	public float rotationMultiplier = 1f;
	private float parentAngle;

	public void Start() {
		parentAngle = parent.transform.rotation.eulerAngles.y;
	}

	public void Update() {
		float previousParentAngle = parentAngle;
		parentAngle = parent.transform.rotation.eulerAngles.y;

		float delta = parentAngle - previousParentAngle;

		Quaternion rotation = transform.rotation;
		Vector3 current = rotation.eulerAngles;
		current.y += delta * rotationMultiplier;
		rotation.eulerAngles = current;
		transform.rotation = rotation;
	}
}

