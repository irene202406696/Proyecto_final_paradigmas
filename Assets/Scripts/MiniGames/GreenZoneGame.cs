using UnityEngine;
using UnityEngine.UI;

//click when the moving line is inside the green zone
public class GreenZoneGame : Minigame
{
    //how fast the line moves and the size of the bar and the green zone
    public double speed = 450f;
    private double barWidth = 500f;
    private double zoneWidth = 90f;

    //position of the green zone and of the line in the bar
    private double zonePosition;
    private double linePosition;

    private double elapsed = 0f;
    private RectTransform line;

    protected override void Create()
    {
        title = "Haz clic cuando la línea esté en verde";
        timeLimit = 5f;

        //grey bar
        CreateImage("Bar", new Vector2((float)barWidth, 40f), new Vector2(0f, 40f), new Color(1f, 1f, 1f, 0.25f));

        //green zone in a random place inside the bar
        double limit = (barWidth - zoneWidth) / 2f;
        zonePosition = Random.Range(-(float)limit, (float)limit);
        CreateImage("GreenZone", new Vector2((float)zoneWidth, 40f), new Vector2((float)zonePosition, 40f), new Color(0.3f, 0.85f, 0.3f, 1f));

        //the line starts on the left
        linePosition = -barWidth / 2f;
        line = CreateImage("Line", new Vector2(8f, 60f), new Vector2((float)linePosition, 40f), Color.white).rectTransform;

        Button button = CreateButton("¡Ahora!", new Vector2(180f, 70f), new Vector2(0f, -80f), buttonColor);
        button.onClick.AddListener(CheckZone);
    }

    public override void UpdateGame()
    {
        elapsed += Time.deltaTime;

        //the line goes from one side of the bar to the other and back
        linePosition = Mathf.PingPong((float)(elapsed * speed), (float)barWidth) - barWidth / 2f;
        line.anchoredPosition = new Vector2((float)linePosition, 40f);
    }

    //you win if the line is inside the green zone when you click
    void CheckZone()
    {
        if (Mathf.Abs((float)(linePosition - zonePosition)) <= zoneWidth / 2f)
        {
            Win();
        }
        else
        {
            Lose();
        }
    }
}
