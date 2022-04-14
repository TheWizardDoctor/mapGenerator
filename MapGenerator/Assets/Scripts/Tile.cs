using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum Biome { Ocean, Mountain, Tundra, BorealForest, Prairie, Shrubland, TemperateForest, Desert, Savanna, Rainforest };

public class Tile
{
    static readonly GameObject cubePrefab = Resources.Load<GameObject>("Tile");

    //attributes
    //elavation is in 100m scale aka 60 = 6000m
    private Biome biome;
    private float elevation;
    private float precipitation;
    private City city;
    private bool road;
    private int x;
    private int y;
    private float fVal;
    private float gVal;
    private float hVal;
    private float latitude;
    private bool explored;
    private Transform tileSet;
    public Tile previous = null;
    public Tile up = null;
    public Tile down = null;
    public Tile left = null;
    public Tile right = null;
    public float tileValue;
    public GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);


    //constructor
    public Tile()
    {
        biome = Biome.Ocean;
        elevation = 0;
        precipitation = 0;
        road = false;
        explored = false;
    }
    public Tile(int xCord, int yCord, Transform ts)
    {
        tileSet = ts;
        cube.transform.SetParent(tileSet);
        elevation = -1;
        precipitation = 0;
        road = false;
        biome = Biome.Ocean;
        explored = false;
        x = xCord;
        y = yCord;
        latitude = ((yCord + 1) * 90 / (Map.height / 2)) - 90;

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
    public float GVal
    {
        get => gVal;
        set => gVal = value;
    }
    public float HVal
    {
        get => hVal;
        set => hVal = value;
    }
    public float FVal
    {
        get => fVal;
        set => fVal = value;
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
    public bool Explored
    {
        get => explored;
        set => explored = value;
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
                    tile.tileValue += City.HasCity(temp);
                }
            }
        }
    }

    public int calculateBiome()
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

        float l = Math.Abs(latitude);
        double temperature = (((elevation * -0.8 + 40) + (30 - l * 1.7 + 0.059 * Math.Pow(l, 2) - 0.0007 * Math.Pow(l, 3)) * 3) / 4);


        cube.transform.localScale = new Vector3(1, elevation / 10 + 1, 1);
        cube.transform.position = new Vector3(x, (elevation / 10 + 1) / 2, y);
        cube.gameObject.GetComponent<MeshRenderer>().receiveShadows = false;

        if (elevation < 5)
        {
            biome = Biome.Ocean;
            cube.GetComponent<Renderer>().material = oceanMat;
        }
        else if (elevation >= 50)
        {
            biome = Biome.Mountain;
            cube.GetComponent<Renderer>().material = mountainMat;
        }
        else if (temperature <= 0)
        {
            if (precipitation < 100)
            {
                biome = Biome.Tundra;
                cube.GetComponent<Renderer>().material = tundraMat;
            }
            else
            {
                biome = Biome.BorealForest;
                cube.GetComponent<Renderer>().material = borealMat;
            }
        }
        else if (temperature <= 20)
        {
            if (precipitation < 100)
            {
                biome = Biome.Prairie;
                cube.GetComponent<Renderer>().material = prairieMat;
            }
            else if (precipitation < 200)
            {
                biome = Biome.Shrubland;
                cube.GetComponent<Renderer>().material = shrublandMat;
            }
            else
            {
                biome = Biome.TemperateForest;
                cube.GetComponent<Renderer>().material = temperateForestMat;
            }
        }
        else
        {
            if (precipitation < 100)
            {
                biome = Biome.Desert;
                cube.GetComponent<Renderer>().material = desertMat;
            }
            else if (precipitation < 200)
            {
                biome = Biome.Savanna;
                cube.GetComponent<Renderer>().material = savanahMat;
            }
            else
            {
                biome = Biome.Rainforest;
                cube.GetComponent<Renderer>().material = rainforestMat;
            }
        }
        return 0;
    }
}
