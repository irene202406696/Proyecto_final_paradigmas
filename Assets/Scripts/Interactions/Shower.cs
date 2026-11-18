using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Shower : MonoBehaviour, IInteractable
{
    //define how long the shower lasts
    public double showerTime = 5f;

    //how white the screen gets while showering and how long the fade lasts
    public double fog = 0.7f;
    public double fadeTime = 0.5f;

    private bool isShowering = false;

    //elements of the shower screen
    private GameObject showerCanvas;
    private CanvasGroup showerGroup;
    private RectTransform progressFill;

    //InteractionController use it to know if you are showering
    public bool IsShowering
    {
        get { return isShowering; }
    }

    public string GetInteractionText()
    {
        //while showering we don't show this text, the shower screen already says "Duchándose..."
        if (isShowering)
        {
            return "";
        }
        else
        {
            return "Pulsa E para ducharte";
        }
    }

    public bool CanInteract()
    {
        if (isShowering)
        {
            return false;
        }
        return true;
    }

    public void Interact(Needs characterNeeds)
    {
        if (characterNeeds == null)
            return;

        //we start showering
        StartCoroutine(TakeShower(characterNeeds));
    }

    //the character showers during 5 seconds and the hygiene bar fills up little by little
    IEnumerator TakeShower(Needs characterNeeds)
    {
        isShowering = true;

        //create the shower screen if it doesn't exist yet and we show it
        if (showerCanvas == null)
        {
            CreateShowerUI();
        }
        showerCanvas.SetActive(true);

        //block the movement while showering
        PlayerController controller = characterNeeds.GetComponent<PlayerController>();
        if (controller != null)
        {
            controller.enabled = false;
        }

        //hygiene that we have to recover per second to reach 100 at the end of the shower
        double hygienePerSecond = (100f - characterNeeds.hygiene) / showerTime;
        double elapsed = 0f;

        while (elapsed < showerTime)
        {
            elapsed += Time.deltaTime;

            characterNeeds.RestoreHygiene((float)(hygienePerSecond * Time.deltaTime));

            //the progress bar grows with the time in the shower
            progressFill.anchorMax = new Vector2(Mathf.Clamp01((float)(elapsed / showerTime)), 1f);

            //the screen gets white at the beginning and normal again at the end
            double fadeIn = elapsed / fadeTime;
            double fadeOut = (showerTime - elapsed) / fadeTime;
            showerGroup.alpha = Mathf.Clamp01(Mathf.Min((float)fadeIn, (float)fadeOut));

            yield return null; //wait until the next frame
        }

        //at the end the hygiene is completely full
        characterNeeds.RestoreHygiene(100f);

        //the character finishes: we hide the screen and he can move again
        showerCanvas.SetActive(false);

        if (controller != null)
        {
            controller.enabled = true;
        }

        isShowering = false;
    }

    //create the white screen, the text and the progress bar
    void CreateShowerUI()
    {
        showerCanvas = new GameObject("ShowerCanvas");
        Canvas canvas = showerCanvas.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.sortingOrder = -1; //behind the needs bars, so we can see the hygiene going up
        showerCanvas.AddComponent<CanvasScaler>();
        showerGroup = showerCanvas.AddComponent<CanvasGroup>(); //it lets us change the transparency of everything at once
        showerGroup.alpha = 0f;

        //white screen
        Image steam = CreateImage("Fog", showerCanvas.transform, new Color(1f, 1f, 1f, (float)fog));
        steam.rectTransform.anchorMin = Vector2.zero;
        steam.rectTransform.anchorMax = Vector2.one;
        steam.rectTransform.offsetMin = Vector2.zero;
        steam.rectTransform.offsetMax = Vector2.zero;

        //text
        GameObject textObject = new GameObject("ShowerText", typeof(RectTransform));
        textObject.transform.SetParent(showerCanvas.transform, false);
        TextMeshProUGUI text = textObject.AddComponent<TextMeshProUGUI>();
        text.text = "Duchándose...";
        text.fontSize = 48;
        text.alignment = TextAlignmentOptions.Center;
        text.color = Color.white;
        text.rectTransform.sizeDelta = new Vector2(600f, 100f);
        text.rectTransform.anchoredPosition = new Vector2(0f, 40f);

        //progress bar
        Image barBackground = CreateImage("ProgressBar", showerCanvas.transform, new Color(1f, 1f, 1f, 0.25f));
        barBackground.rectTransform.sizeDelta = new Vector2(400f, 20f);
        barBackground.rectTransform.anchoredPosition = new Vector2(0f, -30f);

        //the fill of the bar starts empty and grows from left to right
        Image fill = CreateImage("Fill", barBackground.transform, new Color(0f, 0.8f, 1f, 1f));
        progressFill = fill.rectTransform;
        progressFill.anchorMin = Vector2.zero;
        progressFill.anchorMax = new Vector2(0f, 1f);
        progressFill.offsetMin = Vector2.zero;
        progressFill.offsetMax = Vector2.zero;

        showerCanvas.SetActive(false);
    }

    //auxiliary function to create a coloured image inside the canvas
    Image CreateImage(string name, Transform parent, Color color)
    {
        GameObject imageObject = new GameObject(name, typeof(RectTransform));
        imageObject.transform.SetParent(parent, false);
        Image image = imageObject.AddComponent<Image>();
        image.color = color;
        return image;
    }
}
