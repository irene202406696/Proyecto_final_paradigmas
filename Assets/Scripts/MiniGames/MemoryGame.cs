using UnityEngine;
using UnityEngine.UI;
using TMPro;

//remember a number of 3 digits and choose it between 3 options
public class MemoryGame : Minigame
{
    //how long the number is shown
    public double showTime = 1.5f;

    private double elapsed = 0f;
    private bool showingNumber = true;
    private int number;

    private TextMeshProUGUI numberText;
    private Button[] optionButtons = new Button[3];

    protected override void Create()
    {
        title = "Memoriza el número";
        timeLimit = 5f;

        number = Random.Range(100, 1000);
        numberText = CreateText(number.ToString(), 80, new Vector2(0f, 30f));

        //the wrong options are the same number changing the last digit or the middle digit
        int lastDigit = number % 10;
        int middleDigit = (number / 10) % 10;
        int wrong1 = number - lastDigit + (lastDigit + Random.Range(1, 10)) % 10;
        int wrong2 = number - middleDigit * 10 + ((middleDigit + Random.Range(1, 10)) % 10) * 10;
        int[] wrongOptions = { wrong1, wrong2 };

        int correctPosition = Random.Range(0, 3);
        int wrongIndex = 0;

        //the buttons are created hidden, they appear when the number disappears
        for (int i = 0; i < 3; i++)
        {
            Vector2 position = new Vector2(-180f + 180f * i, -50f);

            if (i == correctPosition)
            {
                optionButtons[i] = CreateButton(number.ToString(), new Vector2(140f, 80f), position, buttonColor);
                optionButtons[i].onClick.AddListener(Win);
            }
            else
            {
                optionButtons[i] = CreateButton(wrongOptions[wrongIndex].ToString(), new Vector2(140f, 80f), position, buttonColor);
                optionButtons[i].onClick.AddListener(Lose);
                wrongIndex++;
            }

            optionButtons[i].gameObject.SetActive(false);
        }
    }

    public override void UpdateGame()
    {
        elapsed += Time.deltaTime;

        //after some time the number disappears and the options appear
        if (showingNumber && elapsed >= showTime)
        {
            showingNumber = false;
            title = "¿Cuál era el número?";
            numberText.text = "";

            for (int i = 0; i < 3; i++)
            {
                optionButtons[i].gameObject.SetActive(true);
            }
        }
    }
}
