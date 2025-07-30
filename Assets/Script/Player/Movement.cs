using Game.Utils;
using UnityEngine;
using States;
using Unity.VisualScripting;

public class Movement : MonoBehaviour, IDataPersistence {
    [Header("References")]
    private GameInputManager input;
    private Rigidbody2D rb;
    private PlayerShared shared;
    [Header("Variables")]
    public Vector2 newDir;
    public Vector2 currDir;
    private Vector2 prevInput;
    private bool m_Dash;
    private PlayerState state;
    public Vector2 playerDir;
    public bool canMove { get; private set; }
    public bool canDash = true;
    public bool inDash = false;
    public float baseMoveSpeed;
    public float moveSpeed;

    private void Awake() {
        rb = GetComponent<Rigidbody2D>();
        newDir = Vector2.right;
        currDir = Vector2.right;
        playerDir = Vector2.right;
        canMove = true;
    }
    private void Start() {
        input = GameInputManager.Instance;
        shared = GetComponent<PlayerShared>(); // Referencing the player shared component for common data

        /*Initializing all the setup for each state to prevent null reference exceptions. Passing the class 
        instance to handle the inputs which change every frame, and hence we need a reference and not a 
        value(which remains constant)*/
        // PlayerState.Setup(shared);
    }

    private void Update() {
        FixDirection(); //Removes diagonal movement by checking with previous direction inputs
    }

    private void FixedUpdate() {
        Vector2 dir = input.GetMovementVectorNormalized();
        rb.MovePosition((Vector2)transform.position + dir * Time.fixedDeltaTime * moveSpeed);
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

    public void DisableMovement() {
        canMove = false;
    }

    public void EnableMovement() {
        canMove = true;
    }

    public void LoadData(GameData gameData) {
        gameObject.transform.position = gameData.pos;
    }

    public void SaveData(GameData gameData) {
        gameData.pos = gameObject.transform.position;
    }
}
