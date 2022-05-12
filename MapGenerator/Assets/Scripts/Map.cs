using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Map
{
	[SerializeField]
    public static int width;
	[SerializeField]
	public static int height;
    public static int scanRadius;
    public static Tile[,] tiles;

	public static GameObject tileSet;
	public static GameObject OceanTiles;
	public static GameObject DesertTiles;
	public static GameObject BorealForestTiles;
	public static GameObject MountainTiles;
	public static GameObject PrairieTiles;
	public static GameObject RainforestTiles;
	public static GameObject SavannaTiles;
	public static GameObject ShrublandTiles;
	public static GameObject TemperateForestTiles;
	public static GameObject TundraTiles;
	public static GameObject RoadTiles;
	public static GameObject BorderTiles;
	public static GameObject Houses;

	public static float totalDistro;
	public static float tundraDistro;
	public static float borealForestDistro;
	public static float coldDistro;
	public static float prairieDistro;
	public static float shrublandDistro;
	public static float temperateForestDistro;
	public static float temperateDistro;
	public static float desertDistro;
	public static float savannaDistro;
	public static float rainForestDistro;
	public static float warmDistro;

	public static void createMap(int w, int h)
    {
        width = w;
        height = h;
        scanRadius = (int)Math.Floor(0.02 * width);

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

		//this is the code for loading the demo map
		/*
		var filePath = @"Elevation.csv";
		var data = File.ReadLines(filePath);
		int[,] exampleMapElevation = new int[height, width];
		int indexi = 0;
		int indexj = 0;
		foreach (string row in data)
		{
			indexj = 0;
			string[] items = row.Split(',');
			foreach (string s in items)
			{
				int n;
				if (s == "")
				{
					n = 0;
				}
				else
				{
					n = Convert.ToInt32(s);
				}
				exampleMapElevation[indexi, indexj] = n;
				indexj++;
			}
			indexi++;
		}
		filePath = @"Precipitation.csv";
		data = File.ReadLines(filePath);
		int[,] exampleMapPrecipitation = new int[height, width];
		indexi = 0;
		indexj = 0;
		foreach (string row in data)
		{
			indexj = 0;
			string[] items = row.Split(',');
			foreach (string s in items)
			{
				int n = Convert.ToInt32(s);
				exampleMapPrecipitation[indexi, indexj] = n;
				indexj++;
			}
			indexi++;
		}


		for (int i = 0; i < height; i++)
		{
			for (int j = 0; j < width; j++)
			{
				tiles[i, j] = new Tile(height, i, j, tileSet.transform);
				tiles[i, j].Elevation = exampleMapElevation[i, j];
				tiles[i, j].Precipitation = exampleMapPrecipitation[i, j];
				tiles[i, j].calculateBiome();
			}
		}
		*/
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

		List<Tile> unsetTiles = CreateTerrain.createInitialMountains();
		while(unsetTiles.Count > 0){
			CreateTerrain.setElevations(unsetTiles);
		}
		
		for(int i=0; i < 5; i++){
			CreateTerrain.unclutterOcean();
		}
		
		List<Tile> unsetPrecepitations = CreateBiomes.createInitialWetZones();
		while(unsetPrecepitations.Count > 0){
			CreateBiomes.setPrecipitations(unsetPrecepitations);
		}
		

		//Determine biome distribution for biome generation:

		//get the total distribution number (sum of sliders)
		Map.totalDistro = 0;
		Map.totalDistro += UIData.tundraMultiplier;
		Map.totalDistro += UIData.borealForestMultiplier;
		Map.totalDistro += UIData.prairieMultiplier;
		Map.totalDistro += UIData.shrublandMultiplier;
		Map.totalDistro += UIData.temperateForestMultiplier;
		Map.totalDistro += UIData.desertMultiplier;
		Map.totalDistro += UIData.savannaMultiplier;
		Map.totalDistro += UIData.rainForestMultiplier;
		//get temp distributions (fraction of total)
		if(totalDistro != 0){
			Map.tundraDistro = UIData.tundraMultiplier / Map.totalDistro;
			Map.borealForestDistro = UIData.borealForestMultiplier / Map.totalDistro;
			Map.coldDistro = Map.tundraDistro + Map.borealForestDistro;
			Map.prairieDistro = UIData.prairieMultiplier / Map.totalDistro;
			Map.shrublandDistro = UIData.shrublandMultiplier / Map.totalDistro;
			Map.temperateForestDistro = UIData.temperateForestMultiplier / Map.totalDistro;
			Map.temperateDistro = Map.prairieDistro + Map.shrublandDistro + Map.temperateForestDistro;
			Map.desertDistro = UIData.desertMultiplier / Map.totalDistro;
			Map.savannaDistro = UIData.savannaMultiplier / Map.totalDistro;
			Map.rainForestDistro = UIData.rainForestMultiplier / Map.totalDistro;
			Map.warmDistro = Map.desertDistro + Map.savannaDistro + Map.rainForestDistro;
		}
		//get precip distributions (fraction of temperature)
		if(coldDistro != 0){
			Map.tundraDistro = Map.tundraDistro / Map.coldDistro;
			Map.borealForestDistro = Map.borealForestDistro / Map.coldDistro;
		}
		if(temperateDistro != 0){
			Map.prairieDistro = Map.prairieDistro / Map.temperateDistro;
			Map.shrublandDistro = Map.shrublandDistro / Map.temperateDistro;
			Map.temperateForestDistro = Map.temperateForestDistro / Map.temperateDistro;
		}
		if(warmDistro != 0){
			Map.desertDistro = Map.desertDistro / Map.warmDistro;
			Map.savannaDistro = Map.savannaDistro / Map.warmDistro;
			Map.rainForestDistro = Map.rainForestDistro / Map.warmDistro;
		}
		//turn these numbers into borders for inequalities
		// !! vv THIS HAS A STRICT ORDER, DO NOT CHANGE WITHOUT KNOWING WHAT YOU ARE DOING, ASK SAM vv !!

		Map.borealForestDistro += Map.tundraDistro; //(should be 1 or 0)
		Map.tundraDistro = -10 + (300 * tundraDistro);
		Map.borealForestDistro = -10 + (300 * borealForestDistro);

		Map.shrublandDistro += Map.prairieDistro;
		Map.temperateForestDistro += Map.shrublandDistro; //(should be 1 or 0)
		Map.prairieDistro = -10 + (300 * prairieDistro);
		Map.shrublandDistro = -10 + (300 * shrublandDistro);
		Map.temperateForestDistro = -10 + (300 * temperateForestDistro);

		Map.savannaDistro += Map.desertDistro;
		Map.rainForestDistro += Map.savannaDistro; //(should be 1 or 0)
		Map.desertDistro = -10 + (300 * desertDistro);
		Map.savannaDistro = -10 + (300 * savannaDistro);
		Map.rainForestDistro = -10 + (300 * rainForestDistro);

		Map.temperateDistro += Map.coldDistro;
		Map.warmDistro += Map.temperateDistro; //(should be 1 or 0)
		Map.coldDistro = -50 + (110 * Map.coldDistro);
		Map.temperateDistro = -50 + (110 * Map.temperateDistro);
		Map.warmDistro = -50 + (110 * Map.warmDistro);

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
