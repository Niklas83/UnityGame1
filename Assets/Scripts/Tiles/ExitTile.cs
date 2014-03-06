using UnityEngine;
using System.Collections;

public class ExitTile : BaseTile {

	private SceneTransition mSceneTransition;
	public bool mOpened;
	public int mNrAliveKeys;

	void Start() {
		mSceneTransition = Helper.Find<SceneTransition>("SceneTransition");
	}

	protected override void OnLeaved(BaseUnit iUnit, BaseTile iNextTile) {}
	protected override void OnArrived(BaseUnit iUnit, BaseTile iPreviousTile) {
		if (mOpened && iUnit is AvatarUnit) {
			mSceneTransition.NextScene();
		}
	}

	public void Register() {
		mNrAliveKeys += 1;
	}
	public void Unregister() {
		mNrAliveKeys -= 1;
		if (mNrAliveKeys == 0)
			OpenExit();
	}

	public void OpenExit() {
		Transform t = gameObject.transform.GetChild(0);
		t.gameObject.SetActive(true);
		mOpened = true;
	}
}
