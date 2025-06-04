using UnityEngine;
using States;

public class MoveTop : PlayerState
{
    public override void Enter()
    {
        Debug.Log("Entered Top State");
    }
    public override void Exit()
    {
        Debug.Log("Exited Top State");
    }
}

