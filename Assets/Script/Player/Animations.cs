using System;

using Game.Utils;

using UnityEngine;

public class Animations : MonoBehaviour {
    [SerializeField] AnimationsSO playerAnimations;

    private GameInputManager input;
    private PlayerShared shared;
    private string currAnimation;
    private string newAnimation;
    private bool isMoving;
    private bool isAttacking;
    private void Start() {
        input = GameInputManager.Instance;
        shared = GetComponent<PlayerShared>();
    }

    private void Update() {
        HandleMoveAnimation();

        UpdateAnimation();
    }

    private void HandleMoveAnimation() {
        if (!shared.move.canMove) return;
        if (isAttacking) return;

        isMoving = input.GetMovementVectorNormalized() != Vector2.zero;
        Vector2 dir = shared.move.playerDir;
        if (!isMoving) {
            if (dir == Vector2.right) {
                newAnimation = playerAnimations.IdleRight.name;
            }
            else if (dir == Vector2.left) {
                newAnimation = playerAnimations.IdleLeft.name;
            }
            else if (dir == Vector2.up) {
                newAnimation = playerAnimations.IdleUp.name;
            }
            else if (dir == Vector2.down) {
                newAnimation = playerAnimations.IdleDown.name;
            }
        }
        else {
            if (dir == Vector2.right) {
                newAnimation = playerAnimations.MoveRight.name;
            }
            else if (dir == Vector2.left) {
                newAnimation = playerAnimations.MoveLeft.name;
            }
            else if (dir == Vector2.up) {
                newAnimation = playerAnimations.MoveUp.name;
            }
            else if (dir == Vector2.down) {
                newAnimation = playerAnimations.MoveDown.name;
            }
        }
    }

    public void PlaySlashAnimation(float waitTime) {
        shared.move.DisableMovement();
        Vector2 dir = shared.move.playerDir;
        float animTime = 0;

        if (dir == Vector2.right) {
            newAnimation = playerAnimations.SlashRight.name;
        }
        else if (dir == Vector2.left) {
            newAnimation = playerAnimations.SlashLeft.name;
        }
        else if (dir == Vector2.up) {
            newAnimation = playerAnimations.SlashUp.name;
        }
        else if (dir == Vector2.down) {
            newAnimation = playerAnimations.SlashDown.name;
        }

        FunctionTimer.CreateSceneTimer(() => {
            shared.move.EnableMovement();
        }, animTime + waitTime);
    }

    public void PlayISlashAnimation(float waitTime) {
        shared.move.DisableMovement();
        Vector2 dir = shared.move.playerDir;
        float animTime = 0;
        isAttacking = true;

        if (dir == Vector2.right) {
            newAnimation = playerAnimations.ISlashRight.name;
        }
        else if (dir == Vector2.left) {
            newAnimation = playerAnimations.ISlashLeft.name;
        }
        else if (dir == Vector2.up) {
            newAnimation = playerAnimations.ISlashUp.name;
        }
        else if (dir == Vector2.down) {
            newAnimation = playerAnimations.ISlashDown.name;
        }

        FunctionTimer.CreateSceneTimer(() => {
            shared.move.EnableMovement();
            isAttacking = false;
        }, animTime);
    }

    public void PlayThrustAnimation(float waitTime) {
        shared.move.DisableMovement();
        Vector2 dir = shared.move.playerDir;
        float animTime = 0;
        isAttacking = true;

        if (dir == Vector2.right) {
            newAnimation = playerAnimations.ThrustRight.name;
        }
        else if (dir == Vector2.left) {
            newAnimation = playerAnimations.ThrustLeft.name;
        }
        else if (dir == Vector2.up) {
            newAnimation = playerAnimations.ThrustUp.name;
        }
        else if (dir == Vector2.down) {
            newAnimation = playerAnimations.ThrustDown.name;
        }

        FunctionTimer.CreateSceneTimer(() => {
            shared.move.EnableMovement();
            isAttacking = false;
        }, animTime);
    }

    public void PlayShootAnimation(float waitTime, Action animDone) {
        shared.move.DisableMovement();
        Vector2 dir = shared.move.playerDir;
        float animTime = 0;

        if (dir == Vector2.right) {
            newAnimation = playerAnimations.ShootRight.name;
        }
        else if (dir == Vector2.left) {
            newAnimation = playerAnimations.ShootLeft.name;
        }
        else if (dir == Vector2.up) {
            newAnimation = playerAnimations.ShootUp.name;
        }
        else if (dir == Vector2.down) {
            newAnimation = playerAnimations.ShootDown.name;
        }

        FunctionTimer.CreateSceneTimer(() => {
            animDone();
        }, animTime - 0.12f);
        FunctionTimer.CreateSceneTimer(() => {
            shared.move.EnableMovement();
        }, animTime + waitTime);
    }

    private void UpdateAnimation() {
        if (newAnimation != currAnimation) {
            currAnimation = newAnimation;
            shared.animator.CrossFade(currAnimation, 0.2f, 0);
        }
    }

    public float GetSlashAnimationTime() {
        return playerAnimations.SlashDown?.length ?? 0;
    }

    public float GetThrustAnimationTime() {
        return playerAnimations.ThrustDown?.length ?? 0;
    }
}
