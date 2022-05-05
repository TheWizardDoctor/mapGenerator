using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIData : MonoBehaviour
{
    [SerializeField]
    private Slider citySlider;
    public static float cityMultiplier=0.5f;

    [SerializeField]
    private Slider riverSlider;
    public static float riverMultiplier = 0.5f;

    [SerializeField]
    private Slider countrySlider;
    public static float countryMultiplier = 0.5f;

    [SerializeField]
    private Slider wealthSlider;
    public static float wealthMultiplier = 0.5f;

    [SerializeField]
    private Slider populationSlider;
    public static float populationMultiplier = 0.5f;


    //Scroll View Content
    [SerializeField]
    private Slider oceanSlider;
    public static float oceanMultiplier = 0.5f;

    [SerializeField]
    private Slider mountainSlider;
    public static float mountainMultiplier = 0.5f;

    [SerializeField]
    private Slider tundraSlider;
    public static float tundraMultiplier = 0.5f;

    [SerializeField]
    private Slider borealForestSlider;
    public static float borealForestMultiplier = 0.5f;

    [SerializeField]
    private Slider prairieSlider;
    public static float prairieMultiplier = 0.5f;

    [SerializeField]
    private Slider shrublandSlider;
    public static float shrublandMultiplier = 0.5f;

    [SerializeField]
    private Slider temperateForestSlider;
    public static float temperateForestMultiplier = 0.5f;

    [SerializeField]
    private Slider desertSlider;
    public static float desertMultiplier = 0.5f;

    [SerializeField]
    private Slider savannahSlider;
    public static float savannahMultiplier = 0.5f;

    [SerializeField]
    private Slider rainForestSlider;
    public static float rainForestMultiplier = 0.5f;


    // Start is called before the first frame update
    void Start()
    {
        citySlider.onValueChanged.AddListener(delegate { CityValueChanged(); });
        wealthSlider.onValueChanged.AddListener(delegate { WealthValueChanged(); });
        countrySlider.onValueChanged.AddListener(delegate { CountryValueChanged(); });
        riverSlider.onValueChanged.AddListener(delegate { RiverValueChanged(); });
        populationSlider.onValueChanged.AddListener(delegate { PopulationValueChanged(); });

        //scroll view content
        oceanSlider.onValueChanged.AddListener(delegate { OceanValueChanged(); });
        mountainSlider.onValueChanged.AddListener(delegate { MountainValueChanged(); });
        tundraSlider.onValueChanged.AddListener(delegate { TundraValueChanged(); });
        borealForestSlider.onValueChanged.AddListener(delegate { BorealForestValueChanged(); });
        prairieSlider.onValueChanged.AddListener(delegate { PrairieValueChanged(); });
        shrublandSlider.onValueChanged.AddListener(delegate { ShrublandValueChanged(); });
        temperateForestSlider.onValueChanged.AddListener(delegate { TemperateForestValueChanged(); });
        desertSlider.onValueChanged.AddListener(delegate { DesertValueChanged(); });
        savannahSlider.onValueChanged.AddListener(delegate { SavannahValueChanged(); });
        rainForestSlider.onValueChanged.AddListener(delegate { RainForestValueChanged(); });
    }

    private void CityValueChanged()
    {
        cityMultiplier = citySlider.value;
    }
    private void RiverValueChanged()
    {
        riverMultiplier = riverSlider.value;
    }
    private void CountryValueChanged()
    {
        countryMultiplier = countrySlider.value;
    }
    private void WealthValueChanged()
    {
        wealthMultiplier = wealthSlider.value;
    }
    private void PopulationValueChanged()
    {
        populationMultiplier = populationSlider.value;
    }

    //scroll view content
    private void OceanValueChanged()
    {
        oceanMultiplier = oceanSlider.value;
    }
    private void MountainValueChanged()
    {
        mountainMultiplier = mountainSlider.value;
    }
    private void TundraValueChanged()
    {
        tundraMultiplier = tundraSlider.value;
    }
    private void BorealForestValueChanged()
    {
        borealForestMultiplier = borealForestSlider.value;
    }
    private void PrairieValueChanged()
    {
        prairieMultiplier = prairieSlider.value;
    }
    private void ShrublandValueChanged()
    {
        shrublandMultiplier = shrublandSlider.value;
    }
    private void TemperateForestValueChanged()
    {
        temperateForestMultiplier = temperateForestSlider.value;
    }
    private void DesertValueChanged()
    {
        desertMultiplier = desertSlider.value;
    }
    private void SavannahValueChanged()
    {
        savannahMultiplier = savannahSlider.value;
    }
    private void RainForestValueChanged()
    {
        rainForestMultiplier = rainForestSlider.value;
    }
}
