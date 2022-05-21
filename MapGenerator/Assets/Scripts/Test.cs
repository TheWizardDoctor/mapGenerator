using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    public Camera cam;
    int width = 402;	//x
    int height = 268;   //y

    // Start is called before the first frame update
    private void Start()
    {
        width = Mathf.RoundToInt(width * UIData.sizeMultiplier);
        height = Mathf.RoundToInt(height * UIData.sizeMultiplier);
        if (width <= 120 || height == 80)
        {
            width = 120;
            height = 80;
        }

        //used for perspective Camera
        cam.transform.position = new Vector3(width / 2, height, height / 2);

        Map.S.CreateMap(width, height);


        //Simple Border creation
        if (UIData.borderMultiplier > 0)
        {
            Border.S.generateBorders(Map.S.tiles, Mathf.RoundToInt((Map.S.width * 0.01f) + UIData.borderMultiplier));
            Border.S.SetTileCountries();
        }

        //very simplistic city creation
        //(currently only checks 8 nearby tiles to get tile's creation value)


        if (UIData.cityMultiplier > 0)
        {
            City.GenerateCapitals();
            City.GenerateCities(Mathf.RoundToInt(0.5f * Map.S.width * UIData.cityMultiplier));
        }

        if (UIData.roadMultiplier > 0)
        {
            int numRoads = Mathf.RoundToInt(UIData.roadMultiplier * 2 * City.cityList.Count);
            for (int i = 0; i < numRoads; i++)
            {
                City c = City.cityList[RandomNum.r.Next(0, City.cityList.Count)];

                if (c.Food < c.Water)
                {
                    if (c.Lumber < c.Food)
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
                    if (c.Lumber < c.Water)
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

        //HUGE FPS savers
        Map.S.BorderTiles.GetComponent<MeshCombiner>().CombineMeshes();
        Map.S.BorealForestTiles.GetComponent<MeshCombiner>().CombineMeshes();
        Map.S.DesertTiles.GetComponent<MeshCombiner>().CombineMeshes();
        Map.S.MountainTiles.GetComponent<MeshCombiner>().CombineMeshes();
        Map.S.OceanTiles.GetComponent<MeshCombiner>().CombineMeshes();
        Map.S.PrairieTiles.GetComponent<MeshCombiner>().CombineMeshes();
        Map.S.RainforestTiles.GetComponent<MeshCombiner>().CombineMeshes();
        Map.S.SavannaTiles.GetComponent<MeshCombiner>().CombineMeshes();
        Map.S.ShrublandTiles.GetComponent<MeshCombiner>().CombineMeshes();
        Map.S.TemperateForestTiles.GetComponent<MeshCombiner>().CombineMeshes();
        Map.S.TundraTiles.GetComponent<MeshCombiner>().CombineMeshes();
    }
}
