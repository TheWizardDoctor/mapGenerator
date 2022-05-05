using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum Biome { Ocean, Mountain, Tundra, BorealForest, Prairie, Shrubland, TemperateForest, Desert, Savanna, Rainforest };

[Serializable]
public class Tile
{
    private static readonly GameObject cubePrefab = Resources.Load<GameObject>("Tile");

    //attributes
    //elavation is in 100m scale aka 60 = 6000m

    //Tile Attributes
    private Biome biome;
    [SerializeField]
    private float elevation;
    [SerializeField]
    private float latitude;
    [SerializeField]
    private float precipitation;
    private City city;
    [SerializeField]
    private bool road;
    [SerializeField]
    private int x;
    [SerializeField]
    private int y;
    [SerializeField]
    public Tile up = null;
    [SerializeField]
    public Tile down = null;
    [SerializeField]
    public Tile left = null;
    [SerializeField]
    public Tile right = null;
    private Transform tileSet;
    [NonSerialized]
    public GameObject cube = UnityEngine.Object.Instantiate(cubePrefab);

    [NonSerialized]
    public Tile previous = null;

    //City Stuff
    public float tileValue;

    //Border stuff
    public float border = 0;
    private float navDifficulty = 0;

    //constructor
    public Tile()
    {
        biome = Biome.Ocean;
        elevation = 0;
        precipitation = 0;
    }
    public Tile(int xCord, int yCord)
    {
        elevation = -1;
        precipitation = 0;
        biome = Biome.Ocean;
        x = xCord;
        y = yCord;
        latitude = ((yCord + 1) * 90 / (Map.height / 2)) - 90;
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

        if (elevation < 4.5+UIData.oceanMultiplier)
        {
            cube.transform.SetParent(Map.OceanTiles.transform);
            cube.transform.localScale = new Vector3(1, 5 / 10 + 1, 1);
            cube.transform.position = new Vector3(x, 5 / 10 + 1, y);
            biome = Biome.Ocean;
            cube.GetComponent<Renderer>().material = oceanMat;
            navDifficulty = 12;
        }
        else if (elevation >= 50 - UIData.mountainMultiplier*3)
        {
            cube.transform.SetParent(Map.MountainTiles.transform);
            biome = Biome.Mountain;
            cube.GetComponent<Renderer>().material = mountainMat;
            navDifficulty = 9;
        }
        else if (temperature <= 0)
        {
            if (precipitation-UIData.tundraMultiplier*3+UIData.borealForestMultiplier*3 < 100)
            {
                cube.transform.SetParent(Map.TundraTiles.transform);
                biome = Biome.Tundra;
                cube.GetComponent<Renderer>().material = tundraMat;
                navDifficulty = 7;
            }
            else
            {
                cube.transform.SetParent(Map.BorealForestTiles.transform);
                biome = Biome.BorealForest;
                cube.GetComponent<Renderer>().material = borealMat;
                navDifficulty = 5;
            }
        }
        else if (temperature <= 20)
        {
            if (precipitation-UIData.prairieMultiplier*3 < 100)
            {
                cube.transform.SetParent(Map.PrairieTiles.transform);
                biome = Biome.Prairie;
                cube.GetComponent<Renderer>().material = prairieMat;
                navDifficulty = 1;
            }
            else if (precipitation-UIData.shrublandMultiplier*3 < 200)
            {
                cube.transform.SetParent(Map.ShrublandTiles.transform);
                biome = Biome.Shrubland;
                cube.GetComponent<Renderer>().material = shrublandMat;
                navDifficulty = 2;
            }
            else
            {
                cube.transform.SetParent(Map.TemperateForestTiles.transform);
                biome = Biome.TemperateForest;
                cube.GetComponent<Renderer>().material = temperateForestMat;
                navDifficulty = 3;
            }
        }
        else
        {
            if (precipitation-UIData.desertMultiplier*3 < 100)
            {
                cube.transform.SetParent(Map.DesertTiles.transform);
                biome = Biome.Desert;
                cube.GetComponent<Renderer>().material = desertMat;
                navDifficulty = 6;
            }
            else if (precipitation-UIData.savannahMultiplier*3 < 200)
            {
                cube.transform.SetParent(Map.SavannaTiles.transform);
                biome = Biome.Savanna;
                cube.GetComponent<Renderer>().material = savanahMat;
                navDifficulty = 4;
            }
            else
            {
                cube.transform.SetParent(Map.RainforestTiles.transform);
                biome = Biome.Rainforest;
                cube.GetComponent<Renderer>().material = rainforestMat;
                navDifficulty = 8;
            }
        }
        return 0;
    }
}
