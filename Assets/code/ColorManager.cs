using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class ColorTerrainPlant
{

    [SerializeField]
    Plant plantPrefab;
    public Plant PlantPrefab { get { return plantPrefab; } }
    [SerializeField]
    TerrainEnum terrain;
    public TerrainEnum Terrain { get { return terrain; } }
    [SerializeField]
    RainColor rainColor;
    public RainColor RainColor { get { return rainColor; } }
    [SerializeField]
    Color color;
    public Color Color { get { return color; } }
    [SerializeField]
    GameObject fruitPrefab; 
    public GameObject FruitPrefab {get{return fruitPrefab;}}
}

[Serializable]
public class ColorMix
{
    [SerializeField]
    RainColor colorA;
    public RainColor ColorA { get { return colorA; } }
    [SerializeField]
    RainColor colorB;
    public RainColor ColorB { get { return colorB; } }
    [SerializeField]
    RainColor resultColor;
    public RainColor ResultColor { get { return resultColor; } }
}


public class ColorManager : MonoBehaviour
{
    static ColorManager t;
    [SerializeField]
    ColorTerrainPlant[] ColorTerrainPlants;

    [SerializeField]
    List<ColorMix> ColorsToMix = new List<ColorMix>();
    Dictionary<int, RainColor> MixedColors = new Dictionary<int, RainColor>();
    Dictionary<RainColor, ColorTerrainPlant> RainDict = new Dictionary<RainColor, ColorTerrainPlant>();
    Dictionary<TerrainEnum, ColorTerrainPlant> TerrainDict = new Dictionary<TerrainEnum, ColorTerrainPlant>();

    public static ColorTerrainPlant GetValuesForRain(RainColor rain)
    {
        return t.RainDict.GetValueOrDefault(rain);
    }
    public static ColorTerrainPlant GetValuesForTerrain(TerrainEnum terrain)
    {
        return t.TerrainDict.GetValueOrDefault(terrain);
    }
    void SetupMixedColors()
    {
        ColorsToMix.ForEach((mix) =>
        {
            int a = 1 << (int)(mix.ColorA); //RED 1 << 1 = 1;
            int b = 1 << (int)(mix.ColorB); //YELLOW 1 << 3 = 8; 
            MixedColors[a + b] = mix.ResultColor;  //BLUE - [9] 
        });
    }
    public static RainColor MixColors(RainColor a, RainColor b)
    {
        Debug.Log("Mixing : " + a + " & " + b); 
        if(a == b){
            return a; 
        }
        return t.MixedColors[(1 << (int)a) + (1 << (int)b)];
    }
    void SetupDicts()
    {
        ColorTerrainPlants.ForEach((item) =>
        {
            RainDict[item.RainColor] = item;
            TerrainDict[item.Terrain] = item;
        });
    }

    void Awake()
    {
        t = this;
        SetupDicts();
        SetupMixedColors();
    }
}
