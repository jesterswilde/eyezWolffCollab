using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq; 

[Serializable]
public class TerrainDataEnum{
	public TerrainEnum terrainEnum; 
	public TerrainData terrainData;
}

public class TerrainWeight {
	int x;
	int y;
	float height = 0; 
	public float Height{ get { return height; } }
	Dictionary<TerrainEnum, float> weights;
	public TerrainWeight(int xPos, int yPos){
		x = xPos; 
		y = yPos;
		weights = new Dictionary<TerrainEnum, float>();
		weights [TerrainEnum.NULL] = 1; 
		CalcHeight();
	}
	public void AddBrushStroke(TerrainEnum terrain, float weight){
		weights.Keys.ToList().ForEach ((key) => {
			weights[key] = weights[key] * (1f - weight);
		});
		if(weights.ContainsKey(terrain)){
			weights[terrain] += weight; 
		}
		else{
			weights [terrain] = weight; 
		}
		CalcHeight (); 
	}
	public TerrainEnum GetTerrainType(){
		return weights.Aggregate ((biggest, current) => {
			return biggest.Value > current.Value ? biggest : current;
		}).Key;
	}
	void CalcHeight(){
		height = 0; 
		foreach (var kvp in weights) {
			height += TerrainManager.GetHeightAt (kvp.Key, x, y) * kvp.Value;
		} 
	}
	
}
