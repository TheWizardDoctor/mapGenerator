using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Biome { Ocean, Mountain, Tundra, BorealForest, Prairie, Shrubland, TemperateForest, Desert, Savannah, Rainforest };

public class Tile : IComparable<Tile>
{
    //attributes
    private Biome biome;
    private float elevation;
    private float precipitation;
    private bool city;
    private bool road;
	private int x;
	private int y;
	private float gVal;
	private float hVal;
	private float latitude;
	private bool explored;
	public Tile previous = null;
    public Tile up = null;
    public Tile down = null;
    public Tile left = null;
    public Tile right = null;
    private float border = 0;
    private float navDifficulty = 0;
	
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
    public Tile(int height, int xCord, int yCord, Transform tileSet)
    {
        elevation = 0;
        precipitation = 0;
        city = false;
        road = false;
		explored = false;
		x = xCord;
		y = yCord;
		latitude = ((xCord + 1) * 90/(height/2)) - 90;
		cube.transform.SetParent(tileSet);
		cube.transform.localScale = new Vector3(1, elevation/10 + 1, 1);
		cube.transform.position = new Vector3(x, (elevation/10 + 1)/2, y);
    }

    //properties
    public float Elevation
    {
        get { return elevation; }
        set { elevation = value; 
			cube.transform.localScale = new Vector3(1, elevation/10 + 1, 1);
			cube.transform.position = new Vector3(x, (elevation/10 + 1)/2, y);
		}
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
	//public float FVal
	//{
	//	get { return fVal; }
	//	set { fVal = value; }
	//}
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
    public float Border
    {
        get { return border; }
        set { border = value; }
    }
    public float NavigationDifficulty
    {
        get { return navDifficulty; }
        set { navDifficulty = value; }
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
		
		double temperature = (((elevation * -0.8 + 40) * 2 + (Math.Abs(latitude) * -.65 + 30) * 3) / 5);
		//Debug.Log("x: " + (x).ToString() + "    temperature: " + temperature.ToString() + "    latitude: " + latitude.ToString());

		if (elevation <= 0){
			biome = Biome.Ocean;
			cube.GetComponent<Renderer>().material = oceanMat;
        } else if(elevation >= 50){
			biome = Biome.Mountain;
			cube.GetComponent<Renderer>().material = mountainMat;
            navDifficulty = 9;
		} else if(temperature <= 5){
			if(precipitation < 100){
				biome = Biome.Tundra;
				cube.GetComponent<Renderer>().material = tundraMat;
                navDifficulty = 7;
            }
            else
            {
				biome = Biome.BorealForest;
				cube.GetComponent<Renderer>().material = borealMat;
                navDifficulty = 5;
            }
        } else if(temperature <= 20){
			if (precipitation < 100){
				biome = Biome.Prairie;
				cube.GetComponent<Renderer>().material = prairieMat;
                navDifficulty = 1;
            }
            else if(precipitation < 200){
				biome = Biome.Shrubland;
				cube.GetComponent<Renderer>().material = shrublandMat;
                navDifficulty = 2;
            }
            else {
				biome = Biome.TemperateForest;
				cube.GetComponent<Renderer>().material = temperateForestMat;
                navDifficulty = 3;
            }
        } else{
			if (precipitation < 100) {
				biome = Biome.Desert;
				cube.GetComponent<Renderer>().material = desertMat;
                navDifficulty = 6;
            }
            else if(precipitation < 200){
				biome = Biome.Savannah;
				cube.GetComponent<Renderer>().material = savanahMat;
                navDifficulty = 4;
            }
            else
            {
				biome = Biome.Rainforest;
				cube.GetComponent<Renderer>().material = rainforestMat;
                navDifficulty = 8;
            }
        }
		return 0;
	}

    public int CompareTo(Tile other)
    {
		return GVal.CompareTo(other.GVal);
    }
}
