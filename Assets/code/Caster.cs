using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Caster : MonoBehaviour
{

    Vector3 lastPos;
    [SerializeField]
    List<TerrainEnum> Terrains = new List<TerrainEnum>();
    [SerializeField]
    int paintSize = 10;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            PaintTerrain(0);
        }
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            PaintTerrain(1);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            PaintTerrain(2);
        }
        if(Input.GetKeyDown(KeyCode.Q)){
            PaintCloud(RainColor.RED);
        }
        if(Input.GetKeyDown(KeyCode.W)){
            PaintCloud(RainColor.ORANGE); 
        }
        if (Input.GetMouseButtonDown(0))
        {
            PlantSeed();
        }

    }

    void PaintCloud(RainColor color){
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        float[,] paint = new float[paintSize, paintSize];
        for (int x = 0; x < paintSize; x++)
        {
            for (int y = 0; y < paintSize; y++)
            {
                paint[x, y] = 1;
            }
        }
        if (Physics.Raycast(ray, out hit))
        {
            int[] xy = TerrainManager.HitPointToXY(hit.point); 
            CloudManager.MakeCloud(color, paint, xy[0] - paintSize/2, xy[1] - paintSize / 2);
        }
    }

    void PaintTerrain(int terrainNumber)
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        float[,] paint = new float[paintSize, paintSize];
        for (int x = 0; x < paintSize; x++)
        {
            for (int y = 0; y < paintSize; y++)
            {
                paint[x, y] = 1;
            }
        }
        if (Physics.Raycast(ray, out hit))
        {
            int xOffset = (int)(hit.textureCoord.x * TerrainManager.Width) - paintSize / 2;
            int yOffset = (int)(hit.textureCoord.y * TerrainManager.Height) - paintSize / 2;
            TerrainManager.PaintTerrain(paint, yOffset, xOffset, (TerrainEnum)terrainNumber);
        }
    }

    void PlantSeed()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            PlantManager.PlantSeedAtPoint(hit);
        }
    }
}
