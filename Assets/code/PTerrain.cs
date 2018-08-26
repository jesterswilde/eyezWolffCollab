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
	TerrainWeight[,] weightMap; 
	float[,] heightMap; 

	void Awake(){
		t = this; 
	}

	// Use this for initialization
	void Start () {
		weightMap = new TerrainWeight[width, height]; 
		heightMap = new float[width,height];
		terrainData = terrain.terrainData; 
		GenerateTerrain (); 
	}

	void GenerateTerrain(){
		for (int x = 0; x < width; x++) {
			for (int y = 0; y < height; y++) {
				weightMap [x, y] = new TerrainWeight (x, y); 
			}
		}
		terrainData.SetHeights (0, 0, heightMap); 
	}

	void ModifyHeightInternal(Vector2 point, float weight, int brushThickness, TerrainEnum terrainEnum){
		int xStart = (int)(point.x * width);
		int yStart = (int)(point.y * height);
		int xEnd = Mathf.Min (width, brushThickness + xStart); 
		int yEnd = Mathf.Min (height, brushThickness + yStart);
		int avg = (xEnd - xStart + yEnd - yStart) / 2;
		for (int x = xStart; x < xEnd; x++) {
			for (int y = yStart; y < yEnd; y++) {
				weightMap [x, y].AddBrushStroke (terrainEnum, Time.deltaTime * 3); 
				heightMap [x, y] = weightMap [x, y].Height; 
			}
		}
		terrainData.SetHeights (0, 0, heightMap); 
	}
	public static void ModifyHeight(Vector2 point, float weight, int brushThickness, TerrainEnum terrainEnum){
		t.ModifyHeightInternal(point,weight, brushThickness,terrainEnum); 
	}

}
