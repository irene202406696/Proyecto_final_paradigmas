using UnityEngine;


public abstract class TimeState
{
    public abstract TimeState ManageTime(double hour);

    public abstract void OnStateEnter(TimeManager manager);

    public abstract void OnStateExit(TimeManager manager);

    public abstract void UpdateState(TimeManager manager);
}
