using Game.Utils;
using UnityEngine;
using States;
using Unity.VisualScripting;

public class Movement : MonoBehaviour, IDataPersistence
{
    [Header("References")]
    public Rigidbody2D rigidBody;
    public Animator anim;
    private GameInputManager input;
    private PlayerStateManager PlayerState;
    [Header("Variables")]
    public Vector2 newDir;
    public Vector2 currDir;
    private bool m_Dash;
    PlayerState state;
    public PlayerDirection playerDirection;
    public bool canDash = true;
    public bool inDash = false;
    [SerializeField] private float moveSpeed = 6;
    private void Awake()
    {
        rigidBody = GetComponent<Rigidbody2D>(); //Getting the component from the gameobject
        PlayerState = GetComponent<PlayerStateManager>();//Referencing the State Manager component
        anim = GetComponent<Animator>();
        currDir = Vector2.right;
    }
    private void Start()
    {
        input = GameInputManager.Instance;
        /*Initializing all the setup for each state to prevent null reference exceptions. Passing the class 
        instance to handle the inputs which change every frame, and hence we need a reference and not a 
        value(which remains constant)*/
        PlayerState.Setup(this);
    }

    private void Update()
    {
        newDir = input.GetMovementVectorNormalized(); //Getting the direction of the input
        FixDirection(); //Removes diagonal movement by checking with previous direction inputs
        m_Dash = input.GetDashBool();   //To check whether the button is pressed or not
        if (canDash && m_Dash) inDash = true; //Player enters DashState only when he presses button and cooldown finishes
        PlayerState.SelectState(ref state); //Selects the state depending on which state the player is present in
    }
    private void FixedUpdate()
    {
        if (state != null) state.Execute(); //Call the execute method to execute the necessary functions
    }

    public float GetMoveSpeed() // Self - Explanatory, get the private float variable moveSpeed;
    {
        return moveSpeed;
    }
    
    void FixDirection()
    {
        if (newDir.x != 0 && newDir.y != 0)
        {
            if (currDir.x != 0)
                newDir.y = 0;
            else if (currDir.y != 0)
                newDir.x = 0;
        }
        newDir = newDir.normalized;
}

    public Vector3 GetPlayerDir() {
        return currDir;
    }

    public void LoadData(GameData gameData)
    {
        gameObject.transform.position = gameData.pos;
    }

    public void SaveData(GameData gameData)
    {
        gameData.pos = gameObject.transform.position;
    }
}
