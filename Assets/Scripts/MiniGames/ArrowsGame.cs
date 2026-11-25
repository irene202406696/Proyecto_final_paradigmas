using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

//press the 3 arrows in the order they appear
public class ArrowsGame : Minigame
{
    //the 4 arrows and their names on the screen
    private Key[] arrowKeys = { Key.UpArrow, Key.DownArrow, Key.LeftArrow, Key.RightArrow };
    private string[] arrowNames = { "Arriba", "Abajo", "Izquierda", "Derecha" };

    //the 3 arrows to press (position in the arrays above) and how many are already pressed
    private int[] sequence = new int[3];
    private int pressed = 0;

    private TextMeshProUGUI arrowsText;

    protected override void Create()
    {
        title = "Pulsa las flechas en orden";
        timeLimit = 5f;

        for (int i = 0; i < 3; i++)
        {
            sequence[i] = Random.Range(0, 4);
        }

        arrowsText = CreateText("", 38, new Vector2(0f, 20f));
        UpdateText();
    }

    public override void UpdateGame()
    {
        if (result != 0 || Keyboard.current == null)
            return;

        //if you press the correct arrow we go to the next one, if you press another key you lose
        if (Keyboard.current[arrowKeys[sequence[pressed]]].wasPressedThisFrame)
        {
            pressed++;
            UpdateText();

            if (pressed == 3)
            {
                Win();
            }
        }
        else if (Keyboard.current.anyKey.wasPressedThisFrame)
        {
            Lose();
        }
    }

    //the arrows already pressed are shown in green
    void UpdateText()
    {
        string text = "";

        for (int i = 0; i < 3; i++)
        {
            if (i < pressed)
            {
                text += "<color=#66FF66>" + arrowNames[sequence[i]] + "</color>   ";
            }
            else
            {
                text += arrowNames[sequence[i]] + "   ";
            }
        }

        arrowsText.text = text;
    }
}
