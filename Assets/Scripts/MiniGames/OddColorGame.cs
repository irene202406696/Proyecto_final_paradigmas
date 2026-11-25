using UnityEngine;
using UnityEngine.UI;

//click the button that has a different colour
public class OddColorGame : Minigame
{
    protected override void Create()
    {
        title = "Haz clic en el color distinto";
        timeLimit = 4f;

        //all the buttons have the same colour except one, that is lighter
        float hue = Random.value;
        Color normalColor = Color.HSVToRGB(hue, 0.7f, 0.6f);
        Color oddColor = Color.HSVToRGB(hue, 0.7f, 0.95f);

        int oddPosition = Random.Range(0, 6);

        //6 buttons in 2 rows of 3
        for (int i = 0; i < 6; i++)
        {
            Vector2 position = new Vector2(-160f + 160f * (i % 3), 60f - 140f * (i / 3));

            if (i == oddPosition)
            {
                Button button = CreateButton("", new Vector2(110f, 110f), position, oddColor);
                button.onClick.AddListener(Win);
            }
            else
            {
                Button button = CreateButton("", new Vector2(110f, 110f), position, normalColor);
                button.onClick.AddListener(Lose);
            }
        }
    }
}
