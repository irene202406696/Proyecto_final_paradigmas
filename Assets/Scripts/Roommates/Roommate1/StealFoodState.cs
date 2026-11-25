using UnityEngine;

public class StealFoodState : IState
{
    private RoommateController roommate;
    private float timer;
    private bool hasArrived;

    public StealFoodState(RoommateController roommate)
    {
        this.roommate = roommate;
    }

    public void Enter()
    {
        Debug.Log("The roommate is going to the fridge");

        timer = 0f;
        hasArrived = false;

        // Move towards the fridge
        roommate.MoveTo(roommate.fridgeTarget);
    }

    public void Execute()
    {
        // Check if the roommate has reached the fridge
        if (!hasArrived)
        {
            if (roommate.HasArrived())
            {
                hasArrived = true;
                roommate.StopMoving();

                Debug.Log("The roommate starts eating");
            }

            return;
        }

        // Eat for five seconds
        timer += Time.deltaTime;

        if (timer >= 5f)
        {
            roommate.ChangeState(new IdleState(roommate));
        }
    }

    public void Exit()
    {
        roommate.StopMoving();
        Debug.Log("The roommate has finished eating");
    }
}