
using System;
using UnityEngine;

public class NightState : TimeState
{
    public override TimeState ManageTime(double hour)
    {
        // At 18 we return to the day state
        if (hour >=  6 && hour < 18)
        {
            return new DayState();
        }

        return this;
    }

    public override void OnStateEnter(TimeManager manager)
    {
 
        Debug.Log("Ha empezado la noche");
    }

    public override void OnStateExit(TimeManager manager)
    {
        Debug.Log("Ha terminado la noche");
    }

    public override void UpdateState(TimeManager manager)
    {
        if (manager.sun == null) return;

        double hour = manager.hour;

        //For simplicity's sake, we'll assume that nighttime is from 6:00 p.m. to 12:00 a.m.
        if (hour < 6.0)
        {
            hour += 24.0;
        }

       // Intensity increases from 18 to 00 and increases from 00 to 6
        double intensity = 0.05 * (1.0 - Math.Sin(Math.PI * (hour - 18.0) / 12.0));

        manager.sun.intensity = (float)Math.Max(0.0, intensity);

        // Night colors 
        Color sunsetColor = new Color(1f, 0.75f, 0.5f);
        Color nightColor = new Color(0.25f, 0.35f, 0.6f);

        double transition = Math.Clamp(1.0 - Math.Abs(hour - 24.0) / 6.0,0.0,1.0);

        manager.sun.color = Color.Lerp(sunsetColor,nightColor,(float)transition);
    }
}
