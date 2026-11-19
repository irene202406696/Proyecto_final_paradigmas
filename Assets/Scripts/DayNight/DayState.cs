
using UnityEngine;
using System; 

public class DayState : TimeState
{
    public override TimeState ManageTime(double hour)
    {
        // If it is 18:00 we change to the Night State
        if (hour >= 18f || hour < 6f)
        {
            return new NightState();
        }

        return this; // We return this state 
    }

    public override void OnStateEnter(TimeManager manager)
    {
        // Chante lighting
        if (manager.sun != null)
        {
            manager.sun.enabled = true;
        }

        Debug.Log("Ha empezado el dia");
    }

    public override void OnStateExit(TimeManager manager)
    {
        
        Debug.Log("Ha terminado el dia");
    }

    public override void UpdateState(TimeManager manager)
    {

        double hour = manager.hour;
        
        //Intensity increases during the morning and decreases during the afternoon

        double intensity = Math.Sin(Math.PI * (hour - 6) / 12);

        manager.sun.intensity = (float)Math.Max(0, intensity);

        // Light changes depending on the hour 
        Color morningColor = new Color(1f, 0.75f, 0.5f);
        Color noonColor = Color.white;

        double transition = Math.Clamp(1.0 - Math.Abs(hour - 12.0) / 6.0,0.0,1.0);

        manager.sun.color = Color.Lerp(morningColor,noonColor,(float)transition);
    }
}
