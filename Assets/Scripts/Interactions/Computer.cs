using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;
using TMPro;

public class Computer : MonoBehaviour, IInteractable
{
    //minimum of all the needs to start studying and to keep studying
    public double minNeedsToStart = 60f;
    public double minNeedsToKeepStudying = 20f;

    //how much the study bar goes up with each minigame won, how much it goes down with each one lost and how fast the needs go down while studying
    public double studyPerGame = 5f;
    public double studyLostPerGame = 1f;
    public double decayWhileStudying = 2f;

    private bool isStudying = false;

    //elements of the study screen
    private GameObject studyCanvas;
    private Transform screen;
    private TextMeshProUGUI titleText;
    private TextMeshProUGUI mainText;
    private RectTransform timerFill;

    //InteractionController use it to know if you are studying
    public bool IsStudying
    {
        get { return isStudying; }
    }

    //check if all the needs are at least the minimum
    public bool HasMinimum(Needs characterNeeds, double minimum)
    {
        if (characterNeeds.hunger < minimum || characterNeeds.energy < minimum ||
            characterNeeds.hygiene < minimum || characterNeeds.social < minimum)
        {
            return false;
        }
        return true;
    }

    public string GetInteractionText()
    {
        //while studying we don't show this text, the study screen is already open
        if (isStudying)
        {
            return "";
        }
        else
        {
            return "Pulsa E para estudiar";
        }
    }

    public bool CanInteract()
    {
        if (isStudying)
        {
            return false;
        }
        return true;
    }

    public void Interact(Needs characterNeeds)
    {
        if (characterNeeds == null)
            return;

        //you can only start studying if all the needs are high enough
        if (HasMinimum(characterNeeds, minNeedsToStart))
        {
            StartCoroutine(Study(characterNeeds));
        }
    }

    //choose one of the minigames randomly
    Minigame CreateRandomGame()
    {
        int number = Random.Range(0, 6);

        if (number == 0)
        {
            return new ArrowsGame();
        }
        else if (number == 1)
        {
            return new OddColorGame();
        }
        else if (number == 2)
        {
            return new GreenZoneGame();
        }
        else if (number == 3)
        {
            return new CirclesGame();
        }
        else if (number == 4)
        {
            return new MemoryGame();
        }
        else
        {
            return new OddWordGame();
        }
    }

    //the character plays minigames until he leaves, the needs are too low or the study bar is full
    IEnumerator Study(Needs characterNeeds)
    {
        isStudying = true;

        //create the study screen if it doesn't exist yet and we show it
        if (studyCanvas == null)
        {
            CreateStudyUI();
        }
        studyCanvas.SetActive(true);
        mainText.text = "";

        //block the movement and show the mouse to click the buttons
        PlayerController controller = characterNeeds.GetComponent<PlayerController>();
        if (controller != null)
        {
            controller.enabled = false;
        }
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        //the needs go down faster while studying
        characterNeeds.decayMultiplier = decayWhileStudying;

        bool keepStudying = true;

        while (keepStudying)
        {
            Minigame minigame = CreateRandomGame();
            minigame.StartGame(screen);
            double elapsed = 0f;

            //we wait until the minigame is won or lost
            while (minigame.result == 0)
            {
                elapsed += Time.deltaTime;

                //with Escape you stop studying
                if (Keyboard.current != null && Keyboard.current.escapeKey.wasPressedThisFrame)
                {
                    keepStudying = false;
                    break;
                }

                //if a need is too low you have to stop studying
                if (!HasMinimum(characterNeeds, minNeedsToKeepStudying))
                {
                    minigame.EndGame();
                    titleText.text = "¡Tienes que ocuparte de tus necesidades!";
                    yield return new WaitForSeconds(2f);
                    keepStudying = false;
                    break;
                }

                minigame.UpdateGame();
                titleText.text = minigame.title;

                //if the time is over you lose the minigame
                if (elapsed >= minigame.timeLimit)
                {
                    minigame.result = -1;
                }

                //the timer bar gets smaller with the time
                timerFill.anchorMax = new Vector2(Mathf.Clamp01(1f - (float)(elapsed / minigame.timeLimit)), 1f);

                yield return null; //we wait until the next frame
            }

            //we delete the elements of the minigame
            minigame.EndGame();

            if (!keepStudying)
                break;

            if (minigame.result == 1)
            {
                characterNeeds.AddStudy(studyPerGame);
                titleText.text = "¡Bien! +" + studyPerGame;
            }
            else
            {
                characterNeeds.AddStudy(-studyLostPerGame);
                titleText.text = "¡Fallado! -" + studyLostPerGame;
            }

            yield return new WaitForSeconds(1f);

            //when the study bar is full you win and the game stops
            if (characterNeeds.study >= 100f)
            {
                titleText.text = "¡Has aprobado!";
                mainText.text = "Has llegado al 100% de estudio";
                Time.timeScale = 0f;
                yield break;
            }
        }

        //the character stops studying: we hide the screen and he can move again
        studyCanvas.SetActive(false);

        if (controller != null)
        {
            controller.enabled = true;
        }
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        characterNeeds.decayMultiplier = 1f;
        isStudying = false;
    }

