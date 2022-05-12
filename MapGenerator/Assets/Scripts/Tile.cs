using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum Biome { Ocean, Mountain, Tundra, BorealForest, Prairie, Shrubland, TemperateForest, Desert, Savanna, Rainforest };

public class Tile
{
    private static readonly GameObject cubePrefab = Resources.Load<GameObject>("Tile");

    //attributes
    //elavation is in 100m scale aka 60 = 6000m

    //Tile Attributes
    private Biome biome;
    private float elevation;
    private double temperature;
    private float latitude;
    private float precipitation;
    public Country country;
    private City city;
    private bool road;
    private int x;
    private int y;
    public Tile up = null;
    public Tile down = null;
    public Tile left = null;
    public Tile right = null;
    private Transform tileSet;
    public GameObject cube = UnityEngine.Object.Instantiate(cubePrefab);


    public Tile previous = null;

    //City Stuff
    public float tileValue;

    //Border stuff
    public float border = 0;
    private float navDifficulty = 0;

    //constructor
    public Tile(int xCord, int yCord)
    {
        elevation = -1;
        precipitation = 0;
        temperature = 21;
        biome = Biome.Ocean;
        x = xCord;
        y = yCord;
        latitude = ((yCord + 1) * 90 / (Map.height / 2)) - 90;
        country = Country.unclaimedLand;
    }

    //Serialize
    public String SerializeTile()
    {
        return JsonUtility.ToJson(this);
    }

    //properties
    public float Elevation
    {
        get => elevation;
        set => elevation = value;
    }
    public float Precipitation
    {
        get => precipitation;
        set => precipitation = value;
    }
    public double Temperature
    {
        get => temperature;
        set => temperature = value;
    }
    public Biome Biome
    {
        get => biome;
        set => biome = value;
    }
    public City City
    {
        get => city;
        set => city = value;
    }
    public bool Road
    {
        get => road;
        set => road = value;
    }

    public int X
    {
        get => x;
        set => x = value;//cube.transform.position = new Vector3(x, 0, y);
    }
    public int Y
    {
        get => y;
        set => y = value;//cube.transform.position = new Vector3(x, 0, y);
    }
    public float Latitude
    {
        get => latitude;
        set => latitude = value;
    }
    public float Border
    {
        get { return border; }
        set { border = value; }
    }
    public float NavigationDifficulty
    {
        get { return navDifficulty; }
        set { navDifficulty = value; }
    }

    //methods
    public static void CalculateAllValues()
    {
        Tile[,] tiles = Map.tiles;

        foreach (Tile tile in tiles)
        {
            tile.tileValue = 0;

            for (int i = -Map.scanRadius; i <= Map.scanRadius; i++)
            {
                for (int j = -Map.scanRadius; j <= Map.scanRadius; j++)
                {
                    if (tile.X + i < 0 || tile.Y + j < 0 || tile.X + i > Map.width - 1 || tile.Y + j > Map.height - 1)
                    {
                        continue;
                    }

                    Tile temp = tiles[tile.X + i, tile.Y + j];
                    tile.tileValue += City.BiomeValue(temp);
                }
            }
        }
    }

