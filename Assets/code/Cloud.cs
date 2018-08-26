using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cloud : MonoBehaviour {

	float timeLeft;

	float[,] area; 
	int xOffset;
	int yOffset; 
	RainColor rainColor; 
	List<Plant> plants; 

	bool isRainingOn(int x, int y){
		return x > xOffset && x < xOffset + area.GetUpperBound(0)
			&& y > yOffset && y < yOffset + area.GetUpperBound(1); 
	}

	public void BeAwareOfPlant(Plant plant, int x, int y){
		if(isRainingOn(x,y)){
			plants.Add(plant); 
			plant.AddRain(rainColor); 
		}
	}

	public void CloudStartup(RainColor _rainColor, float[,] _area, int _xOffset, int _yOffset, float duration){
		area = _area; 
		rainColor = _rainColor; 
		timeLeft = duration; 
		xOffset = _xOffset;
		yOffset =_yOffset; 
		plants = PlantManager.GetPlantsInArea(area, xOffset, yOffset);
		plants.ForEach((plant)=> plant.AddRain(rainColor)); 
	}


	void Update(){
		timeLeft -= Time.deltaTime; 
		if(timeLeft <= 0){
			CloudManager.RemoveCloud(rainColor, area, xOffset, yOffset); 
		}
	}
}
