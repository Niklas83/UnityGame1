using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class Hud : MonoBehaviour {

	public GUISkin skin;

	private int mLevel = 1;
	private int mScore = 0;

	private enum InfoType {
		Level = 0,
		Score = 1,
	}

	private Dictionary<InfoType, Rect> mLayout;

	// Use this for initialization
	void Awake() {
		mLayout = new Dictionary<InfoType, Rect>();
		int x = 40;
		int y = 40;
		int height = 25;
		int width = 250;

		int nrElements = Enum.GetNames(typeof(InfoType)).Length;
		for (int i = 0; i < nrElements; i++) {
			mLayout.Add((InfoType) i, new Rect(x, y + i * height, width, height));
		}
	}
	
	// Update is called once per frame
	void Update() {
	}

	void OnGUI() {
		GUI.skin = skin;

		GUI.Label(mLayout[InfoType.Level], String.Format("Level: {0}", mLevel));
		GUI.Label(mLayout[InfoType.Score], String.Format("Score: {0}", mScore));
	}
}
