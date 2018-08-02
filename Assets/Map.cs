using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour {

	Texture2D texture; 
	[SerializeField] 
	int brushThickness = 1; 
	[SerializeField]
	Color brushColor = Color.red; 
	int hardCodedTextureSize = 250; 


	void Awake(){
		texture = new Texture2D (hardCodedTextureSize, hardCodedTextureSize); 

		GetComponent<Renderer> ().material.mainTexture = texture; 
	}

	public void clickedOnPoint(Vector2 point){
		texture.SetPixels ((int)(point.x * 250), (int)(point.y * 250), brushThickness, brushThickness,  MakeColorArray());
		PTerrain.ModifyHeight (point, brushThickness); 
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
