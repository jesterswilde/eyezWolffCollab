using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileManager : MonoBehaviour {

	public static ProjectileManager t;
	[SerializeField] 
	Texture2D splashTex;
	float[,] splashAlphaInfo;
	public static float [,] SplashAlphaInfo { get { return t.splashAlphaInfo; } }

	void Start () {
		t = this;
		int width = splashTex.width;
		int height = splashTex.height;
		splashAlphaInfo = new float[width, height];
		for (int i = 0; i < width; i++) 
		{
			for (int j = 0; j < height; j++) 
			{
				splashAlphaInfo [i, j] = splashTex.GetPixel (i, j).a;
			}
		}
	}
}

//public static class GameManager{
//	public static int BaseSpeed = 5; 
//	static int score = 0; 
//	public static int Score {get{return score;}} 
//	public static void PlayerHit(){
//		score += 5; 
//	}
//	static List<GameObject> Players = new List<GameObject>(); 
//	public static void RegsiterGameObject(GameObject go){
//		Players.Add (go); 
//	}
