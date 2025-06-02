using UnityEngine;

public class Movement : MonoBehaviour {

    private GameInputManager input;
    private Rigidbody2D rigidBody;

    [SerializeField]
    private float moveSpeed = 6;

    private void Start() {
        rigidBody = GetComponent<Rigidbody2D>();
        input = GameInputManager.Instance;
    }

    private void FixedUpdate() {
        HandleMovement();
    }

    private void HandleMovement() {
        Vector2 moveDir = input.GetMovementVectorNormalized();
        float moveDist = moveSpeed * Time.fixedDeltaTime;

        rigidBody.MovePosition(rigidBody.position + (moveDir * moveDist));
    }

    public float GetMoveSpeed() {
        return moveSpeed;
    }
}
