using UnityEngine;

public class EnemyAnimation : MonoBehaviour {
    private MovementBase enemyMovement;
    private Vector2 prevDir;
    private Vector2 curDir;
    private string currAnimation;
    private string newAnimation;
    private bool isMoving;

    [SerializeField] private Animator animator;
    [SerializeField] private AnimationsSO animationData;
    private void Start() {
        enemyMovement = GetComponent<MovementBase>();
        curDir = Vector2.right;
        prevDir = Vector2.right;
    }

    private void Update() {
        HandleMoveAnimation();

        UpdateAnimation();
    }

    private void HandleMoveAnimation() {
        Vector2 dir = enemyMovement.GetMoveDir();
        isMoving = dir != Vector2.zero;
        if (isMoving) {
            if (Mathf.Abs(dir.x) > Mathf.Abs(dir.y)) dir.y = 0;
            else dir.x = 0;
            curDir = dir.normalized;
        }
        if (dir != Vector2.zero) prevDir = curDir;
        Debug.Log(dir);
        // Debug.Log(dir);
        if (!isMoving) {
            if (prevDir == Vector2.down) newAnimation = animationData.IdleDown.name;
            else if (prevDir == Vector2.up) newAnimation = animationData.IdleUp.name;
            else if (prevDir == Vector2.right) newAnimation = animationData.IdleRight.name;
            else if (prevDir == Vector2.left) newAnimation = animationData.IdleLeft.name;
        }
        else {
            if (curDir == Vector2.down) newAnimation = animationData.MoveDown.name;
            else if (curDir == Vector2.up) newAnimation = animationData.MoveUp.name;
            else if (curDir == Vector2.right) newAnimation = animationData.MoveRight.name;
            else if (curDir == Vector2.left) newAnimation = animationData.MoveLeft.name;
        }
    }

    private void UpdateAnimation() {
        if (newAnimation != currAnimation) {
            currAnimation = newAnimation;
            animator.CrossFade(currAnimation, 0.2f);
        }
    }
}
