using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TileUI : MonoBehaviour
{
    public static TileUI S;

    [SerializeField]
    private GameObject menu;
    [SerializeField]
    private GameObject CitySubMenu;
    [SerializeField]
    private TextMeshProUGUI biomeText;
    [SerializeField]
    private TextMeshProUGUI ElevationText;
    [SerializeField]
    private TextMeshProUGUI PrecipitationText;
    [SerializeField]
    private TextMeshProUGUI CountryText;
    [SerializeField]
    private TextMeshProUGUI CityText;
    [SerializeField]
    private TextMeshProUGUI CityPopulationText;
    [SerializeField]
    private TextMeshProUGUI CityWealthText;

    private void Start()
    {
        if(S==null)
        {
            S = this;
        }
    }

    public void SetTileMenu(Vector2 tilePos)
    {
        menu.SetActive(true);

        Tile tile = Map.tiles[Mathf.RoundToInt(tilePos.x), Mathf.RoundToInt(tilePos.y)];

        biomeText.text = "Biome: " + tile.Biome;
        if(tile.Biome==Biome.Ocean)
        {
            ElevationText.text = "Elevation: 0 meters";
            PrecipitationText.text = "Precipitation: N/A";
            CountryText.text = "Country: N/A";
        }
        else
        {
            ElevationText.text = "Elevation: " + (100 * tile.Elevation).ToString("0.00 meters");
            PrecipitationText.text = "Precipitation: " + tile.Precipitation.ToString("0.00 in/a");
            CountryText.text = "Country: " + tile.country.name;
        }
        


        if (tile.City != null)
        {
            CitySubMenu.SetActive(true);
            if(tile.City.Capital)
            {
                CityText.text = "Capital City: " + tile.City.Name;
            }
            else
            {
                CityText.text = "City: " + tile.City.Name;
            }
            CityPopulationText.text = "Population: " + String.Format("{0:n0}", tile.City.Population);
            CityWealthText.text = "Money: " + String.Format("{0:C0}", tile.City.Wealth);
        }
        else
        {
            CitySubMenu.SetActive(false);
        }
        

    }
    public void Disable()
    {
        menu.SetActive(false);
    }
}
