using UnityEngine;
using UnityEditor;
using System.Collections;

public class SetupLevel : ScriptableWizard
{
	public int width = 15;
	public int height = 11;
	public Sprite background;
	public GameObject floorTile;
	public AudioClip audioClip;
	public GameObject player;
	
	[MenuItem("Custom/Setup Level")]
	static void CreateWizard()
	{
		ScriptableWizard.DisplayWizard("Setup Level", typeof(SetupLevel), "Setup Level");
    }

    void OnWizardCreate()
    {
		Floor floor = Helper.CreateObject<Floor>("Floor");
		GameObject parent = floor.gameObject;
		for (int x = 0; x < width; x++) {
			for (int y = 0; y < height; y++) {
				BaseTile tile = Helper.Instansiate<BaseTile>(floorTile, parent);
				tile.transform.position = new Vector3(x, 0, y);
			}
		}

		AudioPlayer audioPlayer = Helper.CreateObject<AudioPlayer>("AudioPlayer");
		AudioSource source = audioPlayer.gameObject.AddComponent<AudioSource>();
		source.clip = audioClip;

		Camera.main.transform.position = new Vector3(7, 12, 5);
		Camera.main.transform.rotation = Quaternion.AngleAxis(90, new Vector3(1, 0, 0));

		SpriteRenderer sr = Helper.CreateObject<SpriteRenderer>("Background");
		sr.sprite = background;
		sr.transform.position = new Vector3(7, -1, 5);
		sr.transform.rotation = Quaternion.AngleAxis(90, new Vector3(1, 0, 0));

		Hud hud = Helper.CreateObject<Hud>("Hud");

		GameObject boxes = new GameObject("Boxes");

		Light light = Helper.CreateObject<Light>("Sun");
		light.type = LightType.Directional;
		light.transform.rotation = Quaternion.AngleAxis(45, new Vector3(1, 1, 0));

		GameObject p = Helper.Instansiate<AvatarUnit>(player).gameObject;
		p.transform.position = new Vector3(0, 1, 0);
    }
}