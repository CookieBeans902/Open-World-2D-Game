// using UnityEngine;
// using States;
// using System.Collections;

// public class DashState : PlayerState {
//     private float latestDashTime;
//     private Vector2 startPosition;
//     private float currentDashTime;
//     private Vector2 dashDirection;
//     [Header("Variables")]
//     public float dashCooldownSeconds = 2f;
//     [SerializeField] private float dashDuration = 0.1f;
//     public float dashDistanceMul = 1f;
//     [SerializeField] bool ModifiedLerp;
//     public float timeSinceLastDash => Time.time - latestDashTime;
//     public override void Enter() {
//         startPosition = player.transform.position; //Store the position at which the player calls the dash
//         latestDashTime = Time.time; //Keep record of the time, to initiate cooldown logic
//         player.canDash = false; //canDash is the variable that keeps track of the cooldwown
//         dashDirection = player.newDir != Vector2.zero ? player.newDir : FindDirection();
//         // StartCoroutine(DoDash()); //StartCoroutine
//         // The next line is written, just to handle idle dashing - if the player dashes without moving
//     }
//     public IEnumerator doDash() {
//         currentDashTime = 0f;
//         Vector2 dashDistance = dashDirection * dashDistanceMul * 4;
//         Vector3 targetPosition = startPosition + dashDistance;
//         while (currentDashTime <= dashDuration) {
//             float t = currentDashTime / dashDuration;
//             player.transform.position = Vector2.Lerp(startPosition, targetPosition, Mathf.Sqrt(t));
//             currentDashTime += Time.deltaTime;
//             yield return null; //Sends the while logic to the next frame update.
//         }
//         player.transform.position = targetPosition;
//         player.inDash = false; //After the while loop finishes, the dash finishes, so player is not in dash state anymore
//     }
//     private Vector2 FindDirection() // Just read this code, it really isn't that hard.
//     {
//         Vector2 faceDirection = Vector2.zero;
//         switch (ConvertToEnum(player.playerDir)) {
//             case PlayerDirection.Left:
//                 faceDirection = new Vector2(-1, 0);
//                 break;
//             case PlayerDirection.Right:
//                 faceDirection = new Vector2(1, 0);
//                 break;
//             case PlayerDirection.Up:
//                 faceDirection = new Vector2(0, 1);
//                 break;
//             case PlayerDirection.Down:
//                 faceDirection = new Vector2(0, -1);
//                 break;
//             default:
//                 return faceDirection;
//         }
//         return faceDirection;
//     }

//     private PlayerDirection ConvertToEnum(Vector2 dir) {
//         if (dir == Vector2.left) return PlayerDirection.Left;
//         else if (dir == Vector2.right) return PlayerDirection.Right;
//         else if (dir == Vector2.down) return PlayerDirection.Down;
//         else return PlayerDirection.Up;
//     }
// }

