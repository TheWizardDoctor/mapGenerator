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
        //used for orthographic Camera
        //cam.orthographicSize = 50;

        //used for perspective Camera
        cam.transform.position = new Vector3(width / 2, height, height / 2);

        var watch = System.Diagnostics.Stopwatch.StartNew();

        Map.createMap(width, height);
        watch.Stop();
        Debug.Log("Time to create all tiles is:" + watch.ElapsedMilliseconds + "ms");

        //Simple Border creation
        watch = System.Diagnostics.Stopwatch.StartNew();
        Border.generateBorders(Map.tiles, 10);
        Border.SetTileCountries();
        watch.Stop();
        Debug.Log("Time to create all tiles is:" + watch.ElapsedMilliseconds + "ms");

        //very simplistic city creation
        //(currently only checks 8 nearby tiles to get tile's creation value)

		watch = System.Diagnostics.Stopwatch.StartNew();
        City.GenerateCapitals();
        City.GenerateCities(Mathf.RoundToInt(5 * Map.scanRadius * UIData.cityMultiplier));
        watch.Stop();
		Debug.Log("Time to create cities is:" + watch.ElapsedMilliseconds + "ms");


        watch = System.Diagnostics.Stopwatch.StartNew();
        for (int i = 0; i < City.cityList.Count; i++)
        {
            double rand = RandomNum.r.NextDouble();

            if (rand < 0.333)
            {
                City.TradeRouteFood(City.cityList[i]);
            }
            else if (rand < 0.666)
            {
                City.TradeRouteLumber(City.cityList[i]);
            }
            else
            {
                City.TradeRouteWater(City.cityList[i]);
            }
        }
        watch.Stop();
        Debug.Log("Time to create roads is:" + watch.ElapsedMilliseconds + "ms");

        //HUGE FPS savers
        //Map.OceanTiles.GetComponent<MeshCombiner>().CombineMeshes();
        Map.BorderTiles.GetComponent<MeshCombiner>().CombineMeshes();
    }
}
