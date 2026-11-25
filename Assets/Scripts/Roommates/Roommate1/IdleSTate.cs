using UnityEngine;

public class IdleState : IState
{
    private RoommateController roommate;

    public IdleState(RoommateController roommate)
    {
        this.roommate = roommate;
    }

    public void Enter()
    {
        Debug.Log("El compañero entra en Idle");
    }

    public void Execute()
    {
        if (roommate.playerHunger.value < 40 )
        {
            roommate.ChangeState(new StealFoodState(roommate)); 
        }


    }

    public void Exit()
    {
        Debug.Log("El compañero sale de Idle");
    }
}