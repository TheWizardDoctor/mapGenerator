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
    public Tile up;
    public Tile down;
    public Tile left;
    public Tile right;

    //constructor
    public Tile()
    {
        biome = Biome.Ocean;
        elevation = 0;
        precipitation = 0;
        city = false;
        road = false;
    }
    public Tile(Biome b, float e, float p)
    {
        biome = b;
        elevation = e;
        precipitation = p;
        city = false;
        road = false;
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
    public Biome Biome
    {
        get { return biome; }
        set { biome=value; }
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

    //methods

}
