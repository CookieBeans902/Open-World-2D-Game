using UnityEngine;

public class MeleeAttack : AttackBase {
    private EnemyAnimation anim;

    private void Start() {

    }

    public void Slash(Vector2 dir, float spread) {
        if (dir == Vector2.zero) return;

        EnemyAnimation anim = GetComponent<EnemyAnimation>();
        int layerMask = LayerMask.GetMask("Player");

        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        rb.AddForce(dir * 3, ForceMode2D.Impulse);

        PerformSlash(dir, spread, layerMask);
        anim.PlaySlashAnimation(dir);
    }

    public void Thrust(Vector2 dir, float spread) {
        if (dir == Vector2.zero) return;

        EnemyAnimation anim = GetComponent<EnemyAnimation>();
        int layerMask = LayerMask.GetMask("Player");

        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        rb.AddForce(dir * 3, ForceMode2D.Impulse);

        PerformThrust(dir, spread, layerMask);
        anim.PlayThrustAnimation(dir);
    }
}
