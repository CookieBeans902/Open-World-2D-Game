using System.Security.Cryptography;

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
    private int state = 1;

    private void Start() {
        shared = GetComponent<PlayerShared>();
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Space)) {
            if (isRanged) Shoot();
            else Slash();
        }
    }

    private void Slash() {
        if (!shared.playerMove.canMove) return;
        Vector2 dir = shared.playerMove.playerDir;
        float spread = 1.5f;
        float range = 1.8f;
        int layerMask = LayerMask.GetMask("Enemy");
        switch (state) {
            case 1:
                PerformSlash(dir, spread, layerMask);
                shared.playerAnim.PlaySlashAnimation(waitTime);
                state++;
                break;
            case 2:
                PerformSlash(dir, spread, layerMask);
                shared.playerAnim.PlayISlashAnimation(waitTime);
                state++;
                break;
            case 3:
                PerformThrust(dir, range, layerMask);
                shared.playerAnim.PlayThrustAnimation(waitTime);
                state = 1;
                break;
        }
    }

    private void Shoot() {
        if (!shared.playerMove.canMove) return;
        Vector2 dir = shared.playerMove.playerDir;
        float speed = 12;
        float range = 16;
        Vector3 start = Vector3.zero;

        if (dir == Vector2.right) start = shoot_right.position;
        else if (dir == Vector2.left) start = shoot_left.position;
        else if (dir == Vector2.up) start = shoot_up.position;
        else start = shoot_down.position;
        shared.playerAnim.PlayShootAnimation(
        waitTime,
        () => PerformShoot(arrowPref, dir * speed, start, range, LayerMask.GetMask("Enemy")));
    }
}
