using UnityEngine;
using System.Collections;

public class DebugManager : MonoBehaviour {

	public bool showShortCuts = false;
	
	void Awake() {
		DontDestroyOnLoad(gameObject);
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.I)) {
			AvatarUnit[] avatars = GameObject.FindObjectsOfType<AvatarUnit>();
			foreach (AvatarUnit a in avatars)
				a.debugInvincible = !a.debugInvincible;
		} else if (Input.GetKeyDown(KeyCode.T)) {
			AvatarUnit[] avatars = GameObject.FindObjectsOfType<AvatarUnit>();
			foreach (AvatarUnit a in avatars)
				a.debugTeleport = !a.debugTeleport;
		} else if (Input.GetKeyDown(KeyCode.O)) {
			ExitTile[] exits = GameObject.FindObjectsOfType<ExitTile>();
			foreach (ExitTile t in exits) {
				t.debugOpen = !t.debugOpen;
				t.SendMessage("SetExitOpen", t.debugOpen);
			}
		} else if (Input.GetKeyDown(KeyCode.D)) {
			showShortCuts = !showShortCuts;
		} else if (Input.GetKeyDown(KeyCode.N)) {
			SceneTransition st = Helper.Find<SceneTransition>("SceneTransition");
			st.NextScene();
		} else if (Input.GetKeyDown(KeyCode.P)) {
			SceneTransition st = Helper.Find<SceneTransition>("SceneTransition");
			Player.CreatePlayer().Level -= 1;
			st.NextScene(Player.CreatePlayer().Level);
		} else if (Input.GetKeyDown(KeyCode.R)) {
			SceneTransition st = Helper.Find<SceneTransition>("SceneTransition");
			st.NextScene(Player.CreatePlayer().Level);
		}
	}
	
	void OnGUI() {
		if (!showShortCuts)
			return;
			
		GUI.Label(new Rect(20, 20, 200, 20), "I: Invincible");
		GUI.Label(new Rect(20, 40, 200, 20), "T: Teleport");
		GUI.Label(new Rect(20, 60, 200, 20), "O: Open Exit");
		GUI.Label(new Rect(20, 80, 200, 20), "N: Next Level");
		GUI.Label(new Rect(20, 100, 200, 20), "P: Previous Level");
		GUI.Label(new Rect(20, 120, 200, 20), "R: Restart Level");
		GUI.Label(new Rect(20, 140, 200, 20), "D: Toggle Debug List");
	}
}
