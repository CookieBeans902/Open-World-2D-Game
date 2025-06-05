using UnityEngine;
using States;
using System.Collections;

public class Dash : PlayerState
{
    private float latestDashTime;
    [SerializeField] private float dashSpeed = 20f;
    private Transform startPosition;
    private float elapsedTime;
    private Vector2 dashDirection;
    [SerializeField] public float dashDuration = 0.1f;
    public float timeSinceLastDash => Time.time - latestDashTime;
    public override void Enter()
    {
        startPosition = player.transform; //Store the position at which the player calls the dash
        latestDashTime = Time.time; //Keep record of the time, to initiate cooldown logic
        player.canDash = false; //canDash is the variable that keeps track of the cooldwown
        elapsedTime = 0f;
        dashDirection = player.moveDir != Vector2.zero ? player.moveDir : FindDirection();
        StartCoroutine(doDash()); //StartCoroutine
        // The next line is written, just to handle idle dashing - if the player dashes without moving
    }
    public IEnumerator doDash()
    {
        while (elapsedTime <= dashDuration)
        {
            float t = timeSinceLastDash / dashDuration;
            Vector2 dashDistance = dashSpeed * dashDuration * dashDirection;
            Vector3 dashDistance3D = new Vector3(dashDistance.x, dashDistance.y, 0);
            player.transform.position = Vector2.Lerp(startPosition.position, startPosition.position + dashDistance3D,Mathf.Pow(t,5));
            elapsedTime += Time.deltaTime;
            yield return null; //Sends the while logic to the next frame update.
        }
        player.inDash = false; //After the while loop finishes, the dash finishes, so player is not in dash state anymore
    }
    private Vector2 FindDirection() // Just read this code, it really isn't that hard.
    {
        Vector2 faceDirection = Vector2.zero;
        switch (player.playerDirection)
        {
            case PlayerDirection.Left:
                faceDirection = new Vector2(-1, 0);
                break;
            case PlayerDirection.Right:
                faceDirection = new Vector2(1, 0);
                break;
            case PlayerDirection.Top:
                faceDirection = new Vector2(0, 1);
                break;
            case PlayerDirection.Bottom:
                faceDirection = new Vector2(0, -1);
                break;
            default:
                return faceDirection;
        }
        return faceDirection;
    }
}

