using UnityEngine;
using System.Collections;

public class Menu : MonoBehaviour {

	public GUISkin guiSkin;
	public float menuAlpha;
	public float fadeAlpha = 1;
	public float timer = 0;

	public Vector3 littlePosition = new Vector3(390, 230, 0);
	public Vector3 bastardsPosition = new Vector3(180, 300, 0);

	Texture2D fade;
	AsyncOperation asyncOp;
	AudioPlayer audioPlayer;

	void Start() {
		fade = new Texture2D(1, 1);
		fade.SetPixel(0, 0, new Color(0, 0, 0));
		fade.Apply();

		audioPlayer = Helper.Find<AudioPlayer>("AudioPlayer");
	}

	void NewGame() {
		if (asyncOp != null)
			return;

		asyncOp = Application.LoadLevelAsync(1);
		asyncOp.allowSceneActivation = false;

		timer = 0;

		audioPlayer.FadeOut(2f/3f);

		Animator a = this.gameObject.GetComponent<Animator>();
		a.Play("FadeOut");
	}
	void Continue() {
		Debug.LogWarning("Continue not implemented!");
	}

	void Update() {
		if (asyncOp == null)
			return;

		timer += Time.deltaTime;
		if (timer >= 2f/3f)
			asyncOp.allowSceneActivation = true;
	}

	void OnGUI() {
		GUI.skin = guiSkin;
		GUIStyle labelStyle = guiSkin.GetStyle("label");
		GUIStyle buttonStyle = guiSkin.GetStyle("button");

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
			if (GUILayout.Button("Continue"))
				Continue();
			GUILayout.EndArea();
		}

		GUI.color = new Color(0, 0, 0, fadeAlpha);
		GUI.DrawTexture(new Rect(0, 0, width+1, height+1), fade);
	}

	void DrawText(Vector2 offset, Color color) {
		GUIStyle labelStyle = guiSkin.GetStyle("label");
		labelStyle.normal.textColor = color;

		labelStyle.fontSize = 130;
		GUI.Label(new Rect(littlePosition.x+offset.x, littlePosition.y+offset.y, 0, 0), "Little");

		labelStyle.fontSize = 300;
		GUI.Label(new Rect(bastardsPosition.x+offset.x, bastardsPosition.y+offset.y, 0, 00), "Bastards");
	}

	/*bool IsMouseOver()
	{
		return Event.current.type == EventType.Repaint && 
			GUILayoutUtility.GetLastRect().Contains(Event.current.mousePosition);
	}*/
}
