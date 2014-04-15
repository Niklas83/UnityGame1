using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour
{
	public string name;
	public int score;
	public SaveData saveData;
	
	public int Level { get { return saveData.level; } set { saveData.level = value; } }

	void Awake() {
		DontDestroyOnLoad(gameObject);
	}

	public void Save() {
		saveData.Save();
	}
	
	public static Player CreatePlayer() {
		bool hasPlayer = GameObject.Find("Player") != null;
		Player player;
		if (hasPlayer) {
			player = Helper.Find<Player>("Player");
		} else {
			player = Helper.CreateObject<Player>("Player");
			player.saveData = SaveData.Load();
		}
		
		return player;
	}
}
