using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainManager : MonoBehaviour {

	static TerrainManager t;
	Dictionary<TerrainEnum, float[,]> terrains = new Dictionary<TerrainEnum, float[,]> (); 

	Object prefab; 
	[SerializeField]
	Terrain _terrain; 
	[SerializeField]
	int resolution = 64; 
	[SerializeField]
	int width; 
	public static int Width { get{ return t.width; } }
	[SerializeField]
	int height; 
	public static int Height { get { return t.height; } }
	List<float[,]> terrainMaps = new List<float[,]>(); 
	List<float> magnitudes = new List<float> (); 
	TerrainWeight[,] weightMap; 
	public static TerrainWeight[,] WeightMap { get{ return t.weightMap; } }
	float[,] heightMap;
	public static float[,] HeightMap { get { return t.heightMap; } }
	int tDataWidth; 
	int tDataHeight; 


	void Awake(){
		t = this; 
		tDataHeight = (int)_terrain.terrainData.size.z;
		tDataWidth = (int)_terrain.terrainData.size.x;
		GenerateTerrains (); 
		GenerateWeightMap (); 
		UpdateTerrainHeight(); 
	}
	public static void PaintTerrain(float[,] area, int xOffset, int yOffset, TerrainEnum terrain){
		int xMax = Mathf.Min(area.GetUpperBound(0) + xOffset, Width);
		int yMax = Mathf.Min(area.GetUpperBound(1) + yOffset, Height);
		int xMin = Mathf.Max(0, xOffset); 
		int yMin = Mathf.Max(0, yOffset);  
		for(int x = xMin; x < xMax; x++){
			for(int y = yMin; y < yMax; y++){
				t.weightMap[x,y].AddBrushStroke(terrain, area[x-xOffset, y-yOffset]);
				t.heightMap[x,y] = t.weightMap[x,y].Height; 
			}
		}
		UpdateTerrainHeight(); 
	}
	static void UpdateTerrainHeight(){
		t._terrain.terrainData.SetHeights(0,0,HeightMap); 
	}
	void GenerateWeightMap(){
		weightMap = new TerrainWeight[width, height];
		heightMap = new float[width, height]; 
		for (int x = 0; x < width; x++) {
			for (int y = 0; y < height; y++) {
				weightMap [x, y] = new TerrainWeight (x, y); 
			}
		}
	}
	  
	void GenerateTerrains(){
		terrains [TerrainEnum.MOUNTAIN] = GenerateMountain.Create (resolution + 1); 
		terrains [TerrainEnum.HILLS] = GenerateHills.Create (resolution);  
		terrains [TerrainEnum.NULL] = new float[resolution, resolution]; 
	}

	public static float GetHeightAt(TerrainEnum terrainEnum, int x, int y){
		return t.terrains [terrainEnum] [x, y];
	}
	public static float GetHeightAt(int x, int y){
		return WeightMap[x,y].Height * t._terrain.terrainData.heightmapHeight;
	}
	public static float GetHeightAt(Vector2 texCoord){
		int[] xy = TextCoordToXY(texCoord);
		return GetHeightAt(xy[0], xy[1]);
	}
	public static Vector3 GetPositionAt(int x, int y){
		float height = GetHeightAt(x, y);
		return new Vector3((float)x, height, (float)y);
	}
	public static TerrainEnum GetTerrainAt(int x, int y){
		return t.weightMap[x,y].GetTerrainType();
	}
	public static TerrainEnum GetTerrainAt(Vector2 texCoord){
		int[] xy = TextCoordToXY(texCoord); 
		return GetTerrainAt(xy[0], xy[1]); 
	}
	public static int[] TextCoordToXY(Vector2 texCoord){
		return new int[]{(int)(texCoord.x * Width), (int)(texCoord.y * Height)};
	}
	public static int[] HitPointToXY(Vector3 hit){
		return new int[]{
			(int)((hit.x / t.tDataWidth) * Width),
			(int)((hit.z / t.tDataHeight) * Height)
		};
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
	}
	public static void ModifyHeight(Vector2 point, float weight, int brushThickness, TerrainEnum terrainEnum){
		t.ModifyHeightInternal(point,weight, brushThickness,terrainEnum); 
	}



}

