using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Map : MonoBehaviour
{
    public static int width;
    public static int height;
    public static Tile[,] tiles;

    public static void CreateMap(int w, int h)
    {
		width = w;
		height = h;

		GameObject tileSet = new GameObject("Tiles");

		tiles = new Tile[height, width];

		var filePath = @"Elevation.csv";
		var data = File.ReadLines(filePath);
		int[,] exampleMapElevation = new int[height, width];
		int indexi = 0;
		int indexj;
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

		for (int i = 0; i < height; i++)
		{
			for (int j = 0; j < width; j++)
			{
				if (j > 0)
				{
					tiles[i, j].left = tiles[i, j - 1];
				}

				if (j < width - 1)
				{
					tiles[i, j].right = tiles[i, j + 1];
				}

				if (i > 0)
				{
					tiles[i, j].up = tiles[i - 1, j];
				}

				if (i < height - 1)
				{
					tiles[i, j].down = tiles[i + 1, j];
				}
			}
		}
	}
}
