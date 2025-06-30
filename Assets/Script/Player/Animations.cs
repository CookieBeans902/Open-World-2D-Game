using States;

using UnityEngine;

public class Animations : MonoBehaviour {
    [SerializeField] AnimationsSO playerAnimations;

    private GameInputManager input;
    private PlayerShared shared;
    private string currAnimation;
    private string newAnimation;
    private bool isMoving;
    private void Start() {
        input = GameInputManager.Instance;
        shared = GetComponent<PlayerShared>();
    }

    private void Update() {
        HandleMoveAnimation();

        UpdateAnimation();
    }

    private void HandleMoveAnimation() {
        isMoving = input.GetMovementVectorNormalized() != Vector2.zero;
        PlayerDirection dir = ConvertToEnum(shared.playerMove.playerDir);
        if (!isMoving) {
            switch (dir) {
                case PlayerDirection.Right:
                    newAnimation = playerAnimations.IdleRight.name;
                    break;
                case PlayerDirection.Left:
                    newAnimation = playerAnimations.IdleLeft.name;
                    break;
                case PlayerDirection.Down:
                    newAnimation = playerAnimations.IdleDown.name;
                    break;
                case PlayerDirection.Up:
                    newAnimation = playerAnimations.IdleUp.name;
                    break;
            }
            // if (dir.y < 0) newAnimation = playerAnimations.IdleDown.name;
            // else if (dir.y > 0) newAnimation = playerAnimations.IdleUp.name;
            // else if (dir.x > 0) newAnimation = playerAnimations.IdleRight.name;
            // else if (dir.x < 0) newAnimation = playerAnimations.IdleLeft.name;
        }
        else {
            switch (dir) {
                case PlayerDirection.Right:
                    newAnimation = playerAnimations.MoveRight.name;
                    break;
                case PlayerDirection.Left:
                    newAnimation = playerAnimations.MoveLeft.name;
                    break;
                case PlayerDirection.Down:
                    newAnimation = playerAnimations.MoveDown.name;
                    break;
                case PlayerDirection.Up:
                    newAnimation = playerAnimations.MoveUp.name;
                    break;
            }
            // if (dir.y < 0) newAnimation = playerAnimations.MoveDown.name;
            // else if (dir.y > 0) newAnimation = playerAnimations.MoveUp.name;
            // else if (dir.x > 0) newAnimation = playerAnimations.MoveRight.name;
            // else if (dir.x < 0) newAnimation = playerAnimations.MoveLeft.name;
        }
    }

    private void UpdateAnimation() {
        if (newAnimation != currAnimation) {
            currAnimation = newAnimation;
            shared.animator.CrossFade(currAnimation, 0.2f);
        }
    }

    private PlayerDirection ConvertToEnum(Vector2 dir) {
        if (dir == Vector2.left) return PlayerDirection.Left;
        else if (dir == Vector2.right) return PlayerDirection.Right;
        else if (dir == Vector2.down) return PlayerDirection.Down;
        else return PlayerDirection.Up;
    }
}
