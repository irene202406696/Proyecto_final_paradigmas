
using UnityEngine;
using UnityEngine.UI;

public class NeedsUI : MonoBehaviour
{
    // We define the values of the sliders 
    public Needs playerNeeds;

    public Slider hungerSlider;
    public Slider energySlider;
    public Slider hygieneSlider;
    public Slider socialSlider;

    void OnEnable()
    {
        if (playerNeeds != null)
            playerNeeds.OnNeedsChanged += UpdateUI;
    }

    void Start()
    {
        UpdateUI();
    }

    void OnDisable()
    {
        if (playerNeeds != null)
            playerNeeds.OnNeedsChanged -= UpdateUI;
    }


    void UpdateUI()
    {
        if (playerNeeds == null) return;

        hungerSlider.value = playerNeeds.hunger;
        energySlider.value = playerNeeds.energy;
        hygieneSlider.value = playerNeeds.hygiene;
        socialSlider.value = playerNeeds.social;
    }
}
