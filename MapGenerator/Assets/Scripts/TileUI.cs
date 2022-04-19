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
    private TextMeshProUGUI CityText;
    [SerializeField]
    private TextMeshProUGUI CityPopulationText;

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
        ElevationText.text = "Elevation: " + tile.Elevation.ToString("0.00");
        PrecipitationText.text = "Precipitation: " + tile.Precipitation.ToString("0.00"); ;

        if (tile.City != null)
        {
            CitySubMenu.SetActive(true);
            CityText.text = "City: " + tile.City.name;
            CityPopulationText.text = "Population: " + tile.City.population;
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
