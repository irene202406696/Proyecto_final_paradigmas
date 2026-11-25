using UnityEngine;

public interface IState
{
    // Interface that are going to have all the roommate states 
    void Enter();
    void Execute();
    void Exit();
}
