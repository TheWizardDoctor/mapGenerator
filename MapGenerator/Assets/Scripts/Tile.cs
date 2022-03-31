using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Biome { Ocean, Mountain, Tundra, BorealForest, Prairie, Shrubland, TemperateForest, Desert, Savannah, Rainforest };

public class Tile : IComparable<Tile>
{
    //attributes
	//elavation is in 100m scale aka 60 = 6000m
    private Biome biome;
    private float elevation;
    private float precipitation;
    private bool city;
    private bool road;
	private int x;
	private int y;
	private float fVal;
	private float gVal;
	private float hVal;
	private float latitude;
	private bool explored;
	public Tile previous = null;
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
		explored = false;
    }
    public Tile(int xCord, int yCord, Transform tileSet)
    {
        elevation = -1;
        precipitation = 0;
        city = false;
        road = false;
		biome = Biome.Ocean;
		explored = false;
		x = xCord;
		y = yCord;
		latitude = ((yCord + 1) * 90/(Map.height/2)) - 90;
		cube.transform.SetParent(tileSet);
		cube.transform.localScale = new Vector3(1, elevation/10 + 1, 1);
		cube.transform.position = new Vector3(x, (elevation/10 + 1)/2, y);
		cube.gameObject.GetComponent<MeshRenderer>().receiveShadows = false;
    }

    //properties
    public float Elevation
    {
        get { return elevation; }
        set { elevation = value; }
    }
    public float Precipitation
    {
        get { return precipitation; }
        set { precipitation = value; }
    }
	public float GVal
	{
		get { return gVal; }
		set { gVal = value; }
	}
	public float HVal
	{
		get { return hVal; }
		set { hVal = value; }
	}
	public float FVal
	{
		get { return fVal; }
		set { fVal = value; }
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
        set { road = value; }
    }
	public bool Explored
	{
		get { return explored; }
		set { explored = value; }
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
		
		float l = Math.Abs(latitude);
		double temperature = (((elevation * -0.8 + 40) + (30 - l*1.7 + 0.059*Math.Pow(l, 2) - 0.0007*Math.Pow(l, 3)) * 3) / 4);
		cube.transform.localScale = new Vector3(1, elevation/10 + 1, 1);
		cube.transform.position = new Vector3(x, (elevation/10 + 1)/2, y);
		
		if (elevation <= 3){
			biome = Biome.Ocean;
			cube.GetComponent<Renderer>().material = oceanMat;
		} else if(elevation >= 50){
			biome = Biome.Mountain;
			cube.GetComponent<Renderer>().material = mountainMat;
		} else if(temperature <= 0){
			if(precipitation < 100){
				biome = Biome.Tundra;
				cube.GetComponent<Renderer>().material = tundraMat;
			} else{
				biome = Biome.BorealForest;
				cube.GetComponent<Renderer>().material = borealMat;
			}
		} else if(temperature <= 20){
			if (precipitation < 100){
				biome = Biome.Prairie;
				cube.GetComponent<Renderer>().material = prairieMat;
			} else if(precipitation < 200){
				biome = Biome.Shrubland;
				cube.GetComponent<Renderer>().material = shrublandMat;
			} else {
				biome = Biome.TemperateForest;
				cube.GetComponent<Renderer>().material = temperateForestMat;
			}
		} else{
			if (precipitation < 100) {
				biome = Biome.Desert;
				cube.GetComponent<Renderer>().material = desertMat;
			} else if(precipitation < 200){
				biome = Biome.Savannah;
				cube.GetComponent<Renderer>().material = savanahMat;
			} else{
				biome = Biome.Rainforest;
				cube.GetComponent<Renderer>().material = rainforestMat;
			}
		}
		return 0;
	}

    public int CompareTo(Tile other)
	{
		return fVal.CompareTo(other.fVal);
    }
}
