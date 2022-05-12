using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Country
{
    //static fields
    public static List<Country> countryList = new List<Country>();
    public static Country unclaimedLand = new Country("Unclaimed Land");
    public static List<string> countryNames;

    public List<Tile> tilesInCountry;
    public bool hasCapital;

    //instance fields
    public string name;
    public Country()
    {
        tilesInCountry = new List<Tile>();
        int num = RandomNum.r.Next(0, countryNames.Count);
        name = countryNames[num];
        countryNames.RemoveAt(num);
        countryList.Add(this);
    }
    public Country(string setName)
    {
        tilesInCountry = new List<Tile>();
        name = setName;
        countryList.Add(this);
    }

    public static void LoadCountryNames()
    {
        var cityNameFile = File.ReadAllLines("CountryNames.txt");
        countryNames = new List<string>(cityNameFile);
    }
}
