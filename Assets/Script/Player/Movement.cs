using Game.Utils;
using UnityEngine;

public class Movement : MonoBehaviour {

    private GameInputManager input;
    private Rigidbody2D rigidBody;
    private Vector3 currDir;

    [SerializeField]
    private float moveSpeed = 6;

    private void Start() {
        rigidBody = GetComponent<Rigidbody2D>();
        input = GameInputManager.Instance;

        currDir = Vector2.right;
    }

    private void FixedUpdate() {
        HandleMovement();
    }

    private void HandleMovement() {
        Vector2 newDir = input.GetMovementVectorNormalized();
        float moveDist = moveSpeed * Time.fixedDeltaTime;
        if (newDir.x != 0 && newDir.y != 0) {
            if (currDir.x != 0)
                newDir.y = 0;
            else if (currDir.y != 0)
                newDir.x = 0;
        }
        newDir = newDir.normalized;

        rigidBody.MovePosition(rigidBody.position + (newDir * moveDist));
        if (newDir != Vector2.zero) currDir = newDir;
    }

    public float GetMoveSpeed() {
        return moveSpeed;
    }

    public Vector3 GetPlayerDir() {
        return currDir;
    }
}
