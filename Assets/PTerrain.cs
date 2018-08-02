using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PTerrain : MonoBehaviour {

	static PTerrain t; 
	[SerializeField]
	Terrain terrain; 
	[SerializeField]
	int width; 
	[SerializeField] 
	int height; 
	TerrainData terrainData; 
	float[,] heightMap; 

	void Awake(){
		t = this; 
	}

	// Use this for initialization
	void Start () {
		heightMap = new float[width, height]; 
		terrainData = terrain.terrainData; 
		GenerateTerrain (); 
	}

	void GenerateTerrain(){
		terrainData.SetHeights (width, height, heightMap); 
	}

	void ModifyHeightInternal(Vector2 point, int brushThickness){
		int xStart = (int)(point.x * width);
		int yStart = (int)(point.y * height);
		int xEnd = Mathf.Min (width, brushThickness + xStart); 
		int yEnd = Mathf.Min (height, brushThickness + yStart);
		int avg = (xEnd - xStart + yEnd - yStart) / 2;
		for (int x = xStart; x < xEnd; x++) {
			for (int y = yStart; y < yEnd; y++) {
				float height =  (float)(Mathf.Abs (x - xStart + y - yStart  - avg)) / (brushThickness * 2); 
				heightMap [x, y] = height * 0.5f; 
			}
		}
		terrainData.SetHeights (width, height, heightMap); 
	}
	public static void ModifyHeight(Vector2 point, int brushThickness){
		t.ModifyHeightInternal(point, brushThickness); 
	}

}
