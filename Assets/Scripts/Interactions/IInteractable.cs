using UnityEngine;

public interface IInteractable
{
    string GetInteractionText();

    bool CanInteract();

    void Interact(Needs characterNeeds);
}