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
                tiles[j, i].calculateBiome();
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
		

        for (int j = 0; j < height; j++)
        {
            for (int i = 0; i < width; i++)
            {
				tiles[i, j].calculateBiome();
            }
        }
		
		Tile.CalculateAllValues();
		City.LoadCityNames();
		Country.LoadCountryNames();
	}
}
