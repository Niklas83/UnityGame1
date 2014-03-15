using UnityEngine;
using System.Collections;

public class ExitTile : BaseTile {

	private SceneTransition mSceneTransition;
	private bool mOpened;
	private int mNrAliveKeys;

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
		SetExitOpen(mNrAliveKeys == 0);
	}
	public void Unregister() {
		mNrAliveKeys -= 1;
		SetExitOpen(mNrAliveKeys == 0);
	}

	private void SetExitOpen(bool open) {
		Transform t = gameObject.transform.GetChild(0);
		t.gameObject.SetActive(open);
		mOpened = open;
	}
}
