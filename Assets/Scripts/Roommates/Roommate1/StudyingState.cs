using UnityEngine;

public class StudyingState : IState
{
    private RoommateController roommate;
    private double timer;

    public StudyingState(RoommateController roommate)
    {
        this.roommate = roommate;
    }

    public void Enter()
    {
        Debug.Log("El compañero empieza a estudiar");
        timer = 0;
    }

    public void Execute()
    {
        timer += Time.deltaTime;

        if (timer >= 10)
        {
            roommate.ChangeState(new IdleState(roommate));
        }
    }

    public void Exit()
    {
        Debug.Log("El compañero deja de estudiar");
    }
}