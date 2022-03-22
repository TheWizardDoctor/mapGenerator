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
    private Slider mountainSlider;
    public static float mountainMultiplier;

    [SerializeField]
    private Slider forestSlider;
    public static float forestMultiplier;

    [SerializeField]
    private Slider riverSlider;
    public static float riverMultiplier;

    // Start is called before the first frame update
    void Start()
    {
        citySlider.onValueChanged.AddListener(delegate { CityValueChanged(); });
        mountainSlider.onValueChanged.AddListener(delegate { MountainValueChanged(); });
        forestSlider.onValueChanged.AddListener(delegate { ForestValueChanged(); });
        riverSlider.onValueChanged.AddListener(delegate { RiverValueChanged(); });
    }

    private void CityValueChanged()
    {
        cityMultiplier = citySlider.value;
        Debug.Log("cityMult:" + cityMultiplier);
    }
    private void MountainValueChanged()
    {
        mountainMultiplier = mountainSlider.value;
        Debug.Log("mountainMult:" + mountainMultiplier);
    }
    private void ForestValueChanged()
    {
        forestMultiplier = forestSlider.value;
        Debug.Log("forestMult:" + forestMultiplier);
    }
    private void RiverValueChanged()
    {
        riverMultiplier = riverSlider.value;
        Debug.Log("riverMult:" + riverMultiplier);
    }
}
