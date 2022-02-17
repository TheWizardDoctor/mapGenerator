using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    public GameObject tilePrefab;
    int width = 100;
    int height = 100;
    int c = 1;

    // Start is called before the first frame update
    void Start()
    {
		GameObject tileSet = new GameObject("Tiles");
		
        Tile[,] t = new Tile[height, width];

        var filePath = @"Book1.csv";
		var data = File.ReadLines(filePath);
		int[,] exampleMap = new int[height,width];
		int indexi = 0;		
		int indexj = 0;
		foreach (string row in data){
			indexj = 0;
			string[] items = row.Split(',');
			foreach (string s in items){
				int n = Convert.ToInt32(s);
				exampleMap[indexi,indexj] = n;
				indexj++;
			}
			indexi++;
		}
		
		for(int i=0; i<height; i++)
        {
            for (int j=0; j<width; j++)
            {
                t[i, j] = new Tile(Biome.BorealForest, i * 10, j * 10, tileSet.transform);
				t[i, j].Elevation = exampleMap[i,j];
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

        
    }

    private void printValues(Tile[,] t)
    {
        for (int i = 0; i < 10; i++)
        {
            for (int j = 0; j < 10; j++)
            {
                print("B:" + t[i, j].Biome + " E:" + t[i, j].Elevation + " P:" + t[i, j].Precipitation + "\t");
            }
        }
    }

    private void printCities(Tile[,] t)
    {
        for (int i = 0; i < 10; i++)
        {
            for (int j = 0; j < 10; j++)
            {
                if(t[i,j].City)
                {
                    print("City found at i:" + i + " j:" + j + "\n");
                }
            }
        }
    }
}