    //create the computer screen, the texts and the timer bar
    void CreateStudyUI()
    {
        //the buttons need an EventSystem to work
        if (FindFirstObjectByType<EventSystem>() == null)
        {
            GameObject eventSystem = new GameObject("EventSystem");
            eventSystem.AddComponent<EventSystem>();
            eventSystem.AddComponent<InputSystemUIInputModule>();
        }

        studyCanvas = new GameObject("StudyCanvas");
        Canvas canvas = studyCanvas.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.sortingOrder = 10; //in front of everything, so we can click the buttons
        studyCanvas.AddComponent<CanvasScaler>();
        studyCanvas.AddComponent<GraphicRaycaster>(); //it lets us click the buttons

        //dark screen behind the computer
        Image dark = CreateImage("Dark", studyCanvas.transform, new Color(0f, 0f, 0f, 0.5f));
        dark.rectTransform.anchorMin = Vector2.zero;
        dark.rectTransform.anchorMax = Vector2.one;
        dark.rectTransform.offsetMin = Vector2.zero;
        dark.rectTransform.offsetMax = Vector2.zero;

        //computer screen, the minigames are created inside it
        Image screenImage = CreateImage("Screen", studyCanvas.transform, new Color(0.1f, 0.15f, 0.3f, 1f));
        screenImage.rectTransform.sizeDelta = new Vector2(700f, 450f);
        screen = screenImage.transform;

        //texts
        titleText = CreateText("Title", screen, "", 36, new Vector2(0f, 160f));
        mainText = CreateText("Main", screen, "", 40, new Vector2(0f, 40f));
        CreateText("Info", screen, "Escape para dejar de estudiar", 20, new Vector2(0f, -205f));

        //timer bar
        Image barBackground = CreateImage("TimerBar", screen, new Color(1f, 1f, 1f, 0.25f));
        barBackground.rectTransform.sizeDelta = new Vector2(600f, 15f);
        barBackground.rectTransform.anchoredPosition = new Vector2(0f, -170f);

        //the fill of the bar starts full and gets smaller from right to left
        Image fill = CreateImage("Fill", barBackground.transform, new Color(0.4f, 0.9f, 0.4f, 1f));
        timerFill = fill.rectTransform;
        timerFill.anchorMin = Vector2.zero;
        timerFill.anchorMax = Vector2.one;
        timerFill.offsetMin = Vector2.zero;
        timerFill.offsetMax = Vector2.zero;

        studyCanvas.SetActive(false);
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

    //create a white text inside the canvas
    TextMeshProUGUI CreateText(string name, Transform parent, string content, double size, Vector2 position)
    {
        GameObject textObject = new GameObject(name, typeof(RectTransform));
        textObject.transform.SetParent(parent, false);
        TextMeshProUGUI text = textObject.AddComponent<TextMeshProUGUI>();
        text.text = content;
        text.fontSize = (float)size;
        text.alignment = TextAlignmentOptions.Center;
        text.color = Color.white;
        text.rectTransform.sizeDelta = new Vector2(680f, 100f);
        text.rectTransform.anchoredPosition = position;
        return text;
    }
}
