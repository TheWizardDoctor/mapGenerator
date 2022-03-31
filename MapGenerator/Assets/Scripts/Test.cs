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
		//used for orthographic Camera
		//cam.orthographicSize = 50;

		//used for perspective Camera
		cam.transform.position = new Vector3(height/2, height, width/2);

		Map.createMap(width, height);


		List<Tile> unsetTiles = CreateTerrain.createInitialMountains(t, width, height);
		while(unsetTiles.Count > 0){
			CreateTerrain.setElevations(unsetTiles);
		}
		
		for(int j=0; j<height; j++)
        {
            for (int i=0; i<width; i++)
            {
                t[i, j].cube.transform.localScale = new Vector3(1, t[i, j].Elevation/10 + 1, 1);
				t[i, j].cube.transform.position = new Vector3(t[i, j].X, (t[i, j].Elevation/10 + 1)/2, t[i, j].Y);
				t[i, j].calculateBiome();
            }
        }
		//very simplistic city creation
		//(currently only checks 8 nearby tiles to get tile's creation value)

		var watch = System.Diagnostics.Stopwatch.StartNew();
		City.generateCities(Map.tiles, 0);
		watch.Stop();
		//Debug.Log("Time to create 0 cities is:" + watch.ElapsedMilliseconds + "ms");
	}
}
