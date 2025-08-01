// using UnityEngine;
// using States;

// public class PlayerStateManager : MonoBehaviour {
//     [SerializeField] private DashState dashState;
//     [SerializeField] private MoveState moveState;
//     [SerializeField] private IdleState idleState;
//     private PlayerState oldstate;
//     private Movement player;

//     public void Setup(PlayerShared shared) {
//         dashState.Setup(shared);
//         idleState.Setup(shared);
//         moveState.Setup(shared);
//         player = shared.playerMove;
//     }
//     public void SelectState(ref PlayerState State) {
//         oldstate = State;
//         if (player.inDash) //If player is in dashing, enter the dash State
//         {
//             State = dashState;
//         }
//         else if (player.newDir != Vector2.zero) //If the player has a movement input, enter any of the moving states
//         {
//             State = moveState;
//         }
//         else State = idleState; //The only other possibility is Vector is non zero is it is a zero vector, hence idle
//         if (oldstate != State) //Updating if old state does not match new state
//         {
//             oldstate?.Exit(); //Exit the old state, if there is any necessity.
//             State.Enter(); //Enter the new state
//         }
//     }
//     // Update is called once per frame
//     void Update() {
//         if (dashState.timeSinceLastDash > dashState.dashCooldownSeconds) player.canDash = true;
//     }
// }
