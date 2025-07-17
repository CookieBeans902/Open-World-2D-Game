using System;

using Game.Utils;

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
        if (!shared.playerMove.canMove) return;

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
        }
    }

    public void PlaySlashAnimation(float waitTime) {
        shared.playerMove.DisableMovement();
        PlayerDirection dir = ConvertToEnum(shared.playerMove.playerDir);
        float animTime = 0;

        switch (dir) {
            case PlayerDirection.Right:
                newAnimation = playerAnimations.SlashRight.name;
                animTime = playerAnimations.SlashRight.length;
                break;
            case PlayerDirection.Left:
                newAnimation = playerAnimations.SlashLeft.name;
                animTime = playerAnimations.SlashLeft.length;
                break;
            case PlayerDirection.Down:
                newAnimation = playerAnimations.SlashDown.name;
                animTime = playerAnimations.SlashDown.length;
                break;
            case PlayerDirection.Up:
                newAnimation = playerAnimations.SlashUp.name;
                animTime = playerAnimations.SlashUp.length;
                break;
        }

        FunctionTimer.CreateSceneTimer(() => {
            shared.playerMove.EnableMovement();
        }, animTime + waitTime);
    }

    public void PlayISlashAnimation(float waitTime) {
        shared.playerMove.DisableMovement();
        PlayerDirection dir = ConvertToEnum(shared.playerMove.playerDir);
        float animTime = 0;

        switch (dir) {
            case PlayerDirection.Right:
                newAnimation = playerAnimations.ISlashRight.name;
                animTime = playerAnimations.ISlashRight.length;
                break;
            case PlayerDirection.Left:
                newAnimation = playerAnimations.ISlashLeft.name;
                animTime = playerAnimations.ISlashLeft.length;
                break;
            case PlayerDirection.Down:
                newAnimation = playerAnimations.ISlashDown.name;
                animTime = playerAnimations.ISlashDown.length;
                break;
            case PlayerDirection.Up:
                newAnimation = playerAnimations.ISlashUp.name;
                animTime = playerAnimations.ISlashUp.length;
                break;
        }

        FunctionTimer.CreateSceneTimer(() => {
            shared.playerMove.EnableMovement();
        }, animTime + waitTime);
    }

    public void PlayThrustAnimation(float waitTime) {
        shared.playerMove.DisableMovement();
        PlayerDirection dir = ConvertToEnum(shared.playerMove.playerDir);
        float animTime = 0;

        switch (dir) {
            case PlayerDirection.Right:
                newAnimation = playerAnimations.ThrustRight.name;
                animTime = playerAnimations.ThrustRight.length;
                break;
            case PlayerDirection.Left:
                newAnimation = playerAnimations.ThrustLeft.name;
                animTime = playerAnimations.ThrustLeft.length;
                break;
            case PlayerDirection.Down:
                newAnimation = playerAnimations.ThrustDown.name;
                animTime = playerAnimations.ThrustDown.length;
                break;
            case PlayerDirection.Up:
                newAnimation = playerAnimations.ThrustUp.name;
                animTime = playerAnimations.ThrustUp.length;
                break;
        }

        FunctionTimer.CreateSceneTimer(() => {
            shared.playerMove.EnableMovement();
        }, animTime + waitTime);
    }

    public void PlayShootAnimation(float waitTime, Action animDone) {
        shared.playerMove.DisableMovement();
        PlayerDirection dir = ConvertToEnum(shared.playerMove.playerDir);
        float animTime = 0;

        switch (dir) {
            case PlayerDirection.Right:
                newAnimation = playerAnimations.ShootRight.name;
                animTime = playerAnimations.ShootRight.length;
                break;
            case PlayerDirection.Left:
                newAnimation = playerAnimations.ShootLeft.name;
                animTime = playerAnimations.ShootLeft.length;
                break;
            case PlayerDirection.Down:
                newAnimation = playerAnimations.ShootDown.name;
                animTime = playerAnimations.ShootDown.length;
                break;
            case PlayerDirection.Up:
                newAnimation = playerAnimations.ShootUp.name;
                animTime = playerAnimations.ShootUp.length;
                break;
        }

        FunctionTimer.CreateSceneTimer(() => {
            animDone();
        }, animTime - 0.12f);
        FunctionTimer.CreateSceneTimer(() => {
            shared.playerMove.EnableMovement();
        }, animTime + waitTime);
    }

    private void UpdateAnimation() {
        if (newAnimation != currAnimation) {
            currAnimation = newAnimation;
            shared.animator.CrossFade(currAnimation, 0.2f, 0);
        }
    }

    private PlayerDirection ConvertToEnum(Vector2 dir) {
        if (dir == Vector2.left) return PlayerDirection.Left;
        else if (dir == Vector2.right) return PlayerDirection.Right;
        else if (dir == Vector2.down) return PlayerDirection.Down;
        else return PlayerDirection.Up;
    }
}
