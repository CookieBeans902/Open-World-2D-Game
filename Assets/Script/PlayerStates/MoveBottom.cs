using UnityEngine;
using States;

public class MoveBottom: PlayerState
{
    public override void Enter()
    {
        Debug.Log("Entered bottom State");
        
    }
    
    public override void Exit()
    {
        Debug.Log("Exited bottom State");
    }
}
