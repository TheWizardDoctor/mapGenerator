using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class City
{
    //static fields
    static int scanRadius;
    private static Material cityMat = (Material)Resources.Load("Materials/City.mat");
    public static List<City> cityList = new List<City>();

    //instance fields
    public int x, y;

    private City(int xVal, int yVal)
    {
        this.x = xVal;
        this.y = yVal;
        cityList.Add(this);
    }

    //create num number of cities
    public static void generateCities(Tile[,] t, int num)
    {
        scanRadius = (int)Math.Floor(0.05 * Map.width);
        Debug.Log("scanRadius:" + scanRadius);

        while(num>0)
        {
            generateCity(t);
            num--;
        }
    }

    //create a single city
    private static void generateCity(Tile[,] tiles)
    {
        int bestVal = int.MinValue;
        ref Tile bestTile = ref tiles[0, 0];

        foreach (Tile tile in tiles)
        {
            if(tile.City==true || tile.Biome==Biome.Ocean)
            {
                continue;
            }

            int currentVal = calculateValue(tiles, tile);
            if (currentVal > bestVal)
            {
                bestVal = currentVal;
                bestTile = tile;
            }
            else if (currentVal==bestVal)
            {
                if(Random.r.NextDouble()>0.5)
                {
                    bestTile = tile;
                }
            }
        }

        //Debug.Log("Best City Location (X:" + bestTile.X + " Y:" + bestTile.Y + ")");

        bestTile.City = true;
        GameObject city = GameObject.CreatePrimitive(PrimitiveType.Capsule);
        city.gameObject.GetComponent<MeshRenderer>().material = cityMat;
        city.transform.position = new Vector3(bestTile.X, 6, bestTile.Y);
        City newCity = new City(bestTile.X, bestTile.Y);
        Debug.Log("best val:" + bestVal);
    }

    //get value of current tile based on neighboring tiles
    public static int calculateValue(Tile[,] tiles, Tile tile)
    {
        int value = 0;

        for (int i = -scanRadius; i <= scanRadius; i++)
        {
            for (int j = -scanRadius; j <= scanRadius; j++)
            {
                if (tile.X + i < 0 || tile.Y + j < 0 || tile.X + i > Map.width - 1 || tile.Y + j > Map.height - 1)
                {
                    continue;
                }

                Tile temp = tiles[tile.X + i, tile.Y + j];
                value += biomeValue(temp);
                value += hasCity(temp);
            }
        }
        return value;
    }

    public static int biomeValue(Tile tile)
    {
        switch (tile.Biome)
        {
            case Biome.BorealForest:
                return 20;
            case Biome.Desert:
                return -20;
            case Biome.Mountain:
                return -10;
            case Biome.Ocean:
                return 25;
            case Biome.Prairie:
                return 20;
            case Biome.Rainforest:
                return 25;
            case Biome.Savannah:
                return 20;
            case Biome.Shrubland:
                return 10;
            case Biome.TemperateForest:
                return 25;
            case Biome.Tundra:
                return 5;
            default:
                return 0;
        }
    }

    public static int hasCity(Tile tile)
    {
        if(tile.City==true)
        {
            return -50*scanRadius;
        }
        else
        {
            return 0;
        }
    }
}
