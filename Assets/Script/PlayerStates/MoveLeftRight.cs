using UnityEngine;
using States;

public class MoveLeftRight : PlayerState
{
    private bool m_facingRight;
    public override void Enter()
    {
        
    }
    public override void Exit()
    {
        // I sometimes like commenting and i sometimes don't. It helps me practice typing but sometimes i 
        // have to think whether i have to explain something very simple. I can't avoid it because its good 
        // practice, and sometimes i look in the mirror and realize i might be the dumb person not understanding
        // the simple code.
    }
    public override void Execute()
    {
        base.Execute();
        //Logic for flipping sprites when facing left or right.
        if (player.moveDir.x < 0 && m_facingRight)
        {
            Flip();
            player.playerDirection = PlayerDirection.Left; // Since the player is moving left now.
        }
        else if (player.moveDir.x > 0 && !m_facingRight)
        {
            Flip();
            player.playerDirection = PlayerDirection.Right; // The player is moving right now DUH.
        }
    }
    private void Flip()
    {
        // We flipped the direction of the sprite, so the bool must also be flipped, akin to the player flipping directions.
        m_facingRight = !m_facingRight;
        // Now we change the direction of the actual sprite.
        Vector3 currentScale = player.transform.localScale;
        currentScale.x *= -1;
        player.transform.localScale = currentScale;
    }
}
