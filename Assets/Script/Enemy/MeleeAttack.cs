using UnityEngine;

public class MeleeAttack : AttackBase {
    [SerializeField] private float spread;
    [SerializeField] private float waitTime;
    public bool canAttack;
    private MovementBase movement;
    private EnemyAnimation anim;

    private void Start() {
        movement = GetComponent<MovementBase>();
        anim = GetComponent<EnemyAnimation>();
    }

    private void Update() {
        if (!canAttack || !movement.canMove) return;

        Vector2 dir = movement.GetMoveDir();
        if (dir == Vector2.zero) return;
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        rb.AddForce(dir * 3, ForceMode2D.Impulse);

        int layerMask = LayerMask.GetMask("Player");

        PerformSlash(dir, spread, layerMask);
        anim.PlaySlashAnimation(waitTime);
    }
}