    public int CalculateBiome()
    {
        Material borealMat = Resources.Load("BorealForest", typeof(Material)) as Material;
        Material desertMat = Resources.Load("Desert", typeof(Material)) as Material;
        Material mountainMat = Resources.Load("Mountain", typeof(Material)) as Material;
        Material oceanMat = Resources.Load("Ocean", typeof(Material)) as Material;
        Material prairieMat = Resources.Load("Prairie", typeof(Material)) as Material;
        Material rainforestMat = Resources.Load("Rainforest", typeof(Material)) as Material;
        Material savanahMat = Resources.Load("Savannah", typeof(Material)) as Material;
        Material shrublandMat = Resources.Load("Shrubland", typeof(Material)) as Material;
        Material temperateForestMat = Resources.Load("TemperateForest", typeof(Material)) as Material;
        Material tundraMat = Resources.Load("Tundra", typeof(Material)) as Material;

        float lat = Math.Abs(latitude);
        //temp range: ~ -50 - 60
        //temperature = (((elevation * -0.8 + 40) + (30 - lat * 1.7 + 0.059 * Math.Pow(lat, 2) - 0.0007 * Math.Pow(lat, 3)) * 3) / 4);
        temperature = (40 - lat) + ((elevation * -1 + 30)/2);

        cube.transform.localScale = new Vector3(1, elevation / 10 + 1, 1);
        cube.transform.position = new Vector3(x, (elevation / 10 + 1) / 2, y);

        if (elevation < UIData.oceanMultiplier*10 && UIData.oceanMultiplier!=0)
        {
            cube.transform.SetParent(Map.OceanTiles.transform);
            cube.transform.localScale = new Vector3(1, 5 / 10 + 1, 1);
            cube.transform.position = new Vector3(x, 5 / 10 + 1, y);
            biome = Biome.Ocean;
            cube.GetComponent<Renderer>().material = oceanMat;
            navDifficulty = 12;
        }
        else if (elevation >= 70 - UIData.mountainMultiplier*40)
        {
            //make mountains appear taller on the map
            float mult = 1.25f;
            if(elevation > 70){
                elevation = mult * elevation;
                cube.transform.localScale = new Vector3(1, elevation / 10 + 1, 1);
            }
            if(elevation > 69){
                elevation = mult * elevation;
                cube.transform.localScale = new Vector3(1, elevation / 10 + 1, 1);
            }
            if(elevation > 67){
                elevation = mult * elevation;
                cube.transform.localScale = new Vector3(1, elevation / 10 + 1, 1);
            }
            if(elevation > 65){
                elevation = mult * elevation;
                cube.transform.localScale = new Vector3(1, elevation / 10 + 1, 1);
            }
            if(elevation > 63){
                elevation = mult * elevation;
                cube.transform.localScale = new Vector3(1, elevation / 10 + 1, 1);
            }
            if(elevation > 60){
                elevation = mult * elevation;
                cube.transform.localScale = new Vector3(1, elevation / 10 + 1, 1);
            }
            if(elevation > 55){
                elevation = mult * elevation;
                cube.transform.localScale = new Vector3(1, elevation / 10 + 1, 1);
            }
            if(elevation > 50){
                elevation = mult * elevation;
                cube.transform.localScale = new Vector3(1, elevation / 10 + 1, 1);
            }
            if(elevation > 45){
                elevation = mult * elevation;
                cube.transform.localScale = new Vector3(1, elevation / 10 + 1, 1);
            }

            cube.transform.SetParent(Map.MountainTiles.transform);
            biome = Biome.Mountain;
            cube.GetComponent<Renderer>().material = mountainMat;
            navDifficulty = 9;
        }
        else {

            if (temperature <= Map.coldDistro)
            {
                if (precipitation < Map.tundraDistro)
                {
                    cube.transform.SetParent(Map.TundraTiles.transform);
                    biome = Biome.Tundra;
                    cube.GetComponent<Renderer>().material = tundraMat;
                    navDifficulty = 7;
                }
                else //(BorealForest)
                {
                    cube.transform.SetParent(Map.BorealForestTiles.transform);
                    biome = Biome.BorealForest;
                    cube.GetComponent<Renderer>().material = borealMat;
                    navDifficulty = 5;
                }
            }
            else if (temperature <= Map.temperateDistro)
            {
                if (precipitation < Map.prairieDistro)
                {
                    cube.transform.SetParent(Map.PrairieTiles.transform);
                    biome = Biome.Prairie;
                    cube.GetComponent<Renderer>().material = prairieMat;
                    navDifficulty = 1;
                }
                else if (precipitation < Map.shrublandDistro)
                {
                    cube.transform.SetParent(Map.ShrublandTiles.transform);
                    biome = Biome.Shrubland;
                    cube.GetComponent<Renderer>().material = shrublandMat;
                    navDifficulty = 2;
                }
                else //(BorealForestDistro)
                {
                    cube.transform.SetParent(Map.TemperateForestTiles.transform);
                    biome = Biome.TemperateForest;
                    cube.GetComponent<Renderer>().material = temperateForestMat;
                    navDifficulty = 3;
                }
            }
            else //(WarmDistro)
            {
                if (precipitation < Map.desertDistro)
                {
                    cube.transform.SetParent(Map.DesertTiles.transform);
                    biome = Biome.Desert;
                    cube.GetComponent<Renderer>().material = desertMat;
                    navDifficulty = 6;
                }
                else if (precipitation < Map.savannaDistro)
                {
                    cube.transform.SetParent(Map.SavannaTiles.transform);
                    biome = Biome.Savanna;
                    cube.GetComponent<Renderer>().material = savanahMat;
                    navDifficulty = 4;
                }
                else //(RainForestDistro)
                {
                    cube.transform.SetParent(Map.RainforestTiles.transform);
                    biome = Biome.Rainforest;
                    cube.GetComponent<Renderer>().material = rainforestMat;
                    navDifficulty = 8;
                }
            }
        }
        return 0;
    }
}
