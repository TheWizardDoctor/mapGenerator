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
		//used for orthographic Camera
		//cam.orthographicSize = 50;

		//used for perspective Camera
		cam.transform.position = new Vector3(height/2, height, width/2);

		Map.createMap(width, height);


		//very simplistic city creation
		//(currently only checks 8 nearby tiles to get tile's creation value)
		var watch = System.Diagnostics.Stopwatch.StartNew();
		City.generateCities(Map.tiles, 0);
		watch.Stop();
		//Debug.Log("Time to create 0 cities is:" + watch.ElapsedMilliseconds + "ms");
	}
}
