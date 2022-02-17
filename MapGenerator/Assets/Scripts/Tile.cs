using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Biome { Ocean, Mountain, Tundra, BorealForest, Prairie, Shrubland, TemperateForest, Desert, Savannah, Rainforest };

public class Tile
{
    //attributes
    private Biome biome;
    private float elevation;
    private float precipitation;
    private bool city;
    private bool road;
	private int x;
	private int y;
	private float latitude;
    public Tile up = null;
    public Tile down = null;
    public Tile left = null;
    public Tile right = null;
	
	public GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
	
    //constructor
    public Tile()
    {
        biome = Biome.Ocean;
        elevation = 0;
        precipitation = 0;
        city = false;
        road = false;
    }
    public Tile(int height, int xCord, int yCord, Transform tileSet)
    {
        elevation = 0;
        precipitation = 0;
        city = false;
        road = false;
		x = xCord;
		y = yCord;
		latitude = ((xCord/10 + 1) * 90/(height/2)) - 90;
		cube.transform.SetParent(tileSet);
		cube.transform.position = new Vector3(x, (elevation + 10)/2, y);
		cube.transform.localScale = new Vector3(10, elevation + 10, 10);
    }

    //properties
    public float Elevation
    {
        get { return elevation; }
        set { elevation = value; 
			cube.transform.localScale = new Vector3(10, elevation + 10, 10);
			cube.transform.position = new Vector3(x, (elevation + 10)/2, y);
		}
    }
    public float Precipitation
    {
        get { return precipitation; }
        set { precipitation = value; }
    }
    public Biome Biome
    {
        get { return biome; }
        set { biome = value; }
    }
    public bool City
    {
        get { return city; }
        set { city = value; }
    }
    public bool Road
    {
        get { return road; }
        set { Road = value; }
    }
	public int X
    {
        get { return x; }
        set { x = value; 
			cube.transform.position = new Vector3(x, 0, y);
		}
    }
	public int Y
    {
        get { return y; }
        set { y = value; 
			cube.transform.position = new Vector3(x, 0, y);
		}
    }
	public float Latitude
    {
        get { return latitude; }
        set { latitude = value; }
    }
    //methods
	
	public int calculateBiome()
	{
		Material borealMat = Resources.Load("BorealForest", typeof(Material)) as Material;
		Material desertMat = Resources.Load("Desert", typeof(Material)) as Material;
		Material mountainMat = Resources.Load("Mountain", typeof(Material)) as Material;
		Material oceanMat = Resources.Load("Ocean", typeof(Material)) as Material;
		Material prairieMat = Resources.Load("Prairie", typeof(Material)) as Material;
		Material rainforestMat = Resources.Load("Rainforest", typeof(Material)) as Material;
		Material savanahMat = Resources.Load("Savannah", typeof(Material)) as Material;
		Material shrublandMat = Resources.Load("Shrubland", typeof(Material)) as Material;
		Material temperateForestMat = Resources.Load("TemperateForest", typeof(Material)) as Material;
		Material tundraMat = Resources.Load("Tundra", typeof(Material)) as Material;
		
		double temperature = (((elevation * -0.8 + 40) + (Math.Abs(latitude) * -.5 + 30) * 3) / 4);
		Debug.Log("x: " + (x/10).ToString() + "    temperature: " + temperature.ToString() + "    latitude: " + latitude.ToString());

		if (elevation <= 0){
			biome = Biome.Ocean;
			cube.GetComponent<Renderer>().material = oceanMat;
		} else if(elevation >= 50){
			biome = Biome.Mountain;
			cube.GetComponent<Renderer>().material = mountainMat;
		} else if(temperature <= 5){
			if(precipitation <= 100){
				biome = Biome.Tundra;
				cube.GetComponent<Renderer>().material = tundraMat;
			} else{
				biome = Biome.BorealForest;
				cube.GetComponent<Renderer>().material = borealMat;
			}
		} else if(temperature <= 20){
			if (precipitation <= 100){
				biome = Biome.Prairie;
				cube.GetComponent<Renderer>().material = prairieMat;
			} else if(precipitation <= 200){
				biome = Biome.Shrubland;
				cube.GetComponent<Renderer>().material = shrublandMat;
			} else {
				biome = Biome.TemperateForest;
				cube.GetComponent<Renderer>().material = temperateForestMat;
			}
		} else{
			if (precipitation <= 100) {
				biome = Biome.Desert;
				cube.GetComponent<Renderer>().material = desertMat;
			} else if(precipitation <= 200){
				biome = Biome.Savannah;
				cube.GetComponent<Renderer>().material = savanahMat;
			} else{
				biome = Biome.Rainforest;
				cube.GetComponent<Renderer>().material = rainforestMat;
			}
		}
		return 0;
	}		

}
