using UnityEngine;
using UnityEditor;
using System.Collections;

public class SetupLevel : ScriptableWizard
{
	public int width = 15;
	public int height = 11;
	public float floorTexureScale = 1;
	public Sprite background;
	public GameObject[] floorTiles;
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
		Vector2 textureScale = new Vector2(floorTexureScale, floorTexureScale);
		for (int x = 0; x < width; x++) {
			for (int y = 0; y < height; y++) {
				int index = Random.Range(0, floorTiles.Length);
				BaseTile tile = Helper.Instansiate<BaseTile>(floorTiles[index], parent);
				tile.renderer.sharedMaterial.mainTextureScale = textureScale;
				tile.transform.position = new Vector3(x, 0, y);
				/*GameObject go = PrefabUtility.InstantiatePrefab(floorTiles[index]) as GameObject;
				go.transform.parent = parent.transform;
				go.renderer.material.mainTextureScale = textureScale;
				go.transform.position = new Vector3(x, 0, y);*/
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

		Helper.CreateObject<Hud>("Hud");
		new GameObject("Boxes");

		Light light = Helper.CreateObject<Light>("Sun");
		light.type = LightType.Directional;
		light.transform.rotation = Quaternion.AngleAxis(45, new Vector3(1, 1, 0));

		GameObject p = PrefabUtility.InstantiatePrefab(player) as GameObject;
		p.transform.position = new Vector3(0, 1, 0);
		
		Object prefab = AssetDatabase.LoadAssetAtPath("Assets/Prefabs/Scene/SceneTransition.prefab", typeof(GameObject));
		PrefabUtility.InstantiatePrefab(prefab);
    }
}