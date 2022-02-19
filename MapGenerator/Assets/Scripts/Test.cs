using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    public GameObject tilePrefab;
	public Camera cam;
    int width = 300;	//x
    int height = 200;	//y

    // Start is called before the first frame update
    void Start()
    {
		cam.transform.position = new Vector3(width/2, height, height/2);

		GameObject tileSet = new GameObject("Tiles");
		
        Tile[,] t = new Tile[width, height];

		//load the example map csvs
		/*
        var filePath = @"Elevation.csv";
		var data = File.ReadLines(filePath);
		int[,] exampleMapElevation = new int[width,height];
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
                exampleMapElevation[indexj,indexi] = n;
				indexj++;
			}
			indexi++;
		}
		filePath = @"Precipitation.csv";
		data = File.ReadLines(filePath);
		int[,] exampleMapPrecipitation = new int[width,height];
		indexi = 0;		
		indexj = 0;
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
				exampleMapPrecipitation[indexj,indexi] = n;
				indexj++;
			}
			indexi++;
		}
		*/
		
		//set initial tiles
		for(int j=0; j<height; j++)
        {
            for (int i=0; i<width; i++)
            {
                t[i, j] = new Tile(height, i, j, tileSet.transform);
				//t[i, j].Elevation = exampleMapElevation[i,j];
				//t[i, j].Precipitation = exampleMapPrecipitation[i,j];
				t[i, j].calculateBiome();
            }
        }
		
		
		//setting lefts, rights, ups, and downs of the tiles
		for(int j = 0; j < height; j++){
			for(int i = 0; i < width; i++){
				if(i > 0)
                {
                    t[i, j].left = t[i - 1, j];
                } else {
					t[i, j].left = t[width - 1, j];
				}
				
                if(i < width - 1)
                {
                    t[i, j].right = t[i + 1, j];
                } else {
					t[i, j].right = t[0, j];
				}
				
                if(j > 0)
                {
                    t[i, j].down = t[i, j - 1];
                }
				
                if (j < height - 1)
                {
                    t[i, j].up = t[i, j + 1];
                }
			}
		}

		CreateTerrain.createInitialMountains(t, width, height);

		//very simplistic city creation
		//(currently only checks 8 nearby tiles to get tile's creation value)
        //CreateCities.generateCities(t, 10);
        
    }
}
