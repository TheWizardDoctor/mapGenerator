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
    public Tile up;
    public Tile down;
    public Tile left;
    public Tile right;
	
	GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
	
    //constructor
    public Tile()
    {
        biome = Biome.Ocean;
        elevation = 0;
        precipitation = 0;
        city = false;
        road = false;
    }
    public Tile(Biome b, int xCord, int yCord)
    {
        biome = b;
        elevation = 0;
        precipitation = 0;
        city = false;
        road = false;
		x = xCord;
		y = yCord;
		cube.transform.position = new Vector3(x, elevation, y);
    }

    //properties
    public float Elevation
    {
        get { return elevation; }
        set { elevation = value; 
			cube.transform.position = new Vector3(x, elevation, y);
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
			cube.transform.position = new Vector3(x, elevation, y);
		}
    }
	public int Y
    {
        get { return y; }
        set { y = value; 
			cube.transform.position = new Vector3(x, elevation, y);
		}
    }
    //methods
	
	
	

}
