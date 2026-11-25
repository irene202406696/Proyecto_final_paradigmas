using UnityEngine;
using UnityEngine.UI;
using TMPro;

//click 5 circles that appear one by one in random places
public class CirclesGame : Minigame
{
    public int totalCircles = 5;
    private int clicked = 0;

    private Button circle;
    private TextMeshProUGUI counterText;

    protected override void Create()
    {
        title = "Haz clic en los " + totalCircles + " círculos";
        timeLimit = 6f;

        counterText = CreateText("0 / " + totalCircles, 28, new Vector2(0f, 110f));

        //there is only one circle: when you click it, it moves to another place
        circle = CreateButton("", new Vector2(70f, 70f), Vector2.zero, new Color(1f, 0.6f, 0.2f, 1f));
        circle.GetComponent<Image>().sprite = CreateCircleSprite();
        circle.onClick.AddListener(ClickCircle);

        MoveCircle();
    }

    void ClickCircle()
    {
        clicked++;
        counterText.text = clicked + " / " + totalCircles;

        if (clicked == totalCircles)
        {
            Win();
        }
        else
        {
            MoveCircle();
        }
    }

    //put the circle in a random place of the screen
    void MoveCircle()
    {
        circle.GetComponent<RectTransform>().anchoredPosition =
            new Vector2(Random.Range(-280f, 280f), Random.Range(-120f, 60f));
    }

    //Unity doesn't have a circle image by default, so we draw it pixel by pixel
    Sprite CreateCircleSprite()
    {
        int size = 64;
        double radius = size / 2f;
        Texture2D texture = new Texture2D(size, size);

        for (int x = 0; x < size; x++)
        {
            for (int y = 0; y < size; y++)
            {
                //distance from the pixel to the centre
                double dx = x + 0.5f - radius;
                double dy = y + 0.5f - radius;

                if (dx * dx + dy * dy <= radius * radius)
                {
                    texture.SetPixel(x, y, Color.white);
                }
                else
                {
                    texture.SetPixel(x, y, Color.clear);
                }
            }
        }

        texture.Apply();
        return Sprite.Create(texture, new Rect(0f, 0f, size, size), new Vector2(0.5f, 0.5f));
    }
}
