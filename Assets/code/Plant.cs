using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Plant : MonoBehaviour
{

    [SerializeField]
    List<GameObject> plantPrefabs;
    RainColor colorBase; 
    int index = 0;
    float growthBreakpoints;

    [SerializeField]
    float waterNeededToGrow = 3;
    bool isGrown = false;
    public bool IsBeingWatered { get { return rains.Count > 0; } }
    float wateredAmount = 0;

    List<RainColor> rains = new List<RainColor>();
    Dictionary<RainColor, float> rainAmount = new Dictionary<RainColor, float>();

    void Start()
    {
        growthBreakpoints = waterNeededToGrow / plantPrefabs.Count;
    }

    void Update()
    {
        Watering();
    }

    void UnRegister()
    {
        PlantManager.UnrregisterPlant(this);
    }
    public void SetBaseColor(RainColor color){
        colorBase = color; 
    }
    public void AddRain(RainColor rain)
    {
        Debug.Log("Getting rained on by: " + rain +  " rain"); 
        rains.Add(rain);
    }
    public void RemoveRain(RainColor rain)
    {
        Debug.Log("Removing rain"); 
        rains.Remove(rain);
    }

    void Watering()
    {
        if (IsBeingWatered)
        {
            if (!isGrown)
            {
                wateredAmount += Time.deltaTime;
                rains.ForEach((rain) =>
                {
                    if (!rainAmount.ContainsKey(rain))
                    {
                        rainAmount[rain] = Time.deltaTime;
                    }
                    else
                    {
                        rainAmount[rain] += Time.deltaTime;
                    }
                });
            }
            if(wateredAmount > waterNeededToGrow){
                MakeFruit(); 
            }
        }
    }
    void MakeFruit(){
        RainColor color; 
        color = rainAmount.Aggregate((biggest, current) => biggest.Value > current.Value ? biggest : current).Key; 
        RainColor fruitColor = ColorManager.MixColors(color, colorBase);
        isGrown = true; 
        Debug.Log("Fruit color: " + fruitColor);  
    }
}
