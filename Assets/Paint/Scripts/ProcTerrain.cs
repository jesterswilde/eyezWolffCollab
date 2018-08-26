using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProcTerrain : MonoBehaviour {

	static ProcTerrain t;
	[SerializeField]
	Terrain terrain;
	[SerializeField]
	int height;
	[SerializeField]
	int width;
	//[SerializeField]
	//Material material;
    [SerializeField]
    GameObject canvas;

	public float heightAmp = 1f;

	Texture2D heightInfo;
	TerrainData terrainData;
	float[,] heightMap;


	void Start () {
		t = this;
		terrainData = terrain.terrainData;
		heightMap = new float[width, height];
        //heightInfo = (Texture2D)canvas.GetComponent<Renderer>().material.GetTexture("_MainTex");
        //Debug.Log(heightInfo);
		GenerateTerrain ();

	}

	void GenerateTerrain()
	{
		terrainData.SetHeights(0, 0, heightMap);
	}

    void SetHeightInternal(Vector2 painter, Texture2D splashTexture)
    {
        heightInfo = (Texture2D)canvas.GetComponent<Renderer>().material.GetTexture("_MainTex");
        int x = (int)(painter.x * width - splashTexture.width / 2);
        int y = (int)(painter.y * height - splashTexture.height / 2);
        for (int i = 0; i < splashTexture.width; i++)
        {
            for (int j = 0; j < splashTexture.height; j++)
            {
                int newX = x + i;
                int newY = y + j;
                float alpha = heightInfo.GetPixel(newX, newY).a;
                heightMap[newX, newY] = alpha * heightAmp;
            }
        }
        terrainData.SetHeights(0, 0, heightMap);
	}
    public static void SetHeight(Vector2 painter, Texture2D splashTexture)
    {
        t.SetHeightInternal(painter, splashTexture);
    }
}
