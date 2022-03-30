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
    public int wealth;
    public float water;
    public float lumber;
    public float food;

    private City(int xVal, int yVal)
    {
        x = xVal;
        y = yVal;
        wealth = 10000;
        water = (float)Random.r.NextDouble();
        lumber = (float)Random.r.NextDouble();
        food = (float)Random.r.NextDouble();
        cityList.Add(this);
    }

    //create num number of cities
    public static void GenerateCities(int num)
    {
        scanRadius = (int)Math.Floor(0.05 * Map.width);
       // Debug.Log("scanRadius:" + scanRadius);

        while (num > 0)
        {
            GenerateCity();
            num--;
        }
    }

    //create a single city
    private static void GenerateCity()
    {
        Tile[,] tiles = Map.tiles;
        int bestVal = int.MinValue;
        ref Tile bestTile = ref tiles[0, 0];

        foreach (Tile tile in tiles)
        {
            if(tile.City!=null || tile.Biome==Biome.Ocean)
            {
                continue;
            }

            int currentVal = CalculateValue(tile);
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

        
        GameObject city = GameObject.CreatePrimitive(PrimitiveType.Capsule);
        city.gameObject.GetComponent<MeshRenderer>().material = cityMat;
        city.transform.position = new Vector3(bestTile.X, 6, bestTile.Y);
        City newCity = new City(bestTile.X, bestTile.Y);
        bestTile.City = newCity;
        //Debug.Log("best val:" + bestVal);
    }

    public static void TradeRouteFood(City start)
    {
        float bestGuessVal = 0;
        City bestGuessCity = null;
        foreach(City c in cityList)
        {
            if (c.food > bestGuessVal)
            {
                Debug.Log("food:" + c.food);
                bestGuessVal = c.food;
                bestGuessCity = c;
            }
        }
        Debug.Log("Best:" + bestGuessVal);

        Road.CreateRoad(start, bestGuessCity);
    }
    public static void TradeRouteWater(City start)
    {
        float bestGuessVal = 0;
        City bestGuessCity = null;
        foreach (City c in cityList)
        {
            if (c.water > bestGuessVal && c != start)
            {
                bestGuessVal = c.water;
                bestGuessCity = c;
            }
        }

        Road.CreateRoad(start, bestGuessCity);
    }
    public static void TradeRouteLumber(City start)
    {
        float bestGuessVal = 0;
        City bestGuessCity = null;
        foreach (City c in cityList)
        {
            if (c.lumber > bestGuessVal && c != start)
            {
                bestGuessVal = c.lumber;
                bestGuessCity = c;
            }
        }

        Road.CreateRoad(start, bestGuessCity);
    }

    //get value of current tile based on neighboring tiles
    public static int CalculateValue(Tile tile)
    {
        Tile[,] tiles = Map.tiles;
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
                value += BiomeValue(temp);
                value += HasCity(temp);
            }
        }
        return value;
    }

    public static int BiomeValue(Tile tile)
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

    public static int HasCity(Tile tile)
    {
        if(tile.City!=null)
        {
            return -50 * scanRadius;
        }
        else
        {
            return 0;
        }
    }
}
