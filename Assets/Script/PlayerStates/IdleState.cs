using UnityEngine;
using States;

public class IdleState : PlayerState
{
    public override void Enter()
    {
        Debug.Log("Entered Idle State");
    }
    
    public override void Exit()
    {
        Debug.Log("Exited Idle State");
    }
}
