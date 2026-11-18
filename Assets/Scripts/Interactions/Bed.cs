using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Bed : MonoBehaviour, IInteractable
{
    //define how fast the energy goes up and when the character can sleep
    public double energyPerSecond = 15f;
    public double extraSleepTime = 3f;
    public double maxEnergyToSleep = 80f;

    //how dark the screen gets while sleeping and how long the fade lasts
    public double darkness = 0.6f;
    public double fadeTime = 1f;

    private bool isSleeping = false;

    //elements of the sleep screen
    private GameObject sleepCanvas;
    private CanvasGroup sleepGroup;
    private RectTransform progressFill;

    //InteractionController use it to know if you are sleeping
    public bool IsSleeping
    {
        get { return isSleeping; }
    }

    //you can only sleep if the energy is low enough
    public bool IsTired(Needs characterNeeds)
    {
        if (characterNeeds.energy > maxEnergyToSleep)
        {
            return false;
        }
        return true;
    }

    public string GetInteractionText()
    {
        //while sleeping we don't show this text, the sleep screen already says "Durmiendo..."
        if (isSleeping)
        {
            return "";
        }
        else
        {
            return "Pulsa E para dormir";
        }
    }

    public bool CanInteract()
    {
        if (isSleeping)
        {
            return false;
        }
        return true;
    }

    public void Interact(Needs characterNeeds)
    {
        if (characterNeeds == null)
            return;

        //if the character is tired, we start sleeping
        if (IsTired(characterNeeds))
        {
            StartCoroutine(Sleep(characterNeeds));
        }
    }

    //the character sleeps until the energy bar is full and then waits 3 s
    IEnumerator Sleep(Needs characterNeeds)
    {
        isSleeping = true;

        //create the sleep screen if it doesn't exist yet and we show it
        if (sleepCanvas == null)
        {
            CreateSleepUI();
        }
        sleepCanvas.SetActive(true);

        //block the movement while sleeping
        PlayerController controller = characterNeeds.GetComponent<PlayerController>();
        if (controller != null)
        {
            controller.enabled = false;
        }

        //to calculate how long the character sleeps = time to fill the bar + 3 seconds
        double timeToFill = (100f - characterNeeds.energy) / energyPerSecond;
        double totalTime = timeToFill + extraSleepTime;
        double elapsed = 0f;

        while (elapsed < totalTime)
        {
            elapsed += Time.deltaTime;

            //first the energy goes up little by little, then it stays full during the extra seconds
            if (elapsed <= timeToFill)
            {
                characterNeeds.RestoreEnergy((float)(energyPerSecond * Time.deltaTime));  //with double it doesn't work
            }
            else
            {
                characterNeeds.RestoreEnergy(100f);
            }

            //the progress bar grows with the time slept
            progressFill.anchorMax = new Vector2(Mathf.Clamp01((float)(elapsed / totalTime)), 1f);

            //the screen gets dark at the beginning and light again at the end
            double fadeIn = elapsed / fadeTime;
            double fadeOut = (totalTime - elapsed) / fadeTime;
            sleepGroup.alpha = Mathf.Clamp01(Mathf.Min((float)fadeIn, (float)fadeOut));

            yield return null; //we wait until the next frame
        }

        //the character wakes up: we hide the screen and he can move again
        sleepCanvas.SetActive(false);

        if (controller != null)
        {
            controller.enabled = true;
        }

        isSleeping = false;
    }

    //we create the dark screen, the text and the progress bar
    void CreateSleepUI()
    {
        sleepCanvas = new GameObject("SleepCanvas");
        Canvas canvas = sleepCanvas.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.sortingOrder = -1; //behind the needs bars, so we can see the energy going up
        sleepCanvas.AddComponent<CanvasScaler>();
        sleepGroup = sleepCanvas.AddComponent<CanvasGroup>(); //it lets us change the transparency of everything at once
        sleepGroup.alpha = 0f;

        //dark screen
        Image dark = CreateImage("Dark", sleepCanvas.transform, new Color(0f, 0f, 0f, (float)darkness));
        dark.rectTransform.anchorMin = Vector2.zero;
        dark.rectTransform.anchorMax = Vector2.one;
        dark.rectTransform.offsetMin = Vector2.zero;
        dark.rectTransform.offsetMax = Vector2.zero;

        //text
        GameObject textObject = new GameObject("SleepText", typeof(RectTransform));
        textObject.transform.SetParent(sleepCanvas.transform, false);
        TextMeshProUGUI text = textObject.AddComponent<TextMeshProUGUI>();
        text.text = "Durmiendo...";
        text.fontSize = 48;
        text.alignment = TextAlignmentOptions.Center;
        text.color = Color.white;
        text.rectTransform.sizeDelta = new Vector2(600f, 100f);
        text.rectTransform.anchoredPosition = new Vector2(0f, 40f);

        //progress bar
        Image barBackground = CreateImage("ProgressBar", sleepCanvas.transform, new Color(1f, 1f, 1f, 0.25f));
        barBackground.rectTransform.sizeDelta = new Vector2(400f, 20f);
        barBackground.rectTransform.anchoredPosition = new Vector2(0f, -30f);

        //the fill of the bar starts empty and grows from left to right
        Image fill = CreateImage("Fill", barBackground.transform, new Color(0.85f, 0.75f, 0.3f, 1f));
        progressFill = fill.rectTransform;
        progressFill.anchorMin = Vector2.zero;
        progressFill.anchorMax = new Vector2(0f, 1f);
        progressFill.offsetMin = Vector2.zero;
        progressFill.offsetMax = Vector2.zero;

        sleepCanvas.SetActive(false);
    }

    //create a coloured image inside the canvas
    Image CreateImage(string name, Transform parent, Color color)
    {
        GameObject imageObject = new GameObject(name, typeof(RectTransform));
        imageObject.transform.SetParent(parent, false);
        Image image = imageObject.AddComponent<Image>();
        image.color = color;
        return image;
    }
}
