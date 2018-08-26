using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GenerateHills {
	public static float[,] Create(int size){
		float[,] data = new float[size, size];
		for (int x = 0; x < size; x++) {
			for (int y = 0; y < size; y++) {
				data [x, y] = Mathf.PerlinNoise ((float)x * 2 / (float)size, (float)y * 2 / (float)size) * 0.5f 	; 
			}
		}
		return data; 
	}
}
