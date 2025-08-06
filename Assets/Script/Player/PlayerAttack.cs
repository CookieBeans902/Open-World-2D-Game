using System.Security.Cryptography;

using Game.Utils;

using UnityEngine;

public class PlayerAttack : AttackBase {
    [SerializeField] private bool isRanged;
    [SerializeField] private float waitTime;
    [SerializeField] private GameObject arrowPref;

    [SerializeField] private Transform shoot_up;
    [SerializeField] private Transform shoot_down;
    [SerializeField] private Transform shoot_left;
    [SerializeField] private Transform shoot_right;

    private PlayerShared shared;
    private bool isAttacking;
    private int state = 1;

    private void Start() {
        shared = GetComponent<PlayerShared>();
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Space)) {
            // if (isRanged) Shoot();
            Slash();
        }
    }

    private void Slash() {
        if (isAttacking) return;

        Character charData = CharacterManager.Instance?.characters[shared.charId];
        if (charData == null) return;

        Vector2 dir = shared.move.playerDir;
        shared.rb.AddForce(dir * 10, ForceMode2D.Impulse);
        float spread = 1.5f;
        float range = 1.8f;
        int layerMask = LayerMask.GetMask("Enemy");
        switch (state) {
            case 1:
                PerformSlash(dir, spread, charData.ATK, charData.LUCK, LayerMask.GetMask("Enemy"));
                shared.anim.PlaySlashAnimation(waitTime);
                isAttacking = true;
                state = 2;

                FunctionTimer.CreateSceneTimer(() => {
                    isAttacking = false;
                }, shared.anim.GetSlashAnimationTime());
                break;
            case 2:
                PerformSlash(dir, spread, charData.ATK, charData.LUCK, LayerMask.GetMask("Enemy"));
                shared.anim.PlayISlashAnimation(waitTime);
                isAttacking = true;
                state = 3;

                FunctionTimer.CreateSceneTimer(() => {
                    isAttacking = false;
                }, shared.anim.GetSlashAnimationTime());
                break;
            case 3:
                PerformThrust(dir, range, charData.ATK, charData.LUCK, LayerMask.GetMask("Enemy"));
                shared.anim.PlayThrustAnimation(waitTime);
                isAttacking = true;
                state = 1;

                FunctionTimer.CreateSceneTimer(() => {
                    isAttacking = false;
                }, shared.anim.GetThrustAnimationTime());
                break;
        }
    }

    // private void Shoot() {
    //     if (!shared.playerMove.canMove) return;
    //     Vector2 dir = shared.playerMove.playerDir;
    //     float speed = 12;
    //     float range = 16;
    //     Vector3 start = Vector3.zero;

    //     if (dir == Vector2.right) start = shoot_right.position;
    //     else if (dir == Vector2.left) start = shoot_left.position;
    //     else if (dir == Vector2.up) start = shoot_up.position;
    //     else start = shoot_down.position;
    //     shared.playerAnim.PlayShootAnimation(
    //     waitTime,
    //     () => PerformShoot(arrowPref, dir * speed, start, range, LayerMask.GetMask("Enemy")));
    // }
}
