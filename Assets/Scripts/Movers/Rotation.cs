using UnityEngine;
using System;
using System.Collections;

public class Rotation : MonoBehaviour {

	public float RotationSpeed = 5f;

	public void RotateTowards(int xDir, int zDir) {
		StartCoroutine(Rotate(xDir, zDir));
	}
	
	public IEnumerator Rotate(int xDir, int zDir)
	{
		// mIsRotating = true;
		float t = 0;

		Quaternion target = Quaternion.LookRotation(new Vector3(xDir, 0, zDir));
		Quaternion start = transform.rotation;
		
		while (t < 1f)
		{
			transform.rotation = Quaternion.Slerp(start, target, 3*t*t-2*t*t*t);
			t += Time.deltaTime * RotationSpeed;
			yield return null;
		}
		
		transform.rotation = target;
		// mIsRotating = false;
		
		yield return 0;
	}
}