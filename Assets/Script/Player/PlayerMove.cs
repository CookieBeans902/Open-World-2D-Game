using Game.Utils;
using UnityEngine;
using Unity.VisualScripting;

public class PlayerMove : MonoBehaviour, IDataPersistence {
    [Header("References")]
    private GameInputManager input;
    private Rigidbody2D rb;
    private PlayerShared shared;
    [Header("Variables")]
    public Vector2 newDir;
    public Vector2 currDir;
    private Vector2 prevInput;
    private bool m_Dash;
    public Vector2 playerDir;
    public bool canMove { get; private set; }
    public bool canDash = true;
    public bool inDash = false;
    public float moveSpeed;
    public float speedNerf;
    private float speedBuff;
    private float elapsed = 0;
    private float buffTime;

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
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update() {
        FixDirection(); //Removes diagonal movement by checking with previous direction inputs
        if (speedBuff > 0) {
            if (elapsed < buffTime) {
                elapsed += Time.deltaTime;
            }
            else {
                elapsed = 0;
                speedBuff = 0;
                shared.moveSpeedFactor = 1;
            }
        }
    }

    private void FixedUpdate() {
        float moveDist = (moveSpeed + speedBuff - speedNerf) * Time.fixedDeltaTime;
        if (canMove && newDir != Vector2.zero) rb.MovePosition(rb.position + (newDir * moveDist));
    }

    void FixDirection() {
        newDir = input.GetMovementVectorNormalized(); //Getting the direction of the input

        if (prevInput.x != 0 && prevInput.y != 0) {
            if (newDir.x != 0 && newDir.y != 0) {
                if (playerDir.x != 0)
                    newDir.y = 0;
                else if (playerDir.y != 0)
                    newDir.x = 0;
            }
        }
        else {
            if (newDir.x != 0 && newDir.y != 0) {
                if (playerDir.x != 0)
                    newDir.x = 0;
                else if (playerDir.y != 0)
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

    public void SetSpeedBuff(float amount) {
        elapsed = 0;
        buffTime = 2;
        speedBuff = amount;
        shared.moveSpeedFactor = 1 + ((float)amount / moveSpeed);
    }

    public void LoadData(GameData gameData) {
        gameObject.transform.position = gameData.pos;
    }

    public void SaveData(GameData gameData) {
        gameData.pos = gameObject.transform.position;
    }
}
