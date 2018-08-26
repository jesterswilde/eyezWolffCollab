using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GenerateMountain {
	public static float[,] Create(int size){
		List<float[,]> terrainMaps = new List<float[,]> (); 
		List<float> magnitudes = new List<float> (); 
		terrainMaps.Add (DiamondSquare (size, 1));
		terrainMaps.Add (DiamondSquare (size, 2));
		terrainMaps.Add (DiamondSquare (size, 3));
		magnitudes.AddRange(new float[]{0.2f, 0.5f, 1f}); 
		return MergeArrays (terrainMaps, magnitudes); 
	}
	static float[,] MergeArrays(List<float[,]> arrays, List<float> magnitudes){
		int xMax = arrays [0].GetUpperBound (0); 
		int yMax = arrays [0].GetUpperBound (1); 
		float[,] data = new float[xMax, yMax]; 
		for (int i = 0; i < arrays.Count; i++) {
			for (int x = 0; x < xMax; x++) {
				for (int y = 0; y < yMax; y++) {
					data [x, y] += arrays [i] [x, y] * magnitudes[i]; 
				}
			}
		}
		return data; 
	}
	static float[,] DiamondSquare(int size, int seed=1){
		float[,] values = new float[size, size]; 

		int sizeIndex = size - 1; 
		Random.InitState(seed);
		values [0, 0] = Random.Range (0f, 1f); 
		values [sizeIndex, 0] = Random.Range (0f, 1f); 
		values [sizeIndex, sizeIndex] = Random.Range (0f, 1f); 
		values [0, sizeIndex] = Random.Range (0f, 1f);

		DiamondSquareIteration (values, 0, 0, sizeIndex, 2); 
		return values; 
	}
	static void DiamondSquareIteration(float[,] values, int xOffset, int yOffset, int size, int step){
		Diamond (values, xOffset, yOffset, size, step); 
		Box (values, xOffset, yOffset, size, step); 
		if (size > 2) {
			size = size / 2; 
			step*= 2;
			DiamondSquareIteration (values, xOffset, yOffset, size, step); 
			DiamondSquareIteration (values, xOffset + size, yOffset, size, step); 
			DiamondSquareIteration (values, xOffset + size, yOffset + size, size, step); 
			DiamondSquareIteration (values, xOffset, yOffset + size, size, step); 
		}
	}
	static void Diamond(float[,] values, int xOffset, int yOffset, int size, int step){
		int midpoint = (size / 2);

		float average = (values [xOffset, yOffset] +
			values [xOffset + size, yOffset] +
			values [xOffset + size, yOffset + size] +
			values [xOffset, yOffset + size]) / 4;
		values[xOffset + midpoint, yOffset + midpoint] = average + (Random.Range (-0.5f, 0.5f) / step);
	}
	static void Box(float[,] values, int xOffset, int yOffset, int size, int step){
		int xMidpoint = (size / 2) + xOffset;
		int yMidpoint = (size / 2) + yOffset;
		float upperLeft = values [xOffset, yOffset + size]; 
		float upperRight = values [xOffset + size, yOffset + size]; 
		float lowerLeft = values [xOffset, yOffset]; 
		float lowerRight = values [xOffset + size, yOffset]; 

		values [xMidpoint, yOffset] = (lowerLeft + lowerRight) / 2 + (Random.Range (-0.5f, 0.5f) / step);
		values [xMidpoint, yOffset + size] = (upperLeft + upperRight) / 2 + (Random.Range (-0.5f, 0.5f) / step); 
		values [xOffset, yMidpoint] = (upperLeft + lowerLeft) / 2 + (Random.Range (-0.5f, 0.5f) / step);
		values [xOffset + size, yMidpoint] = (upperRight + lowerRight) / 2 + (Random.Range (-0.5f, 0.5f) / step); 
	}
}
