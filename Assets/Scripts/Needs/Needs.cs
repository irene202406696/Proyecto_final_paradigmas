
using System;
using UnityEngine;

public class Needs : MonoBehaviour
{
    [Header("Necesidades (0-100)")]
    [Range(0, 100)] public float hunger = 100f;
    [Range(0, 100)] public float energy = 100f;
    [Range(0, 100)] public float hygiene = 100f;
    [Range(0, 100)] public float fun = 100f;
    [Range(0, 100)] public float social = 100f;

    [Header("Reducción por segundo")]
    public float hungerDecay = 0.5f;
    public float energyDecay = 0.3f;
    public float hygieneDecay = 0.2f;
    public float funDecay = 0.4f;
    public float socialDecay = 0.2f;

    public event Action OnNeedsChanged;

    void Update()
    {
        hunger = Mathf.Max(0, hunger - hungerDecay * Time.deltaTime);
        energy = Mathf.Max(0, energy - energyDecay * Time.deltaTime);
        hygiene = Mathf.Max(0, hygiene - hygieneDecay * Time.deltaTime);
        fun = Mathf.Max(0, fun - funDecay * Time.deltaTime);
        social = Mathf.Max(0, social - socialDecay * Time.deltaTime);

        OnNeedsChanged?.Invoke();
    }

    public void RestoreHunger(float amount)
    {
        hunger = Mathf.Clamp(hunger + amount, 0, 100);
        OnNeedsChanged?.Invoke();
    }

    public void RestoreEnergy(float amount)
    {
        energy = Mathf.Clamp(energy + amount, 0, 100);
        OnNeedsChanged?.Invoke();
    }

    public void RestoreHygiene(float amount)
    {
        hygiene = Mathf.Clamp(hygiene + amount, 0, 100);
        OnNeedsChanged?.Invoke();
    }

    public void RestoreFun(float amount)
    {
        fun = Mathf.Clamp(fun + amount, 0, 100);
        OnNeedsChanged?.Invoke();
    }

    public void RestoreSocial(float amount)
    {
        social = Mathf.Clamp(social + amount, 0, 100);
        OnNeedsChanged?.Invoke();
    }
}
