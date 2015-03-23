﻿using UnityEngine;
using System.Collections;

public class SceneTransition : MonoBehaviour {

	public float fadeAlpha = 1;

	private Texture2D _fadeTexture;
#if USE_UNITY_PRO
	private AsyncOperation asyncOp;
#else
	private AsyncOperationWrapper _asyncOp;
#endif
	private AudioPlayerAdvanced _audioPlayerAdvanced;
	private AudioPlayer _audioPlayer;
	private Animator _animator;
	private Player _player;

	void Start() {
		_fadeTexture = new Texture2D(1, 1);
		_fadeTexture.SetPixel(0, 0, new Color(0, 0, 0));
		_fadeTexture.Apply();

		_animator = gameObject.GetComponent<Animator>();
		if (GameObject.Find("AudioPlayerAdvanced") != null)
			_audioPlayerAdvanced = Helper.Find<AudioPlayerAdvanced>("AudioPlayerAdvanced");
		else
			_audioPlayer = Helper.Find<AudioPlayer>("AudioPlayer");

		_player = Player.CreatePlayer();
		
		_animator.Play("FadeIn");
	}

	public void NextScene() {
		_player.Level = _player.Level + 1;
		NextScene(_player.Level);
		_player.Save();
	}
	public void NextScene(int nextLevel) 
	{
		DebugAux.Assert(_asyncOp == null, "Scene transition already in progress!");
		Log.debug("SceneTransition", "Set next level {0}", nextLevel);

#if USE_UNITY_PRO
		asyncOp = Application.LoadLevelAsync(nextLevel); // TODO: Scene name or index!
#else
		_asyncOp = new AsyncOperationWrapper(nextLevel);
#endif
		_asyncOp.allowSceneActivation = false; // Should wait for the fade.

		if (_audioPlayerAdvanced != null)
			_audioPlayerAdvanced.FadeOut(1f/4f);
		else
			_audioPlayer.FadeOut(1f/4f);

		Animator a = this.gameObject.GetComponent<Animator>();
		a.Play("FadeOut");
	}

    public void NextSceneString(string nextLevel)
    {
        DebugAux.Assert(_asyncOp == null, "Scene transition already in progress!");
        Log.debug("SceneTransition", "Set next level {0}", nextLevel);

        Application.LoadLevelAsync(nextLevel); 

        if (_audioPlayerAdvanced != null)
            _audioPlayerAdvanced.FadeOut(1f / 4f);
        else
            _audioPlayer.FadeOut(1f / 4f);

        Animator a = this.gameObject.GetComponent<Animator>();
        a.Play("FadeOut");
    }

    public IEnumerator LoadLevelWithDelay(float delay, string sceneName)
    {
        DebugAux.Assert(_asyncOp == null, "Scene transition already in progress!");
        Log.debug("SceneTransition", "Set next level {0}", Application.loadedLevelName);

        yield return new WaitForSeconds(delay);                  //restart delay
        Application.LoadLevelAsync(sceneName);

        if (_audioPlayerAdvanced != null)
            _audioPlayerAdvanced.FadeOut(1f / 4f);
        else
            _audioPlayer.FadeOut(1f / 4f);

        Animator a = this.gameObject.GetComponent<Animator>();
        a.Play("FadeOut");
    }

	void Update() {
		if (_asyncOp == null)
			return;

		if (fadeAlpha >= 1.0f) // Done fading
			_asyncOp.allowSceneActivation = true;
	}

	void OnGUI() {
		/*float width = Defines.RESOLUTION_WIDTH;
		float height = Defines.RESOLUTION_HEIGHT;
		Vector3 scale = new Vector3(Screen.width/width, Screen.height/height, 1);
		GUI.matrix = Matrix4x4.TRS(Vector3.zero, Quaternion.identity, scale);*/

		GUI.color = new Color(0, 0, 0, fadeAlpha);
		GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), _fadeTexture);
	}

#if !USE_UNITY_PRO
	// This is to keep the roughly the same interface as AsyncOperation. So that we can change to actual async later.
	private class AsyncOperationWrapper {
		private int _nextLevel;
		
		public AsyncOperationWrapper(int nextLevel) {
			_nextLevel = nextLevel;
		}
		public bool allowSceneActivation { 
			get { return true; } 
			set { 
				if (value)
					Application.LoadLevel(_nextLevel);
			}
		}
	}
#endif
}
