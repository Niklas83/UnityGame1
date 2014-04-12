using UnityEngine;
using System.Collections;

public class ExitTile : BaseTile {

	private SceneTransition _sceneTransition;
	private bool _opened;
	private int _nrAliveKeys;

	void Start() {
		_sceneTransition = Helper.Find<SceneTransition>("SceneTransition");
	}

	protected override void OnLeaved(BaseUnit unit, BaseTile nextTile) {}
	protected override void OnArrived(BaseUnit unit, BaseTile previousTile) {
		if (_opened && unit is AvatarUnit) {
			_sceneTransition.NextScene();
		}
	}

	public void Register() {
		_nrAliveKeys += 1;
		SetExitOpen(_nrAliveKeys == 0);
	}
	public void Unregister() {
		_nrAliveKeys -= 1;
		SetExitOpen(_nrAliveKeys == 0);
	}

	private void SetExitOpen(bool open) {
		Transform t = transform.FindChild("OpenEffects");
		t.gameObject.SetActive(open);
		_opened = open;
	}
}
