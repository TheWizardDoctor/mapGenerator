using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Map
{
    private static Map s;

    public static Map S
    {
        get
        {
            if (s == null)
            {
                s = new Map();
            }
            return s;
        }
    }

    public int width;
    public int height;
    public int scanRadius;
    public Tile[,] tiles;

    public GameObject tileSet;
    public GameObject OceanTiles;
    public GameObject DesertTiles;
    public GameObject BorealForestTiles;
    public GameObject MountainTiles;
    public GameObject PrairieTiles;
    public GameObject RainforestTiles;
    public GameObject SavannaTiles;
    public GameObject ShrublandTiles;
    public GameObject TemperateForestTiles;
    public GameObject TundraTiles;
    public GameObject RoadTiles;
    public GameObject BorderTiles;
    public GameObject Houses;

    public float totalDistro;
    public float tundraDistro;
    public float borealForestDistro;
    public float coldDistro;
    public float prairieDistro;
    public float shrublandDistro;
    public float temperateForestDistro;
    public float temperateDistro;
    public float desertDistro;
    public float savannaDistro;
    public float rainForestDistro;
    public float warmDistro;

    public void CreateMap(int w, int h)
    {
        width = w;
        height = h;
        scanRadius = Mathf.RoundToInt(0.03f * width);

        GameObject tileSet = GameObject.Find("All Tiles");
        tiles = new Tile[width, height];

        OceanTiles = GameObject.Find("Ocean Tiles");
        OceanTiles.transform.SetParent(tileSet.transform);

        DesertTiles = GameObject.Find("Desert Tiles");
        DesertTiles.transform.SetParent(tileSet.transform);

        BorealForestTiles = GameObject.Find("Boreal Forest Tiles");
        BorealForestTiles.transform.SetParent(tileSet.transform);

        MountainTiles = GameObject.Find("Mountain Tiles");
        MountainTiles.transform.SetParent(tileSet.transform);

        PrairieTiles = GameObject.Find("Prairie Tiles");
        PrairieTiles.transform.SetParent(tileSet.transform);

        RainforestTiles = GameObject.Find("Rainforest Tiles");
        RainforestTiles.transform.SetParent(tileSet.transform);

        SavannaTiles = GameObject.Find("Savanna Tiles");
        SavannaTiles.transform.SetParent(tileSet.transform);

        ShrublandTiles = GameObject.Find("Shrubland Tiles");
        ShrublandTiles.transform.SetParent(tileSet.transform);

        TemperateForestTiles = GameObject.Find("Temperate Forest Tiles");
        TemperateForestTiles.transform.SetParent(tileSet.transform);

        TundraTiles = GameObject.Find("Tundra Tiles");
        TundraTiles.transform.SetParent(tileSet.transform);

        RoadTiles = GameObject.Find("Road Tiles");
        RoadTiles.transform.SetParent(tileSet.transform);

        BorderTiles = GameObject.Find("Border Tiles");
        BorderTiles.transform.SetParent(tileSet.transform);

        Houses = GameObject.Find("Houses");

        //creating initial tiles
        for (int i = 0; i < height; i++)
        {
            for (int j = 0; j < width; j++)
            {
                tiles[j, i] = new Tile(j, i);
                tiles[j, i].CalculateBiome();
            }
        }
        //setting lefts and rights and ups and downs
        for (int i = 0; i < height; i++)
        {
            for (int j = 0; j < width; j++)
            {
                if (j > 0)
                {
                    tiles[j, i].left = tiles[j - 1, i];
                }
                else
                {
                    tiles[j, i].left = tiles[width - 1, i];
                }

                if (j < width - 1)
                {
                    tiles[j, i].right = tiles[j + 1, i];
                }
                else
                {
                    tiles[j, i].right = tiles[0, i];
                }

                if (i > 0)
                {
                    tiles[j, i].up = tiles[j, i - 1];
                }

                if (i < height - 1)
                {
                    tiles[j, i].down = tiles[j, i + 1];
                }
            }
        }

        List<Tile> unsetTiles = CreateTerrain.CreateInitialMountains();
        while (unsetTiles.Count > 0)
        {
            CreateTerrain.SetElevations(unsetTiles);
        }

        for (int i = 0; i < 5; i++)
        {
            CreateTerrain.UnclutterOcean();
        }

        List<Tile> unsetPrecepitations = CreateBiomes.createInitialWetZones();
        while (unsetPrecepitations.Count > 0)
        {
            CreateBiomes.setPrecipitations(unsetPrecepitations);
        }


        //Determine biome distribution for biome generation:

        //get the total distribution number (sum of sliders)
        totalDistro = 0;
        totalDistro += UIData.tundraMultiplier;
        totalDistro += UIData.borealForestMultiplier;
        totalDistro += UIData.prairieMultiplier;
        totalDistro += UIData.shrublandMultiplier;
        totalDistro += UIData.temperateForestMultiplier;
        totalDistro += UIData.desertMultiplier;
        totalDistro += UIData.savannaMultiplier;
        totalDistro += UIData.rainForestMultiplier;
        //get temp distributions (fraction of total)
        if (totalDistro != 0)
        {
            tundraDistro = UIData.tundraMultiplier / totalDistro;
            borealForestDistro = UIData.borealForestMultiplier / totalDistro;
            coldDistro = tundraDistro + borealForestDistro;
            prairieDistro = UIData.prairieMultiplier / totalDistro;
            shrublandDistro = UIData.shrublandMultiplier / totalDistro;
            temperateForestDistro = UIData.temperateForestMultiplier / totalDistro;
            temperateDistro = prairieDistro + shrublandDistro + temperateForestDistro;
            desertDistro = UIData.desertMultiplier / totalDistro;
            savannaDistro = UIData.savannaMultiplier / totalDistro;
            rainForestDistro = UIData.rainForestMultiplier / totalDistro;
            warmDistro = desertDistro + savannaDistro + rainForestDistro;
        }
        //get precip distributions (fraction of temperature)
        if (coldDistro != 0)
        {
            tundraDistro = tundraDistro / coldDistro;
            borealForestDistro = borealForestDistro / coldDistro;
        }
        if (temperateDistro != 0)
        {
            prairieDistro = prairieDistro / temperateDistro;
            shrublandDistro = shrublandDistro / temperateDistro;
            temperateForestDistro = temperateForestDistro / temperateDistro;
        }
        if (warmDistro != 0)
        {
            desertDistro = desertDistro / warmDistro;
            savannaDistro = savannaDistro / warmDistro;
            rainForestDistro = rainForestDistro / warmDistro;
        }
        //turn these numbers into borders for inequalities
        // !! vv THIS HAS A STRICT ORDER, DO NOT CHANGE WITHOUT KNOWING WHAT YOU ARE DOING, ASK SAM vv !!

        borealForestDistro += tundraDistro; //(should be 1 or 0)
        tundraDistro = -10 + (300 * tundraDistro);
        borealForestDistro = -10 + (300 * borealForestDistro);

        shrublandDistro += prairieDistro;
        temperateForestDistro += shrublandDistro; //(should be 1 or 0)
        prairieDistro = -10 + (300 * prairieDistro);
        shrublandDistro = -10 + (300 * shrublandDistro);
        temperateForestDistro = -10 + (300 * temperateForestDistro);

        savannaDistro += desertDistro;
        rainForestDistro += savannaDistro; //(should be 1 or 0)
        desertDistro = -10 + (300 * desertDistro);
        savannaDistro = -10 + (300 * savannaDistro);
        rainForestDistro = -10 + (300 * rainForestDistro);

        temperateDistro += coldDistro;
        warmDistro += temperateDistro; //(should be 1 or 0)
        coldDistro = -50 + (110 * coldDistro);
        temperateDistro = -50 + (110 * temperateDistro);
        warmDistro = -50 + (110 * warmDistro);

        // !! ^^ END OF STRICT ORDER, thank you ^^ !!

        for (int j = 0; j < height; j++)
        {
            for (int i = 0; i < width; i++)
            {
                tiles[i, j].CalculateBiome();
            }
        }

        Tile.CalculateAllValues();
        City.LoadCityNames();
        Country.LoadCountryNames();
    }

    /*
	//properties (biome distributions)
	public static float TotalDistro{
		get => totalDistro;
		set => totalDistro = value;
	}
	public static float TundraDistro{
		get => tundraDistro;
		set => tundraDistro = value;
	}
	public static float BorealForestDistro{
		get => borealForestDistro;
		set => borealForestDistro = value;
	}
	public static float ColdDistro{
		get => coldDistro;
		set => coldDistro = value;
	}
	public static float PrairieDistro{
		get => prairieDistro;
		set => prairieDistro = value;
	}
	public static float ShrublandDistro{
		get => shrublandDistro;
		set => shrublandDistro = value;
	}
	public static float TemperateForestDistro{
		get => temperateForestDistro;
		set => temperateForestDistro = value;
	}
	public static float TemperateDistro{
		get => temperateDistro;
		set => temperateDistro = value;
	}
	public static float DesertDistro{
		get => desertDistro;
		set => desertDistro = value;
	}
	public static float SavannaDistro{
		get => savannaDistro;
		set => savannaDistro = value;
	}
	public static float RainForestDistro{
		get => rainForestDistro;
		set => rainForestDistro = value;
	}
	public static float WarmDistro{
		get => warmDistro;
		set => warmDistro = value;
	}
	*/
}
