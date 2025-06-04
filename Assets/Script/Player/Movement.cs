using UnityEngine;
using States;
using Unity.VisualScripting;

public class Movement : MonoBehaviour {

    private GameInputManager input;
    private Vector2 moveDir;
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
    private bool canDash = true;

    [SerializeField] private float moveSpeed = 6;

    private void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        input = GameInputManager.Instance;
        dashState.Setup(this);
        leftRightState.Setup(this);
        topState.Setup(this);
        bottomState.Setup(this);
    }

    private void Update()
    {
        moveDir = input.GetMovementVectorNormalized();
        
        m_Dash = input.GetDashBool();
        if (m_Dash && canDash) Dashing();
        SelectState();
    }
    private void FixedUpdate()
    {
        if (dashState.dashTimeWindow > dashCooldownSeconds) canDash = true;
        HandleMovement();
    }
    private void Flip()
    {
        
    }
    private void SelectState()
    {
        PlayerState oldstate = state;
        if (!canDash)
        {
            state = dashState;
        }
        else if (moveDir != Vector2.zero)
        {
            if (moveDir.x != 0)
            {
                state = leftRightState;
                Flip();
            }
            else if (moveDir.y == 1)
            {
                state = topState;
                playerDirection = PlayerDirection.Top;
            }
            else
            {
                state = bottomState;
                playerDirection = PlayerDirection.Bottom;
            }

        }
        else state = idleState;
        //Updating if old state does not match new state
        if (oldstate != state)
        {
            oldstate.Exit();
            state.Enter();
        }
    }
    private void Dashing()
    {
        canDash = false;
        Vector2 dashDir = moveDir != Vector2.zero ? moveDir : FindDirection();
        rigidBody.AddForce(dashDir * dashSpeed);
    }
    private Vector2 FindDirection()
    {
        Vector2 faceDirection = Vector2.zero;
        switch (playerDirection)
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
    private void HandleMovement()
    {
        float moveDist = moveSpeed * Time.fixedDeltaTime;
        rigidBody.MovePosition(rigidBody.position + (moveDir * moveDist));
    }
    public float GetMoveSpeed() {
        return moveSpeed;
    }
}
