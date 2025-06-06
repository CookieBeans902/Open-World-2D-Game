using UnityEngine;

public class EnemyAnimation : MonoBehaviour {
    private Animator animator;
    private MovementBase enemyMovement;
    private Vector2 currDir;
    private string currAnimation;
    private string newAnimation;
    private bool isMoving;

    [SerializeField] PlayerAnimationsSO animationData;
    private void Start() {
        enemyMovement = GetComponent<MovementBase>();
        animator = GetComponent<Animator>();
        currDir = Vector2.zero;
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
            currDir = dir;
        }
        Debug.Log(dir);
        if (!isMoving) {
            if (currDir.y < 0) newAnimation = animationData.IdleDown.name;
            else if (currDir.y > 0) newAnimation = animationData.IdleUp.name;
            else if (currDir.x > 0) newAnimation = animationData.IdleRight.name;
            else if (currDir.x < 0) newAnimation = animationData.IdleLeft.name;
        }
        else {
            if (currDir.y < 0) newAnimation = animationData.MoveDown.name;
            else if (currDir.y > 0) newAnimation = animationData.MoveUp.name;
            else if (currDir.x > 0) newAnimation = animationData.MoveRight.name;
            else if (currDir.x < 0) newAnimation = animationData.MoveLeft.name;
        }
    }

    private void UpdateAnimation() {
        if (newAnimation != currAnimation) {
            currAnimation = newAnimation;
            animator.CrossFade(currAnimation, 0.2f);
        }
    }
}
