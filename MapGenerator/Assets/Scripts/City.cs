using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class City
{
    //static fields
    public static List<City> cityList = new List<City>();
    public static List<string> cityNames;

    //instance fields
    public string Name { get; }
    public int Population { get; private set; }
    public int X { get; }
    public int Y { get; }
    public int Wealth { get; private set; }
    public float Water { get; private set; }
    public float Lumber { get; private set; }
    public float Food { get; private set; }
    public bool Capital { get; private set; }
    public GameObject House { get; private set; }

    private City(int xVal, int yVal)
    {
        if (cityNames.Count > 0)
        {
            int num = RandomNum.r.Next(0, cityNames.Count);
            Name = cityNames[num];
            cityNames.RemoveAt(num);
        }
        X = xVal > 0 && xVal < Map.width ? xVal : 0;
        Y = yVal > 0 && yVal < Map.height ? yVal : 0;
        Population = Mathf.RoundToInt((0.5f + UIData.populationMultiplier) * RandomNum.r.Next(100, 10000));
        Capital = false;
        if (cityList != null)
        {
            cityList.Add(this);
        }
        SetResources(this);
    }

    //create 1 to Map.width number of cities
    public static void GenerateCities(int num)
    {
        if (num > 0 && num < Map.width)
        {
            while (num > 0)
            {
                GenerateCity();
                num--;
            }
        }
    }

    public static void GenerateCapitals()
    {
        if (null == Country.countryList)
        {
            return;
        }

        foreach (Country c in Country.countryList)
        {
            float bestVal = int.MinValue;
            if (c.tilesInCountry == null || c.tilesInCountry.Count == 0)
            {
                continue;
            }

            Tile bestTile = c.tilesInCountry[0];

            foreach (Tile tile in c.tilesInCountry)
            {
                if (tile == null || tile.City != null || tile.Biome == Biome.Ocean)
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

            if (bestTile != null)
            {
                AddCity(bestTile);
                GameObject cityGameObject = Object.Instantiate(Resources.Load<GameObject>("house"));
                if (cityGameObject != null)
                {
                    cityGameObject.transform.SetParent(Map.Houses.transform);
                    cityGameObject.transform.position = new Vector3(bestTile.X, (bestTile.Elevation / 10) + 1, bestTile.Y);
                    cityGameObject.transform.localScale = 1.3f * cityGameObject.transform.localScale;
                }

                City newCity = new City(bestTile.X, bestTile.Y);

                if (newCity != null)
                {
                    newCity.Wealth *= RandomNum.r.Next(2, 3);
                    newCity.Population *= RandomNum.r.Next(2, 5);
                    newCity.Capital = true;
                    newCity.House = cityGameObject;
                    bestTile.country.hasCapital = true;
                    bestTile.City = newCity;
                }
            }
        }
    }

    //create a single city
    private static void GenerateCity()
    {
        Tile[,] tiles = Map.tiles;

        if(tiles == null)
        {
            return;
        }

        float bestVal = int.MinValue;
        ref Tile bestTile = ref tiles[0, 0];

        if (bestTile == null)
        {
            return;
        }

        foreach (Tile tile in tiles)
        {
            if (tile == null || tile.City != null || tile.Biome == Biome.Ocean)
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

        if (bestTile != null)
        {
            AddCity(bestTile);
            GameObject cityGameObject = UnityEngine.Object.Instantiate(Resources.Load<GameObject>("house"));

            if(cityGameObject != null)
            {
                cityGameObject.transform.SetParent(Map.Houses.transform);
                cityGameObject.transform.position = new Vector3(bestTile.X, (bestTile.Elevation / 10) + 1, bestTile.Y);

                City newCity = new City(bestTile.X, bestTile.Y);

                if (newCity != null)
                {
                    newCity.House = cityGameObject;
                    bestTile.City = newCity;
                }
            }
        }
    }

    public static void PlaceNewCity(Tile tile)
    {
        if (tile != null && tile.City == null)
        {
            AddCity(tile);
            GameObject cityGameObject = Object.Instantiate(Resources.Load<GameObject>("house"));

            if (cityGameObject != null)
            {
                cityGameObject.transform.position = new Vector3(tile.X, (tile.Elevation / 10) + 1, tile.Y);
                cityGameObject.transform.SetParent(Map.Houses.transform);
                City newCity = new City(tile.X, tile.Y);

                if (newCity != null)
                {
                    newCity.House = cityGameObject;
                    tile.City = newCity;

                    if (tile.country != null && tile.country.hasCapital == false)
                    {
                        cityGameObject.transform.localScale = 1.3f * cityGameObject.transform.localScale;
                        newCity.Wealth *= RandomNum.r.Next(2, 3);
                        newCity.Population *= RandomNum.r.Next(2, 5);
                        newCity.Capital = true;
                        tile.country.hasCapital = true;
                    }
                }
            }
        }
    }

    public static void TradeRouteFood(City start)
    {
        if (start == null)
        {
            return;
        }

        float bestGuessVal = 0;
        City bestGuessCity = null;
        foreach (City c in cityList)
        {
            if (c != null && c != start && c.Food > bestGuessVal)
            {
                bestGuessVal = c.Food;
                bestGuessCity = c;
            }
        }

        if (bestGuessCity != null)
        {
            Road.CreateRoad(start, bestGuessCity);
        }
    }
    public static void TradeRouteWater(City start)
    {
        if (start == null)
        {
            return;
        }

        float bestGuessVal = 0;
        City bestGuessCity = null;
        foreach (City c in cityList)
        {
            if (c != null && c.Water > bestGuessVal && c != start)
            {
                bestGuessVal = c.Water;
                bestGuessCity = c;
            }
        }

        if (bestGuessCity != null)
        {
            Road.CreateRoad(start, bestGuessCity);
        }
    }
    public static void TradeRouteLumber(City start)
    {
        if (start == null)
        {
            return;
        }

        float bestGuessVal = 0;
        City bestGuessCity = null;
        foreach (City c in cityList)
        {
            if (c != null && c.Lumber > bestGuessVal && c != start)
            {
                bestGuessVal = c.Lumber;
                bestGuessCity = c;
            }
        }

        if (bestGuessCity != null)
        {
            Road.CreateRoad(start, bestGuessCity);
        }
    }

    private static void AddCity(Tile tile)
    {
        Tile[,] tiles = Map.tiles;

        if (tile == null || tiles == null)
        {
            return;
        }

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
        Tile[,] tiles = Map.tiles;

        if (tile.City != null && tiles != null)
        {
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
            if(tile.City != null && tile.City.Capital)
            {
                tile.country.hasCapital = false;
            }

            if (tile.City != null)
            {
                Object.Destroy(tile.City.House);
                _ = cityList.Remove(tile.City);
                tile.City = null;
            }
        }
    }

    private static void SetResources(City city)
    {
        Tile[,] tiles = Map.tiles;

        if (city == null || tiles == null)
        {
            return;
        }

        for (int i = -Map.scanRadius; i <= Map.scanRadius; i++)
        {
            for (int j = -Map.scanRadius; j <= Map.scanRadius; j++)
            {
                if (city.X + i < 0 || city.Y + j < 0 || city.X + i > Map.width - 1 || city.Y + j > Map.height - 1)
                {
                    continue;
                }

                Tile temp = tiles[city.X + i, city.Y + j];

                if(temp == null)
                {
                    continue;
                }

                switch (temp.Biome)
                {
                    case Biome.Ocean:
                        city.Water += 50;
                        city.Food += 20;
                        break;
                    case Biome.Mountain:
                        city.Food += 5;
                        city.Lumber += 10;
                        break;
                    case Biome.Tundra:
                        city.Lumber += 10;
                        city.Water += 10;
                        city.Food += 5;
                        break;
                    case Biome.BorealForest:
                        city.Water += 15;
                        city.Food += 15;
                        city.Lumber += 15;
                        break;
                    case Biome.Prairie:
                        city.Food += 15;
                        city.Water += 10;
                        break;
                    case Biome.Shrubland:
                        city.Food += 15;
                        city.Water += 10;
                        city.Lumber += 5;
                        break;
                    case Biome.TemperateForest:
                        city.Food += 15;
                        city.Lumber += 25;
                        city.Water += 10;
                        break;
                    case Biome.Desert:
                        city.Food += 5;
                        break;
                    case Biome.Savanna:
                        city.Water += 10;
                        city.Lumber += 20;
                        city.Food += 20;
                        break;
                    case Biome.Rainforest:
                        city.Water += 30;
                        city.Lumber += 20;
                        city.Food += 15;
                        break;
                }
            }
        }
        city.Wealth = Mathf.RoundToInt(RandomNum.r.Next(10, 100) * UIData.wealthMultiplier * (city.Food + city.Water + city.Lumber));
    }

    public static int BiomeValue(Tile tile)
    {
        if(tile != null)
        {
            return 0;
        }

        float randomness = (float)RandomNum.r.NextDouble();
        return tile.Biome switch
        {
            Biome.BorealForest => Mathf.RoundToInt(1.2f * randomness),
            Biome.Desert => Mathf.RoundToInt(1 * randomness),
            Biome.Mountain => Mathf.RoundToInt(1.1f * randomness),
            Biome.Ocean => Mathf.RoundToInt(1.2f * randomness),
            Biome.Prairie => Mathf.RoundToInt(1.15f * randomness),
            Biome.Rainforest => Mathf.RoundToInt(1.2f * randomness),
            Biome.Savanna => Mathf.RoundToInt(1.2f * randomness),
            Biome.Shrubland => Mathf.RoundToInt(1.15f * randomness),
            Biome.TemperateForest => Mathf.RoundToInt(1.2f * randomness),
            Biome.Tundra => Mathf.RoundToInt(1.15f * randomness),
            _ => 0,
        };
    }

    public static void LoadCityNames()
    {
        string[] cityNameFile = File.ReadAllLines("CityNames.txt");
        if (cityNameFile != null)
        {
            cityNames = new List<string>(cityNameFile);
        }
    }
}
