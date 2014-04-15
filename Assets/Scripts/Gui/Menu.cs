using UnityEngine;
using System.Collections;

public class Menu : MonoBehaviour {

	public GUISkin guiSkin;
	public float menuAlpha;

	public Vector3 littlePosition = new Vector3(390, 230, 0);
	public Vector3 bastardsPosition = new Vector3(180, 300, 0);
	
	private Player _player;
	
	void Start()
	{
		_player = Player.CreatePlayer();
	}

	void NewGame() 
	{
		_player.saveData = new SaveData();
		SceneTransition st = Helper.Find<SceneTransition>("SceneTransition");
		st.NextScene();
	}
	
	void Continue() 
	{
		SceneTransition st = Helper.Find<SceneTransition>("SceneTransition");
		st.NextScene(_player.Level);
	}

	void OnGUI() {
		GUI.skin = guiSkin;
		GUIStyle labelStyle = guiSkin.GetStyle("label");

		float width = Defines.RESOLUTION_WIDTH;
		float height = Defines.RESOLUTION_HEIGHT;

		Vector3 scale = new Vector3(Screen.width/width, Screen.height/height, 1);
		GUI.matrix = Matrix4x4.TRS(Vector3.zero, Quaternion.identity, scale);

		Color original = labelStyle.normal.textColor;
		DrawText(new Vector2(8, 8), new Color(0, 0, 0)); // Draw shadow
		DrawText(Vector2.zero, original);

		GUI.color = new Color(0, 0, 0, menuAlpha);
		if (menuAlpha > 0) {
			GUILayout.BeginArea(new Rect(width/2-300, height/2+300, 600, 300));
			if (GUILayout.Button("New Game"))
				NewGame();
			if (_player.saveData != null && GUILayout.Button("Continue"))
				Continue();
			GUILayout.EndArea();
		}
	}

	void DrawText(Vector2 offset, Color color) {
		GUIStyle labelStyle = guiSkin.GetStyle("label");
		labelStyle.normal.textColor = color;

		labelStyle.fontSize = 130;
		GUI.Label(new Rect(littlePosition.x+offset.x, littlePosition.y+offset.y, 0, 0), "Little");

		labelStyle.fontSize = 300;
		GUI.Label(new Rect(bastardsPosition.x+offset.x, bastardsPosition.y+offset.y, 0, 0), "Bastards");
	}

	/*bool IsMouseOver()
	{
		return Event.current.type == EventType.Repaint && 
			GUILayoutUtility.GetLastRect().Contains(Event.current.mousePosition);
	}*/
}
