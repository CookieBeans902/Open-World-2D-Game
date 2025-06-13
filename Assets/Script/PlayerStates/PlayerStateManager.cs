using UnityEngine;
using States;

public class PlayerStateManager : MonoBehaviour
{
    public static PlayerStateManager Instance { get; private set; }
    [SerializeField] Dash dashState;
    [SerializeField] MoveState moveState;
    [SerializeField] IdleState idleState;
    PlayerState oldstate;
    Movement player;
    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void Setup(Movement transfer)
    {
        dashState.Setup(transfer);
        idleState.Setup(transfer);
        moveState.Setup(transfer);
        player = transfer;
    }
    public void SelectState(ref PlayerState initState)
    {
        oldstate = initState;
        if (player.inDash) //If player is in dashing, enter the dash State
        {
            initState = dashState;
        }
        else if (player.moveDir != Vector2.zero) //If the player has a movement input, enter any of the moving states
        {
            initState = moveState;
        }
        else initState = idleState; //The only other possibility is Vector is non zero is it is a zero vector, hence idle
        if (oldstate != initState) //Updating if old state does not match new state
        {
            oldstate?.Exit(); //Exit the old state, if there is any necessity.
            initState.Enter(); //Enter the new state
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
