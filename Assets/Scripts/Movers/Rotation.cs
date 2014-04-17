using UnityEngine;
using System;
using System.Collections;

public class Rotation : MonoBehaviour {

	public float rotationSpeed = 5f;
	public float shearAngle = 25f;

	public void RotateTowards(int xDir, int zDir) {
		StartCoroutine(Rotate(xDir, zDir));
	}
	
	public IEnumerator Rotate(int xDir, int zDir)
	{
		float t = 0;

		Quaternion shear = Quaternion.Euler(shearAngle, 0, 0);
		Vector3 up = shear * Vector3.up;
		Vector3 lookDirection = shear * new Vector3(xDir, 0, zDir);
		
		Quaternion target = Quaternion.LookRotation(lookDirection, up);
		Quaternion start = transform.rotation;
		
		while (t < 1f)
		{
			transform.rotation = Quaternion.Slerp(start, target, 3*t*t-2*t*t*t);
			t += Time.deltaTime * rotationSpeed;
			yield return null;
		}
		
		transform.rotation = target;
		yield return 0;
	}
}