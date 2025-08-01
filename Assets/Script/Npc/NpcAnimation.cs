using Game.Utils;

using UnityEngine;

public class NpcAnimation : MonoBehaviour {
    private MovementBase npcMovement;
    private Vector2 prevDir;
    private string currAnimation;
    private string newAnimation;
    private bool isMoving;

    [SerializeField] private Animator animator;
    [SerializeField] private WalkAnimationNameSO animationData;
    private void Start() {
        npcMovement = GetComponent<MovementBase>();
        prevDir = Vector2.right;
    }

    private void Update() {
        HandleWalkAnimation();

        UpdateAnimation();
    }

    private void HandleWalkAnimation() {
        Vector2 dir = npcMovement.GetMoveDir();
        isMoving = dir != Vector2.zero;
        if (!npcMovement.canMove) return;

        if (!isMoving) {
            if (prevDir == Vector2.down) newAnimation = animationData.IdleDown;
            else if (prevDir == Vector2.up) newAnimation = animationData.IdleUp;
            else if (prevDir == Vector2.right) newAnimation = animationData.IdleRight;
            else if (prevDir == Vector2.left) newAnimation = animationData.IdleLeft;
        }
        else {
            if (dir == Vector2.down) newAnimation = animationData.WalkDown;
            else if (dir == Vector2.up) newAnimation = animationData.WalkUp;
            else if (dir == Vector2.right) newAnimation = animationData.WalkRight;
            else if (dir == Vector2.left) newAnimation = animationData.WalkLeft;
            prevDir = dir;
        }
    }

    private void UpdateAnimation() {
        if (newAnimation != currAnimation) {
            currAnimation = newAnimation;
            animator.CrossFade(currAnimation, 0.2f, 0);
        }
    }
}
