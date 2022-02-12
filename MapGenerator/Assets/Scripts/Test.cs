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
        Tile[,] t = new Tile[height, width];

        for(int i=0; i<height; i++)
        {
            for (int j=0; j<width; j++)
            {
                t[i, j] = new Tile(Biome.BorealForest, i, j);
                t[i, j].Elevation = Random.value * 10;
                t[i, j].Precipitation = Random.value * 20;
                if(j>0)
                {
                    t[i, j].left = t[i, j-1];
                }
                if(j<width-1)
                {
                    t[i, j].right = t[i, j+1];
                }
                if(i>0)
                {
                    t[i, j].up = t[i - 1, j];
                }
                if (i<height-1)
                {
                    t[i, j].down = t[i+1, j];
                }

            }
        }

        printValues(t);
        
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
