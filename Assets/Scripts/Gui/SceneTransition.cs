﻿using UnityEngine;
using System.Collections;

public class SceneTransition : MonoBehaviour {

	public float fadeAlpha = 1;

	private Texture2D fadeTexture;
#if USE_UNITY_PRO
	private AsyncOperation asyncOp;
#else
	private AsyncOperationWrapper asyncOp;
#endif
	private AudioPlayer audioPlayer;
	private Animator animator;

	void Start() {
		fadeTexture = new Texture2D(1, 1);
		fadeTexture.SetPixel(0, 0, new Color(0, 0, 0));
		fadeTexture.Apply();

		animator = gameObject.GetComponent<Animator>();
		audioPlayer = Helper.Find<AudioPlayer>("AudioPlayer");

		animator.Play("FadeIn");
	}

	public void NextScene() {
		if (asyncOp != null)
			return;

#if USE_UNITY_PRO
		asyncOp = Application.LoadLevelAsync(Application.loadedLevel + 1); // TODO: Scene name or index!
#else
		asyncOp = new AsyncOperationWrapper();
#endif
		asyncOp.allowSceneActivation = false; // Should wait for the fade.

		audioPlayer.FadeOut(2f/3f);

		Animator a = this.gameObject.GetComponent<Animator>();
		a.Play("FadeOut");
	}

	void Update() {
		if (asyncOp == null)
			return;

		if (fadeAlpha >= 1.0f) // Done fading
			asyncOp.allowSceneActivation = true;
	}

	void OnGUI() {
		/*float width = Defines.RESOLUTION_WIDTH;
		float height = Defines.RESOLUTION_HEIGHT;
		Vector3 scale = new Vector3(Screen.width/width, Screen.height/height, 1);
		GUI.matrix = Matrix4x4.TRS(Vector3.zero, Quaternion.identity, scale);*/

		GUI.color = new Color(0, 0, 0, fadeAlpha);
		GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), fadeTexture);
	}

#if !USE_UNITY_PRO
	// This is to keep the roughly the same interface as AsyncOperation. So that we can change to actual async later.
	private class AsyncOperationWrapper {
		public bool allowSceneActivation { 
			get { return true; } 
			set { 
				if (value)
					Application.LoadLevel(Application.loadedLevel + 1);
			}
		}
	}
#endif
}
