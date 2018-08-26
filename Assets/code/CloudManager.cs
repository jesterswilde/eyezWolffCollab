using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudManager : MonoBehaviour {

	static CloudManager t;

	[SerializeField]
	Cloud cloudPrefab; 
	[SerializeField]
	float cloudDuration = 120; 
	public static float CloudDuration {get{return t.cloudDuration;}}
	[SerializeField]
	float cloudDurationVariation = 30; 
	public static float CloudDurationVariation {get{return t.cloudDurationVariation;}}
	List<Cloud> clouds = new List<Cloud>(); 


	public static void MakeCloud(RainColor rainColor, float[,] area, int xOffset, int yOffset){
		Cloud cloud = Instantiate(t.cloudPrefab); 
		cloud.CloudStartup(rainColor, area, xOffset, yOffset, CloudDuration + Random.Range(0, CloudDurationVariation)); 
		t.clouds.Add(cloud); 
	}

	public static void RemoveCloud(RainColor rainColor, float[,] area, int xOffset, int  yOffset){
		
	}

	public static void BeAwareofPlant(Plant plant, int x, int y){
		t.clouds.ForEach((cloud)=> cloud.BeAwareOfPlant(plant, x, y));
	}

	void Awake(){
		t = this; 
	}
}
