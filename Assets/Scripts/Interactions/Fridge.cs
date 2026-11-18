using System.Reflection.Metadata.Ecma335;
using UnityEngine;

public class Fridge : MonoBehaviour, IInteractable
{
    [Header("Comida disponible")]
    public int maxFood = 5;
    public int currentFood;

    public float hungerRestored = 40f;

    void Awake()
    {
        currentFood = maxFood;
    }

    public bool HasFood()
    {
        if (currentFood == 0)
        {
            return false; 
        }
        return true;
    }

    public bool TakeFood()
    {
        if (!HasFood())
        {
            return false;
        }
        currentFood -= 1;
        return true; 
    }

    public void Restock()
    {
        currentFood = maxFood;
    }

    public string GetInteractionText()
    {
        if (!HasFood())
        {
            return "Nevera vacía"; 
        }
        else
        {
            return "Pulsa E para comer"; 
        }
    }

    public bool CanInteract()
    {
        return HasFood(); 
    }

    public void Interact(Needs characterNeeds)
    {
        if (characterNeeds == null)
            return;

        if (TakeFood())
        {
            characterNeeds.RestoreHunger(hungerRestored);

            Debug.Log("¡Has comido! Quedan " + currentFood + " raciones.");
        }

    }
}
