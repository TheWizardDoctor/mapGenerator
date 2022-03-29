using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIData : MonoBehaviour
{
    [SerializeField]
    private Slider citySlider;
    public static float cityMultiplier;

    [SerializeField]
    private Slider riverSlider;
    public static float riverMultiplier;

    [SerializeField]
    private Slider countrySlider;
    public static float countryMultiplier;

    [SerializeField]
    private Slider wealthSlider;
    public static float wealthMultiplier;


    //Scroll View Content
    [SerializeField]
    private Slider oceanSlider;
    public static float oceanMultiplier;

    [SerializeField]
    private Slider mountainSlider;
    public static float mountainMultiplier;

    [SerializeField]
    private Slider tundraSlider;
    public static float tundraMultiplier;

    [SerializeField]
    private Slider borealForestSlider;
    public static float borealForestMultiplier;

    [SerializeField]
    private Slider prairieSlider;
    public static float prairieMultiplier;

    [SerializeField]
    private Slider shrublandSlider;
    public static float shrublandMultiplier;

    [SerializeField]
    private Slider temperateForestSlider;
    public static float temperateForestMultiplier;

    [SerializeField]
    private Slider desertSlider;
    public static float desertMultiplier;

    [SerializeField]
    private Slider savannahSlider;
    public static float savannahMultiplier;

    [SerializeField]
    private Slider rainForestSlider;
    public static float rainForestMultiplier;


    // Start is called before the first frame update
    void Start()
    {
        citySlider.onValueChanged.AddListener(delegate { CityValueChanged(); });
        wealthSlider.onValueChanged.AddListener(delegate { WealthValueChanged(); });
        countrySlider.onValueChanged.AddListener(delegate { CountryValueChanged(); });
        riverSlider.onValueChanged.AddListener(delegate { RiverValueChanged(); });

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
