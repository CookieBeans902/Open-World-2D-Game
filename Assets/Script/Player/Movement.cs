using UnityEngine;
using States;
using Unity.VisualScripting;

public class Movement : MonoBehaviour
{
    private GameInputManager input;
    public Vector2 moveDir;
    [SerializeField] private float dashSpeed = 6000;
    PlayerState state;
    [SerializeField] Dash dashState;
    [SerializeField] float dashCooldownSeconds = 2;
    [SerializeField] MoveLeftRight leftRightState;
    [SerializeField] MoveTop topState;
    [SerializeField] MoveBottom bottomState;
    [SerializeField] IdleState idleState;
    public PlayerDirection playerDirection;
    public Rigidbody2D rigidBody;
    private bool m_Dash;
    public bool canDash = true;
    public bool inDash = false;
    [SerializeField] private float moveSpeed = 6;
    private void Awake()
    {
        rigidBody = GetComponent<Rigidbody2D>(); //Getting the component from the gameobject
    }
    private void Start()
    {
        input = GameInputManager.Instance; 
        /*Initializing all the setup for each state to prevent null reference exceptions. Passing the class 
        instance to handle the inputs which change every frame, and hence we need a reference and not a 
        value(which remains constant)*/ 
        dashState.Setup(this); 
        idleState.Setup(this); 
        leftRightState.Setup(this);
        topState.Setup(this);
        bottomState.Setup(this);
    }
    private void Update()
    {
        moveDir = input.GetMovementVectorNormalized(); //Getting the direction of the input
        m_Dash = input.GetDashBool();   //To check whether the button is pressed or not
        if (canDash && m_Dash) inDash = true; //Player enters DashState only when he presses button and cooldown finishes
        SelectState(); //Selects the state depending on which state the player is present in
    }
    private void FixedUpdate()
    {
        if (dashState.timeSinceLastDash > dashCooldownSeconds) canDash = true; //Checking for when the cooldown ends
        if (state != dashState) state?.Execute(); //Execute method is present for non dash states, so call it
    }

    private void SelectState()
    {
        PlayerState oldstate = state;
        if (inDash) //If player is in dashing, enter the dash State
        {
            state = dashState;
        }
        else if (moveDir != Vector2.zero) //If the player has a movement input, enter any of the moving states
        {
            if (moveDir.x != 0) //Either player is moving left and right or diagonally(no difference in animations)
            {
                state = leftRightState; //Enter the left right state
            }
            else if (moveDir.y == 1) //Input is 1 when player is moving vertically up
            {
                state = topState;
                playerDirection = PlayerDirection.Top; //Set the latest state direction to top. This handles idle dashing
            }
            else //Since Vector2 is not zero, the only other possibility is bottom state
            {
                state = bottomState;
                playerDirection = PlayerDirection.Bottom;
            }
        }
        else state = idleState; //The only other possibility is Vector is non zero is it is a zero vector, hence idle
        if (oldstate != state) //Updating if old state does not match new state
        {
            oldstate?.Exit(); //Exit the old state, if there is any necessity.
            state.Enter(); //Enter the new state
        }
    }
    public float GetMoveSpeed() // Self - Explanatory, get the private float variable moveSpeed;
    {
        return moveSpeed;
    }
    public float GetDashSpeed() // Not gonna explain twice, it's the same thing
    {
        return dashSpeed;
    }
}
