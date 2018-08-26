using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour {

	Texture2D texture; 
	[SerializeField] 
	int brushThickness = 1; 
	[SerializeField]
	Color primaryBrushColor = Color.red; 
	[SerializeField]
	TerrainEnum primaryBrushType;
	[SerializeField]
	Color secondaryBrushColor = Color.green; 
	[SerializeField]
	TerrainEnum secondaryBrushType = TerrainEnum.HILLS;

	TerrainEnum brushType; 
	Color brushColor; 
	int hardCodedTextureSize = 64; 


	void Awake(){
		texture = new Texture2D (hardCodedTextureSize, hardCodedTextureSize); 

		GetComponent<Renderer> ().material.mainTexture = texture; 
	}

	void Update(){
		if (Input.GetMouseButtonDown (0)) {
			brushColor = primaryBrushColor; 
			brushType = primaryBrushType;
		}
		if (Input.GetMouseButtonDown (1)) {
			brushColor = secondaryBrushColor; 
			brushType = secondaryBrushType;
		}
	}

	public void clickedOnPoint(Vector2 point){
		float hardCodedWeight = 0.3f; 
		texture.SetPixels ((int)(point.x * hardCodedTextureSize), (int)(point.y * hardCodedTextureSize), brushThickness, brushThickness,  MakeColorArray());
		PTerrain.ModifyHeight (point, hardCodedWeight, brushThickness, brushType); 
		texture.Apply (); 
	}

	Color[] MakeColorArray(){
		Color[] colArray = new Color[brushThickness * brushThickness];
		for (int i = 0; i < colArray.Length; i++) {
			colArray [i] = brushColor; 
		}
		return colArray; 
	}

}
