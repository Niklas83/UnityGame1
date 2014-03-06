using UnityEngine;
using UnityEditor;
using System.Collections;

public class ReplaceTexture : ScriptableWizard
{
	public float tileSize = 1;
	public Texture texture;
	public GameObject[] Objects;
	
	[MenuItem("Custom/Replace Texture")]
	static void CreateWizard()
	{
		ScriptableWizard.DisplayWizard("Replace Texture", typeof(ReplaceTexture), "Replace Texture");
	}

    void OnWizardCreate()
    {
		foreach (GameObject go in Objects)
		{
			Vector2 textureScale = new Vector2(tileSize, tileSize);
			int indexX = Mathf.RoundToInt(1 / textureScale.x);
			int indexY = Mathf.RoundToInt(1 / textureScale.y);

			int randomX = Random.Range(0, indexX);
			int randomY = Random.Range(0, indexY);

			float x = Mathf.Sign(Random.value - 0.5f);
			float y = Mathf.Sign(Random.value - 0.5f);

			Vector2 scale  = new Vector2(textureScale.x * x, textureScale.y * y);
			Vector2 offset = new Vector2(randomX * textureScale.x, randomY * textureScale.y);

			Material tempMaterial = new Material(go.renderer.sharedMaterial);

			tempMaterial.mainTexture = texture;
			tempMaterial.mainTextureScale = scale;
			tempMaterial.mainTextureOffset = offset;

			go.renderer.sharedMaterial = tempMaterial;
		}
    }
}