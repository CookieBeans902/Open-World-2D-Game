using UnityEngine;
using States;

public class MoveState : PlayerState
{
    public override void Enter()
    {
        
    }
    public override void Execute()
    {
        float moveDist = moveSpeed * Time.fixedDeltaTime;
        _rigidBody.MovePosition(_rigidBody.position + (player.moveDir * moveDist));
    }
    public override void Exit()
    {
        // I sometimes like commenting and i sometimes don't. It helps me practice typing but sometimes i 
        // have to think whether i have to explain something very simple. I can't avoid it because its good 
        // practice, and sometimes i look in the mirror and realize i might be the dumb person not understanding
        // the simple code.
    }
}
