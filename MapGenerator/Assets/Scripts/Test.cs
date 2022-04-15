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
		cam.transform.position = new Vector3(width/2, height, height/2);

		var watch = System.Diagnostics.Stopwatch.StartNew();

		Map.createMap(width, height);
		watch.Stop();
		Debug.Log("Time to create all tiles is:" + watch.ElapsedMilliseconds + "ms");

		//very simplistic city creation
		//(currently only checks 8 nearby tiles to get tile's creation value)

		//var watch = System.Diagnostics.Stopwatch.StartNew();
		City.GenerateCities(50);
		//watch.Stop();
		//Debug.Log("Time to create 0 cities is:" + watch.ElapsedMilliseconds + "ms");
	}
}
