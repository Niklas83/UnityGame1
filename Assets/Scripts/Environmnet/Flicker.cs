using UnityEngine;
using System.Collections;

public class Flicker : MonoBehaviour {

	public float speed = 2;
	public float amplitude = 0.1f;

	private Vector3 _originalPosition;
	private float _seed;

	void Start () {
		_originalPosition = transform.position;
		_seed = Random.value * 10;
	}
	
	void Update () {
		float heightX = Mathf.PerlinNoise(Time.time * speed, _seed);
		float x = (heightX - 0.5f) * amplitude;
		
		float heightZ = Mathf.PerlinNoise(Time.time * speed, _seed*2);
		float z = (heightZ - 0.5f) * amplitude;
		transform.position = _originalPosition + new Vector3(x, 0, z);
	}
}
