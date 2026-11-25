
using System;
using UnityEngine;

public class Needs : MonoBehaviour
{
    // We define each need and how much it decreases over time 
    [Header("Necesidades (0-100)")]
    [Range(0, 100)] public double hunger = 100f;
    [Range(0, 100)] public double energy = 100f;
    [Range(0, 100)] public double hygiene = 100f;
    [Range(0, 100)] public double social = 100f;
    [Range(0, 100)] public double study = 0f;

    [Header("Reducción por segundo")]
    public double hungerDecay = 0.5f;
    public double energyDecay = 0.3f;
    public double hygieneDecay = 0.2f;
    public double socialDecay = 0.2f;

    // It multiplies how fast the needs go down (it is 2 while studying)
    public double decayMultiplier = 1f;

    // It serves to indicate that needs have changed 
    public event Action OnNeedsChanged;

    // With this function we update the needs as time goes by 
    void Update()
    {
        hunger = Math.Max(0, hunger - hungerDecay * decayMultiplier * Time.deltaTime);
        energy = Math.Max(0, energy - energyDecay * decayMultiplier * Time.deltaTime);
        hygiene = Math.Max(0, hygiene - hygieneDecay * decayMultiplier * Time.deltaTime);
        social = Math.Max(0, social - socialDecay * decayMultiplier * Time.deltaTime);

        OnNeedsChanged?.Invoke();
    }

    // When the character eats, showers, etc. We add up a certain amount 
    public void RestoreHunger(double amount)
    {
        hunger = Math.Clamp(hunger + amount, 0, 100);
        OnNeedsChanged?.Invoke();
    }

    public void RestoreEnergy(double amount)
    {
        energy = Math.Clamp(energy + amount, 0, 100);
        OnNeedsChanged?.Invoke();
    }

    public void RestoreHygiene(double amount)
    {
        hygiene = Math.Clamp(hygiene + amount, 0, 100);
        OnNeedsChanged?.Invoke();
    }

    public void RestoreSocial(double amount)
    {
        social = Math.Clamp(social + amount, 0, 100);
        OnNeedsChanged?.Invoke();
    }

    // When the character wins a minigame the study bar goes up
    public void AddStudy(double amount)
    {
        study = Math.Clamp(study + amount, 0, 100);
        OnNeedsChanged?.Invoke();
    }
}