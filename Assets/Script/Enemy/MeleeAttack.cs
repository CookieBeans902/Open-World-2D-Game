using Game.Utils;

using UnityEngine;

public class MeleeAttack : AttackBase {
    public void Slash(Vector2 dir, float spread, float atk, float luck, float forceMag) {
        if (dir == Vector2.zero) return;

        EnemyAnimation anim = GetComponent<EnemyAnimation>();

        anim.PlaySlashAnimation(dir);
        FunctionTimer.CreateSceneTimer(() => {
            int layerMask = LayerMask.GetMask("Player");
            PerformSlash(dir, spread, atk, luck, layerMask, forceMag);
        }, anim.GetSlashAnimationTime() * 0.6f);
    }

    public void Thrust(Vector2 dir, float range, float atk, float luck, float forceMag) {
        if (dir == Vector2.zero) return;

        EnemyAnimation anim = GetComponent<EnemyAnimation>();

        anim.PlayThrustAnimation(dir);
        FunctionTimer.CreateSceneTimer(() => {
            int layerMask = LayerMask.GetMask("Player");
            PerformThrust(dir, range, atk, luck, layerMask, forceMag);
        }, anim.GetThrustAnimationTime() * 0.6f);
    }
}
