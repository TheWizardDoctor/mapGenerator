using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    public GameObject tilePrefab;
	public Camera cam;
    int width = 100;
    int height = 100;

    // Start is called before the first frame update
    void Start()
    {
		cam.transform.position = new Vector3(height/2, height, width/2);

		GameObject tileSet = new GameObject("Tiles");
		
        Tile[,] t = new Tile[height, width];

        var filePath = @"Elevation.csv";
		var data = File.ReadLines(filePath);
		int[,] exampleMapElevation = new int[height,width];
		int indexi = 0;		
		int indexj = 0;
		foreach (string row in data){
			indexj = 0;
			string[] items = row.Split(',');
			foreach (string s in items){
                int n;
                if(s=="")
                {
                    n = 0;
                }
                else
                { 
				    n = Convert.ToInt32(s);
                }
                exampleMapElevation[indexi,indexj] = n;
				indexj++;
			}
			indexi++;
		}
		filePath = @"Precipitation.csv";
		data = File.ReadLines(filePath);
		int[,] exampleMapPrecipitation = new int[height,width];
		indexi = 0;		
		indexj = 0;
		foreach (string row in data){
			indexj = 0;
			string[] items = row.Split(',');
			foreach (string s in items){
				int n = Convert.ToInt32(s);
				exampleMapPrecipitation[indexi,indexj] = n;
				indexj++;
			}
			indexi++;
		}
		
		
		for(int i=0; i<height; i++)
        {
            for (int j=0; j<width; j++)
            {
                t[i, j] = new Tile(height, i, j, tileSet.transform);
				t[i, j].Elevation = exampleMapElevation[i,j];
				t[i, j].Precipitation = exampleMapPrecipitation[i,j];
				t[i, j].calculateBiome();
            }
        }

		for(int i = 0; i < height; i++){
			for(int j = 0; j < width; j++){
				if(j > 0)
                {
                    t[i, j].left = t[i, j - 1];
                } else {
					t[i, j].left = t[i, width - 1];
				}
				
                if(j < width - 1)
                {
                    t[i, j].right = t[i, j + 1];
                } else {
					t[i, j].right = t[i, 0];
				}
				
                if(i>0)
                {
                    t[i, j].up = t[i - 1, j];
                }
				
                if (i<height-1)
                {
                    t[i, j].down = t[i + 1, j];
                }
			}
		}

		//very simplistic city creation
		//(currently only checks 8 nearby tiles to get tile's creation value)
		var watch = System.Diagnostics.Stopwatch.StartNew();
		City.generateCities(t, 0);
		watch.Stop();
		Debug.Log("Time to create 0 cities is:" + watch.ElapsedMilliseconds + "ms");

        System.Random r = new System.Random();

		Tile one, two;
		while(true)
        {
			one = t[r.Next(100), r.Next(100)];
			if(one.Biome!=Biome.Ocean)
            {
				break;
            }
		}
		while (true)
		{
			two = t[r.Next(100), r.Next(100)];
			if (two.Biome != Biome.Ocean)
			{
				break;
			}
		}


		watch = System.Diagnostics.Stopwatch.StartNew();
		Road.createRoad(t, one, two);
		watch.Stop();
		Debug.Log("Time to create 1 road(s) is:" + watch.ElapsedMilliseconds + "ms");

	}
}
