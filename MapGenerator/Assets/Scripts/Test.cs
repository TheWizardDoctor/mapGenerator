using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    public Camera cam;
    int width = 300;	//x
    int height = 200;	//y

    // Start is called before the first frame update
    void Start()
    {
        width = Mathf.RoundToInt(width*UIData.sizeMultiplier);
        height = Mathf.RoundToInt(height*UIData.sizeMultiplier);
        if(width <= 120 || height == 80)
        {
            width = 120;
            height = 80;
        }

        //used for perspective Camera
        cam.transform.position = new Vector3(width / 2, height, height / 2);

        var watch = System.Diagnostics.Stopwatch.StartNew();
        Map.createMap(width, height);
        watch.Stop();
        Debug.Log("Time to create all tiles is:" + watch.ElapsedMilliseconds + "ms");

        //Simple Border creation
        watch = System.Diagnostics.Stopwatch.StartNew();
        if(UIData.borderMultiplier>0)
        {
            Border.generateBorders(Map.tiles, Mathf.RoundToInt(0.0005f*Map.width*Map.height*0.5f+UIData.borderMultiplier));
            Border.SetTileCountries();
        }
        watch.Stop();
        Debug.Log("Time to create all tiles is:" + watch.ElapsedMilliseconds + "ms");

        //very simplistic city creation
        //(currently only checks 8 nearby tiles to get tile's creation value)

		watch = System.Diagnostics.Stopwatch.StartNew();
        if(UIData.cityMultiplier>0)
        {
            City.GenerateCapitals();
            City.GenerateCities(Mathf.RoundToInt(5 * Map.scanRadius * UIData.cityMultiplier));
        }
        watch.Stop();
		Debug.Log("Time to create cities is:" + watch.ElapsedMilliseconds + "ms");


        watch = System.Diagnostics.Stopwatch.StartNew();
        if(UIData.roadMultiplier>0)
        {
            int numRoads = Mathf.RoundToInt(UIData.roadMultiplier * 2*City.cityList.Count);
            for (int i = 0; i < numRoads; i++)
            {
                City c = City.cityList[RandomNum.r.Next(0, City.cityList.Count)];

                if(c.food < c.water)
                {
                    if(c.lumber<c.food)
                    {
                        //Road for lumber
                        City.TradeRouteLumber(c);
                    }
                    else
                    {
                        //Road for food
                        City.TradeRouteFood(c);
                    }
                }
                else
                {
                    if(c.lumber<c.water)
                    {
                        //Road for lumber
                        City.TradeRouteLumber(c);
                    }
                    else
                    {
                        //Road for water
                        City.TradeRouteWater(c);
                    }
                }
            }
        }
        
        watch.Stop();
        Debug.Log("Time to create roads is:" + watch.ElapsedMilliseconds + "ms");

        //HUGE FPS savers
        //Map.OceanTiles.GetComponent<MeshCombiner>().CombineMeshes();
        Map.BorderTiles.GetComponent<MeshCombiner>().CombineMeshes();
    }
}
