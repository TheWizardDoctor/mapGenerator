using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class City
{
    //static fields
    public static List<City> cityList = new List<City>();
    public static List<String> cityNames;

    //instance fields
    public string name;
    public int population;
    public int x, y;
    public int wealth;
    public float water;
    public float lumber;
    public float food;
    public int roads;
    public bool capital;
    public GameObject house;

    private City(int xVal, int yVal)
    {
        int num = RandomNum.r.Next(0, cityNames.Count);
        name = cityNames[num];
        cityNames.RemoveAt(num);
        population = Mathf.RoundToInt(UIData.populationMultiplier * RandomNum.r.Next(100, 10000));
        x = xVal;
        y = yVal;
        capital = false;
        cityList.Add(this);
        SetResources(this);
    }

    //create num number of cities
    public static void GenerateCities(int num)
    {
        while (num > 0)
        {
            GenerateCity();
            num--;
        }
    }

    public static void GenerateCapitals()
    {
        foreach(Country c in Country.countryList)
        {
            float bestVal = int.MinValue;
            if(c.tilesInCountry.Count==0)
            {
                continue;
            }

            Tile bestTile = c.tilesInCountry[0];

            foreach (Tile tile in c.tilesInCountry)
            {
                if (tile.City != null || tile.Biome == Biome.Ocean)
                {
                    continue;
                }

                float currentVal = tile.tileValue;
                if (currentVal > bestVal)
                {
                    bestVal = currentVal;
                    bestTile = tile;
                }
                else if (currentVal == bestVal)
                {
                    if (RandomNum.r.NextDouble() > 0.5)
                    {
                        bestTile = tile;
                    }
                }
            }

            //Debug.Log("Best City Location (X:" + bestTile.X + " Y:" + bestTile.Y + ")");
            AddCity(bestTile);
            GameObject cityGameObject = UnityEngine.Object.Instantiate(Resources.Load<GameObject>("house"));
            cityGameObject.transform.SetParent(Map.Houses.transform);
            cityGameObject.transform.position = new Vector3(bestTile.X, (bestTile.Elevation / 10) + 1, bestTile.Y);
            cityGameObject.transform.localScale = 1.3f * cityGameObject.transform.localScale;

            City newCity = new City(bestTile.X, bestTile.Y);
            newCity.wealth *= RandomNum.r.Next(2,3);
            newCity.population *= RandomNum.r.Next(2, 5);
            newCity.capital = true;
            newCity.house = cityGameObject;
            bestTile.City = newCity;
            //Debug.Log("best val:" + bestVal);
        }
    }

    //create a single city
    private static void GenerateCity()
    {
        Tile[,] tiles = Map.tiles;
        float bestVal = int.MinValue;
        ref Tile bestTile = ref tiles[0, 0];

        foreach (Tile tile in tiles)
        {
            if (tile.City != null || tile.Biome == Biome.Ocean)
            {
                continue;
            }

            float currentVal = tile.tileValue;
            if (currentVal > bestVal)
            {
                bestVal = currentVal;
                bestTile = tile;
            }
            else if (currentVal == bestVal)
            {
                if (RandomNum.r.NextDouble() > 0.5)
                {
                    bestTile = tile;
                }
            }
        }

        //Debug.Log("Best City Location (X:" + bestTile.X + " Y:" + bestTile.Y + ")");
        AddCity(bestTile);
        GameObject cityGameObject = UnityEngine.Object.Instantiate(Resources.Load<GameObject>("house"));
        cityGameObject.transform.SetParent(Map.Houses.transform);
        cityGameObject.transform.position = new Vector3(bestTile.X, (bestTile.Elevation / 10) + 1, bestTile.Y);

        City newCity = new City(bestTile.X, bestTile.Y);
        newCity.house = cityGameObject;
        bestTile.City = newCity;
        //Debug.Log("best val:" + bestVal);
    }

    public static void PlaceNewCity(Tile tile)
    {
        if(tile.City==null)
        {
            AddCity(tile);
            GameObject city = UnityEngine.Object.Instantiate(Resources.Load<GameObject>("house"));
            city.transform.SetParent(Map.Houses.transform);
            city.transform.position = new Vector3(tile.X, (tile.Elevation / 10) + 1, tile.Y);

            City newCity = new City(tile.X, tile.Y);
            tile.City = newCity;
        }
    }

    public static void TradeRouteFood(City start)
    {
        float bestGuessVal = 0;
        City bestGuessCity = null;
        foreach (City c in cityList)
        {
            if (c.food > bestGuessVal)
            {
                //Debug.Log("food:" + c.food);
                bestGuessVal = c.food;
                bestGuessCity = c;
            }
        }
        //Debug.Log("Best:" + bestGuessVal);

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

    /*public static float GetValue(Tile tile)
    {
        return tile.tileValue;
    }*/

    private static void AddCity(Tile tile)
    {
        Tile[,] tiles = Map.tiles;

        for (int i = -Map.scanRadius; i <= Map.scanRadius; i++)
        {
            for (int j = -Map.scanRadius; j <= Map.scanRadius; j++)
            {
                if (tile.X + i < 0 || tile.Y + j < 0 || tile.X + i > Map.width - 1 || tile.Y + j > Map.height - 1)
                {
                    continue;
                }

                tiles[tile.X + i, tile.Y + j].tileValue = -1000 * Map.scanRadius;
            }
        }
    }
    public static void RemoveCity(Tile tile)
    {
        if(tile.City!=null)
        {
            Tile[,] tiles = Map.tiles;

            for (int i = -Map.scanRadius; i <= Map.scanRadius; i++)
            {
                for (int j = -Map.scanRadius; j <= Map.scanRadius; j++)
                {
                    if (tile.X + i < 0 || tile.Y + j < 0 || tile.X + i > Map.width - 1 || tile.Y + j > Map.height - 1)
                    {
                        continue;
                    }

                    tiles[tile.X + i, tile.Y + j].tileValue = +1000 * Map.scanRadius;
                }
            }
            UnityEngine.GameObject.Destroy(tile.City.house);
            cityList.Remove(tile.City);
        }
    }

    private static void SetResources(City city)
    {
        Tile[,] tiles = Map.tiles;

        for (int i = -Map.scanRadius; i <= Map.scanRadius; i++)
        {
            for (int j = -Map.scanRadius; j <= Map.scanRadius; j++)
            {
                if (city.x + i < 0 || city.y + j < 0 || city.x + i > Map.width - 1 || city.y + j > Map.height - 1)
                {
                    continue;
                }

                Tile temp = tiles[city.x + i, city.y + j];

                switch (temp.Biome)
                {
                    case Biome.Ocean:
                        city.water += 50;
                        city.food += 20;
                        break;
                    case Biome.Mountain:
                        city.food += 5;
                        city.lumber += 10;
                        break;
                    case Biome.Tundra:
                        city.lumber += 10;
                        city.water += 10;
                        city.food += 5;
                        break;
                    case Biome.BorealForest:
                        city.water += 15;
                        city.food += 15;
                        city.lumber += 15;
                        break;
                    case Biome.Prairie:
                        city.food += 15;
                        city.water += 10;
                        break;
                    case Biome.Shrubland:
                        city.food += 15;
                        city.water += 10;
                        city.lumber += 5;
                        break;
                    case Biome.TemperateForest:
                        city.food += 15;
                        city.lumber += 25;
                        city.water += 10;
                        break;
                    case Biome.Desert:
                        city.food += 5;
                        break;
                    case Biome.Savanna:
                        city.water += 10;
                        city.lumber += 20;
                        city.food += 20;
                        break;
                    case Biome.Rainforest:
                        city.water += 30;
                        city.lumber += 20;
                        city.food += 15;
                        break;
                }
            }
        }
        city.wealth = Mathf.RoundToInt(RandomNum.r.Next(10, 100) * UIData.wealthMultiplier * (city.food + city.water + city.lumber));
    }

    public static int BiomeValue(Tile tile)
    {
        float randomness = (float)RandomNum.r.NextDouble();
        switch (tile.Biome)
        {
            case Biome.BorealForest:
                return Mathf.RoundToInt(1.2f * randomness);
            case Biome.Desert:
                return Mathf.RoundToInt(1 * randomness);
            case Biome.Mountain:
                return Mathf.RoundToInt(1.1f * randomness);
            case Biome.Ocean:
                return Mathf.RoundToInt(1.2f * randomness);
            case Biome.Prairie:
                return Mathf.RoundToInt(1.15f * randomness);
            case Biome.Rainforest:
                return Mathf.RoundToInt(1.2f * randomness);
            case Biome.Savanna:
                return Mathf.RoundToInt(1.2f * randomness);
            case Biome.Shrubland:
                return Mathf.RoundToInt(1.15f * randomness);
            case Biome.TemperateForest:
                return Mathf.RoundToInt(1.2f * randomness);
            case Biome.Tundra:
                return Mathf.RoundToInt(1.15f * randomness);
            default:
                return 0;
        }
    }

    public static int HasCity(Tile tile)
    {
        if (tile.City != null)
        {
            return -50 * Map.scanRadius;
        }
        else
        {
            return 0;
        }
    }

    public static void LoadCityNames()
    {
        var cityNameFile = File.ReadAllLines("CityNames.txt");
        cityNames = new List<string>(cityNameFile);

    }
}
