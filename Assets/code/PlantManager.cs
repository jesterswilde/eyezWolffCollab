using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class PlantManager : MonoBehaviour
{
    static PlantManager t;
    Dictionary<string, Plant> plantLocations = new Dictionary<string, Plant>();

    public static void PlantSeedAtPoint(RaycastHit hit){
        TerrainEnum terrain = TerrainManager.GetTerrainAt(new Vector2(hit.textureCoord2.y, hit.textureCoord2.x)); 
        ColorTerrainPlant ctp = ColorManager.GetValuesForTerrain(terrain);
        Plant plant = Instantiate(ctp.PlantPrefab, hit.point, Quaternion.identity); 
        plant.SetBaseColor(ctp.RainColor); 
        RegisterPlant(plant); 
        
    }
    public static void RegisterPlant(Plant plant)
    {
        string location = GetLocation(plant.transform);
        if (t.plantLocations.ContainsKey(location))
        {
            Destroy(plant); 
        }
        Debug.Log("Plant Location: " + location); 
        t.plantLocations[location] = plant;
    }
    static string GetLocation(Transform trans)
    {
        int[] xy = TerrainManager.HitPointToXY(trans.position); 
        return xy[0] + "-" + xy[1];
    }
    static string GetLocation(int x, int y)
    {
        return x.ToString() + "-" + y.ToString();
    }
    public static void UnrregisterPlant(Plant plant)
    {
        string location = GetLocation(plant.transform);
        t.plantLocations[location] = null;
    }

    public static List<Plant> GetPlantsInArea(float[,] area, int xOffset, int yOffset)
    {
        Debug.Log("Looking for plants between: " + xOffset + " - " + (xOffset + area.GetUpperBound(0)) + " | " + yOffset + " - " + (yOffset + area.GetUpperBound(1))); 
        List<Plant>plants = new List<Plant>(); 
        for (int x = xOffset; x < area.GetUpperBound(0) + xOffset; x++)
        {
            for (int y = yOffset; y < area.GetUpperBound(1) + yOffset; y++)
            {
                string location = GetLocation(x, y);
                if(t.plantLocations.ContainsKey(location)){
                    plants.Add(t.plantLocations[location]); 
                }
            }
        }
        return plants; 
    }

    public static void CloudRemoved(float[,] area, int xOffset, int yOffset, RainColor color)
    {

    }


    void Awake()
    {
        t = this;
    }

}
