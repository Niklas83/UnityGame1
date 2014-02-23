using UnityEngine;
using System.Collections;

public class Menu : MonoBehaviour {

	public GUISkin guiSkin;
	public GUIStyle guiStyle;
	public float menuAlpha;

	public Vector3 littlePosition = new Vector3(390, 230, 0);
	public Vector3 bastardsPosition = new Vector3(180, 300, 0);

	// Use this for initialization
	void Start () {
	}

	float originalWidth = 2048.0f;
	float originalHeight = 1536.0f;

	bool clicked = false;
	AsyncOperation aop;
	// Update is called once per frame
	void OnGUI() {
		GUI.skin = guiSkin;

		Vector3 scale = new Vector3(Screen.width/originalWidth, Screen.height/originalHeight, 1);
		GUI.matrix = Matrix4x4.TRS(Vector3.zero, Quaternion.identity, scale);

		Color original = guiStyle.normal.textColor;
		guiStyle.alignment = TextAnchor.UpperLeft;
	
		guiStyle.normal.textColor = new Color(0, 0, 0);
		DrawText(new Vector2(8, 8));
		guiStyle.normal.textColor = original;
		DrawText(Vector2.zero);


		GUI.color = new Color(0, 0, 0, menuAlpha);
		if (menuAlpha == 0)
			return;

		guiStyle.fontSize = 70;
		guiStyle.alignment = TextAnchor.MiddleCenter;
		Vector2 size = new Vector2(800, 100);

		if (GUI.Button(new Rect((originalWidth-size.x)/2, originalHeight/2+300, size.x, size.y), "New Game", guiStyle)) {
			aop = Application.LoadLevelAsync(1);
			// aop.allowSceneActivation = false;
			/*Debug.Log(Application.CanStreamedLevelBeLoaded(1));
			clicked = true;*/
		}
		if (GUI.Button(new Rect((originalWidth-size.x)/2, originalHeight/2+450, size.x, size.y), "Continue", guiStyle)) {
			/*Debug.Log("Pressed");
			aop.allowSceneActivation = true;*/
		}
 
		/*if (clicked) {
			Debug.Log(aop.progress + " " + aop.isDone + " " + Application.isLoadingLevel);
		}*/
	}

	void DrawText(Vector2 offset) {
		guiStyle.fontSize = 130;
		GUI.Label(new Rect(littlePosition.x+offset.x, littlePosition.y+offset.y, 300, 50), "Little", guiStyle);

		guiStyle.fontSize = 300;
		GUI.Label(new Rect(bastardsPosition.x+offset.x, bastardsPosition.y+offset.y, 300, 50), "Bastards", guiStyle);
	}
}
