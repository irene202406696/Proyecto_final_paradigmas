using UnityEngine;
using UnityEngine.UI;
using TMPro;

//base class of all the minigames: each minigame creates its elements and decides if you win or lose
public abstract class Minigame
{
    //state of the minigame: 0 playing, 1 won, -1 lost
    public int result = 0;

    //text that explains what to do and time to finish the minigame
    public string title = "";
    public double timeLimit = 4f;

    //colour of the normal buttons
    protected Color buttonColor = new Color(0.25f, 0.55f, 0.95f, 1f);

    //all the elements of the minigame are created inside this object, so we can delete them at the end
    protected GameObject root;

    //each minigame creates its own elements
    protected abstract void Create();

    //it is called every frame while playing (only some minigames need it)
    public virtual void UpdateGame()
    {
    }

    //create the minigame inside the computer screen
    public void StartGame(Transform screen)
    {
        result = 0;

        root = new GameObject("Minigame", typeof(RectTransform));
        root.transform.SetParent(screen, false);
        RectTransform rootRect = root.GetComponent<RectTransform>();
        rootRect.anchorMin = Vector2.zero;
        rootRect.anchorMax = Vector2.one;
        rootRect.offsetMin = Vector2.zero;
        rootRect.offsetMax = Vector2.zero;

        Create();
    }

    //delete all the elements of the minigame
    public void EndGame()
    {
        if (root != null)
        {
            GameObject.Destroy(root);
            root = null;
        }
    }

    protected void Win()
    {
        if (result == 0)
        {
            result = 1;
        }
    }

    protected void Lose()
    {
        if (result == 0)
        {
            result = -1;
        }
    }

    //create a coloured image inside the minigame
    protected Image CreateImage(string name, Vector2 size, Vector2 position, Color color)
    {
        GameObject imageObject = new GameObject(name, typeof(RectTransform));
        imageObject.transform.SetParent(root.transform, false);
        Image image = imageObject.AddComponent<Image>();
        image.color = color;
        image.rectTransform.sizeDelta = size;
        image.rectTransform.anchoredPosition = position;
        return image;
    }

    //create a white text inside the minigame
    protected TextMeshProUGUI CreateText(string content, float fontSize, Vector2 position)
    {
        GameObject textObject = new GameObject("Text", typeof(RectTransform));
        textObject.transform.SetParent(root.transform, false);
        TextMeshProUGUI text = textObject.AddComponent<TextMeshProUGUI>();
        text.text = content;
        text.fontSize = fontSize;
        text.alignment = TextAlignmentOptions.Center;
        text.color = Color.white;
        text.rectTransform.sizeDelta = new Vector2(680f, 100f);
        text.rectTransform.anchoredPosition = position;
        return text;
    }

    //create a button with a text inside (if the label is empty, it has no text)
    protected Button CreateButton(string label, Vector2 size, Vector2 position, Color color)
    {
        Image image = CreateImage("Button", size, position, color);
        Button button = image.gameObject.AddComponent<Button>();

        if (label != "")
        {
            GameObject textObject = new GameObject("ButtonText", typeof(RectTransform));
            textObject.transform.SetParent(image.transform, false);
            TextMeshProUGUI text = textObject.AddComponent<TextMeshProUGUI>();
            text.text = label;
            text.fontSize = 30;
            text.alignment = TextAlignmentOptions.Center;
            text.color = Color.white;
            text.rectTransform.anchorMin = Vector2.zero;
            text.rectTransform.anchorMax = Vector2.one;
            text.rectTransform.offsetMin = Vector2.zero;
            text.rectTransform.offsetMax = Vector2.zero;
        }

        return button;
    }
}
