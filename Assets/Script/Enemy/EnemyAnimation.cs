using Game.Utils;

using UnityEngine;

public class EnemyAnimation : MonoBehaviour {
    private MovementBase enemyMovement;
    private Vector2 prevDir;
    private string currAnimation;
    private string newAnimation;
    private bool isMoving;

    [SerializeField] private Animator animator;
    [SerializeField] private AnimationsSO animationData;
    private void Start() {
        enemyMovement = GetComponent<MovementBase>();
        prevDir = Vector2.right;
    }

    private void Update() {
        HandleMoveAnimation();

        UpdateAnimation();
    }

    private void HandleMoveAnimation() {
        Vector2 dir = enemyMovement.GetMoveDir();
        isMoving = dir != Vector2.zero;
        if (!enemyMovement.canMove) return;

        if (!isMoving) {
            if (prevDir == Vector2.down) newAnimation = animationData.IdleDown.name;
            else if (prevDir == Vector2.up) newAnimation = animationData.IdleUp.name;
            else if (prevDir == Vector2.right) newAnimation = animationData.IdleRight.name;
            else if (prevDir == Vector2.left) newAnimation = animationData.IdleLeft.name;
        }
        else {
            if (dir == Vector2.down) newAnimation = animationData.MoveDown.name;
            else if (dir == Vector2.up) newAnimation = animationData.MoveUp.name;
            else if (dir == Vector2.right) newAnimation = animationData.MoveRight.name;
            else if (dir == Vector2.left) newAnimation = animationData.MoveLeft.name;
            prevDir = dir;
        }
    }

    public void PlaySlashAnimation(float waitTime) {
        enemyMovement.DisableMovement();
        float animTime = 0;
        Vector2 dir = enemyMovement.GetMoveDir();

        if (dir == Vector2.right) {
            newAnimation = animationData.SlashRight.name;
            animTime = animationData.SlashRight.length;
        }
        else if (dir == Vector2.left) {
            newAnimation = animationData.SlashLeft.name;
            animTime = animationData.SlashLeft.length;
        }
        else if (dir == Vector2.down) {
            newAnimation = animationData.SlashDown.name;
            animTime = animationData.SlashDown.length;
        }
        else if (dir == Vector2.up) {
            newAnimation = animationData.SlashUp.name;
            animTime = animationData.SlashUp.length;
        }

        FunctionTimer.CreateSceneTimer(() => {
            enemyMovement.EnableMovement();
            // enemyMovement.targetReached = false;
        }, animTime + waitTime);
    }

    private void UpdateAnimation() {
        if (newAnimation != currAnimation) {
            currAnimation = newAnimation;
            animator.CrossFade(currAnimation, 0.2f);
        }
    }
}
