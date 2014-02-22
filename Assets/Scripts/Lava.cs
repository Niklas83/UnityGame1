using UnityEngine;
using System.Collections;

public class Lava : MonoBehaviour {

	private Texture2D noiseTex;

	// Use this for initialization
	void Start () {
		noiseTex = new Texture2D(128, 128);
		float scale = 2;
		Color[] color = new Color[noiseTex.width * noiseTex.height];
		for (int x = 0; x < noiseTex.width; x++) {
			for (int y = 0; y < noiseTex.height; y++) {
				float perlinX = x/(float)noiseTex.width;
				float perlinY = y/(float)noiseTex.height;
				float value = Mathf.PerlinNoise(perlinX*scale, perlinY*scale);
				color[y * noiseTex.width + x] = new Color(value, value, value);
			}
		}

		noiseTex.SetPixels(color);
		noiseTex.Apply();
		this.renderer.material.SetTexture("_Perlin2D", noiseTex);
	}
	
	// Update is called once per frame
	void Update () {
	}
}
