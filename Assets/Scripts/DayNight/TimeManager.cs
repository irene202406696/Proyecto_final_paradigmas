
using UnityEngine;
using TMPro;
using System;

public class TimeManager : MonoBehaviour
{

    public double hour = 8.0;
    public int day = 1;
    public double secondsPerGameHour = 30.0;
    public Light sun;
    public TextMeshProUGUI clockText;

    private TimeState currentState;

    void Start()
    {
        if (hour >= 6.0 && hour < 18.0)
        {
            currentState = new DayState();
        }
        else
        {
            currentState = new NightState();
        }

        currentState.OnStateEnter(this);

        UpdateSun();
        UpdateClock();
        currentState.UpdateState(this);
    }

    void Update()
    {
        // Move forward
        if (secondsPerGameHour <= 0.0) return;

        hour += Time.deltaTime / secondsPerGameHour;

        // Day Change
        while (hour >= 24.0)
        {
            hour -= 24.0;
            day++;
        }

        // Stage change
        TimeState nextState = currentState.ManageTime(
            (float)hour
        );

        if (nextState != currentState)
        {
            currentState.OnStateExit(this);
            currentState = nextState;
            currentState.OnStateEnter(this);
        }

        currentState.UpdateState(this);

        UpdateSun();
        UpdateClock();
    }

    void UpdateSun()
    {
        if (sun == null) return;

        float angle = (float)(hour / 24.0 * 360.0 - 90.0);

        sun.transform.rotation = Quaternion.Euler(angle,-30f,0);
    }

    void UpdateClock()
    {
        if (clockText == null) return;

        int hours = (int)hour;
        int minutes = (int)((hour - hours) * 60.0);

        clockText.text = "Dia " + day + "\n"+ hours.ToString("00") + ":"+ minutes.ToString("00");
    }

    public void AdvanceHours(double hours)
    {
        if (hours < 0.0) return;

        hour += hours;
    }
}
