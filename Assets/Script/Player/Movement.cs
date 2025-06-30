using Game.Utils;
using UnityEngine;
using States;
using Unity.VisualScripting;

public class Movement : MonoBehaviour {
    [Header("References")]
    private GameInputManager input;
    private PlayerShared shared;
    [Header("Variables")]
    public Vector2 newDir;
    public Vector2 currDir;
    private Vector2 prevInput;
    private bool m_Dash;
    private PlayerStateManager PlayerState;
    private PlayerState state;
    public Vector2 playerDir;
    public bool canDash = true;
    public bool inDash = false;
    public float baseMoveSpeed;
    public float moveSpeed;

    private void Awake() {
        newDir = Vector2.right;
        currDir = Vector2.right;
        playerDir = Vector2.right;
    }
    private void Start() {
        input = GameInputManager.Instance;
        shared = GetComponent<PlayerShared>(); // Referencing the player shared component for common data
        PlayerState = GetComponent<PlayerStateManager>(); // Referencing the State Manager component

        /*Initializing all the setup for each state to prevent null reference exceptions. Passing the class 
        instance to handle the inputs which change every frame, and hence we need a reference and not a 
        value(which remains constant)*/
        PlayerState.Setup(shared);
    }

    private void Update() {
        FixDirection(); //Removes diagonal movement by checking with previous direction inputs
        m_Dash = input.GetDashBool();   //To check whether the button is pressed or not
        if (canDash && m_Dash) inDash = true; //Player enters DashState only when he presses button and cooldown finishes
        PlayerState.SelectState(ref state); //Selects the state depending on which state the player is present in
    }

    private void FixedUpdate() {
        if (state != null) state.Execute(); //Call the execute method to execute the necessary functions
    }


    void FixDirection() {
        newDir = input.GetMovementVectorNormalized(); //Getting the direction of the input

        if (prevInput.x != 0 && prevInput.y != 0) {
            if (newDir.x != 0 && newDir.y != 0) {
                if (currDir.x != 0)
                    newDir.y = 0;
                else if (currDir.y != 0)
                    newDir.x = 0;
            }
        }
        else {
            if (newDir.x != 0 && newDir.y != 0) {
                if (currDir.x != 0)
                    newDir.x = 0;
                else if (currDir.y != 0)
                    newDir.y = 0;
            }
        }
        newDir = newDir.normalized;
        prevInput = input.GetMovementVectorNormalized();
        if (newDir != Vector2.zero) playerDir = newDir;
    }
}
