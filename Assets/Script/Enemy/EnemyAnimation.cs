using Game.Utils;

using UnityEngine;

public class EnemyAnimation : MonoBehaviour {
    private MovementBase enemyMovement;
    private Vector2 prevDir;
    private string currAnimation;
    private string newAnimation;
    private bool isMoving;
    private bool isAttacking;
    private bool isDefeated;

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
        if (isDefeated) return;
        if (isAttacking) return;

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

    public void PlaySlashAnimation(Vector2 dir) {
        if (isAttacking) return;
        isAttacking = true;

        float animTime = 0;

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
            isAttacking = false;
        }, animTime);
    }

    public void PlayThrustAnimation(Vector2 dir) {
        if (isAttacking) return;
        isAttacking = true;

        float animTime = 0;

        if (dir == Vector2.right) {
            newAnimation = animationData.ThrustRight.name;
            animTime = animationData.ThrustRight.length;
        }
        else if (dir == Vector2.left) {
            newAnimation = animationData.ThrustLeft.name;
            animTime = animationData.ThrustLeft.length;
        }
        else if (dir == Vector2.down) {
            newAnimation = animationData.ThrustDown.name;
            animTime = animationData.ThrustDown.length;
        }
        else if (dir == Vector2.up) {
            newAnimation = animationData.ThrustUp.name;
            animTime = animationData.ThrustUp.length;
        }

        FunctionTimer.CreateSceneTimer(() => {
            isAttacking = false;
        }, animTime);
    }

    public void PlayShootAnimation(Vector2 dir) {
        if (isAttacking) return;
        isAttacking = true;

        float animTime = 0;
        if (dir != Vector2.zero) prevDir = dir;

        if (dir == Vector2.right) {
            newAnimation = animationData.ShootRight.name;
            animTime = animationData.ShootRight.length;
        }
        else if (dir == Vector2.left) {
            newAnimation = animationData.ShootLeft.name;
            animTime = animationData.ShootLeft.length;
        }
        else if (dir == Vector2.down) {
            newAnimation = animationData.ShootDown.name;
            animTime = animationData.ShootDown.length;
        }
        else if (dir == Vector2.up) {
            newAnimation = animationData.ShootUp.name;
            animTime = animationData.ShootUp.length;
        }

        FunctionTimer.CreateSceneTimer(() => {
            isAttacking = false;
        }, animTime);
    }

    public void PlayCastAnimation(Vector2 dir) {
        if (isAttacking) return;
        isAttacking = true;

        float animTime = 0;

        if (dir == Vector2.right) {
            newAnimation = animationData.CastRight.name;
            animTime = animationData.CastRight.length;
        }
        else if (dir == Vector2.left) {
            newAnimation = animationData.CastLeft.name;
            animTime = animationData.CastLeft.length;
        }
        else if (dir == Vector2.down) {
            newAnimation = animationData.CastDown.name;
            animTime = animationData.CastDown.length;
        }
        else if (dir == Vector2.up) {
            newAnimation = animationData.CastUp.name;
            animTime = animationData.CastUp.length;
        }

        FunctionTimer.CreateSceneTimer(() => {
            isAttacking = false;
        }, animTime);
    }

    public void PlayHurtAnimation() {
        if (isDefeated) return;
        isDefeated = true;

        newAnimation = animationData.Hurt.name;
    }

    private void UpdateAnimation() {
        if (newAnimation != currAnimation) {
            currAnimation = newAnimation;
            animator.CrossFade(currAnimation, 0.2f);
        }
    }

    public float GetShootAnimationTime() {
        return animationData.ShootDown?.length ?? 0;
    }

    public float GetSlashAnimationTime() {
        return animationData.SlashDown?.length ?? 0;
    }

    public float GetThrustAnimationTime() {
        return animationData.ThrustDown?.length ?? 0;
    }

    public float GetCastAnimationTime() {
        return animationData.CastDown?.length ?? 0;
    }
    public float GetHurtAnimationTime() {
        return animationData.Hurt?.length ?? 0;
    }
}
