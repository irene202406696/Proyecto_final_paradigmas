
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
    public Slider studySlider;


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

        hungerSlider.value = (float)playerNeeds.hunger;
        energySlider.value = (float)playerNeeds.energy;
        hygieneSlider.value = (float)playerNeeds.hygiene;
        socialSlider.value = (float)playerNeeds.social;
        studySlider.value = (float)playerNeeds.study;
    }
}
