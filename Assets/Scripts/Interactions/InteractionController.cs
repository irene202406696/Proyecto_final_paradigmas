
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class InteractionController : MonoBehaviour
{
    public Camera playerCamera;
    public Needs playerNeeds;
    public float interactionDistance = 3f;

    public TextMeshProUGUI interactionText;

    private Coroutine messageCoroutine;
    private bool showingTemporaryMessage = false; //Indicates if we are showing a message 

    private float messageEndTime = 0f;

    void Start()
    {
        if (interactionText != null)
        {
            interactionText.text = ""; //We start without showing any message 
        }
            
    }

    void Update()
    {
        if (Time.time < messageEndTime)
            return;
        // If we are showing a message we stop 
        if (showingTemporaryMessage)
        {
            return;
        }

        // We start deleting the message, and then if we detect an interactive object we write it 
        if (interactionText != null)
        {
            interactionText.text = "";
        }

        // We create a ray to detect the interactive objects 
        Ray ray = new Ray(playerCamera.transform.position,playerCamera.transform.forward);
        
        // If the ray is collading with a collider 
        if (Physics.Raycast(ray, out RaycastHit hit,interactionDistance))
        {
            // We check if the collider is an interactive object (implements IInteractable)
            IInteractable interactable = hit.collider.GetComponentInParent<IInteractable>();

            if (interactable != null)
            {
                if (interactionText != null)
                    interactionText.text = interactable.GetInteractionText();

                if (Keyboard.current != null &&Keyboard.current.eKey.wasPressedThisFrame &&
                    interactable.CanInteract())
                {
                    interactable.Interact(playerNeeds);

                    // We check if the object that we are looking is the frisge 
                    Fridge fridge = hit.collider.GetComponentInParent<Fridge>();

                    if (fridge != null)
                    {
                        ShowTemporaryMessage("¡Has comido! Quedan " + fridge.currentFood + " raciones.");
                    }

                    Bed bed = hit.collider.GetComponentInParent<Bed>();

                    if (bed != null && !bed.IsSleeping)
                    {
                        ShowTemporaryMessage("No tienes sueño");
                    }
             
                }
            }
        }
    }

    void ShowTemporaryMessage(string message)
    {
        if (messageCoroutine != null)
            StopCoroutine(messageCoroutine);

        messageCoroutine =
            StartCoroutine(DisplayMessage(message));
    }

    IEnumerator DisplayMessage(string message)
    {
        showingTemporaryMessage = true;

        if (interactionText != null)
            interactionText.text = message;

        yield return new WaitForSeconds(2f);

        showingTemporaryMessage = false;
        messageCoroutine = null;
    }


}
