using Game.Utils;

using Pathfinding;

using UnityEngine;

public class StandAndAttack : MovementBase {
    private enum EnemyState {
        Random,
        Chase,
        Attack,
    }

    public enum AttackType {
        Slash,
        Thrust,
    }

    private Transform player;

    private FunctionTimer chaseTimer;
    private string chaseTimerName;
    private string waitTimerName;
    private string attackTimerName;

    private bool isAttacking;

    private EnemyState state;
    private Vector2 prevPos;

    [SerializeField] private AttackType attackType;

    [SerializeField] private float speed;
    [SerializeField] private float chaseSpeed;

    [SerializeField] private float randomRadius;
    [SerializeField] private float chaseRadius;
    [SerializeField] private float attackRadius;

    [SerializeField] private float attackDelay;
    [SerializeField] private float spread;
    [SerializeField] private float range;
    [SerializeField] private float waitTime;
    [SerializeField] private Transform centre;

    private float lastPathUpdateTime;
    private float pathUpdateCooldown = 0.5f;
    private Vector2 lastPlayerPos;

    private void Awake() {
        InitBasicComponents();
        waitTimerName = "WaitTimer" + gameObject.GetInstanceID();
        chaseTimerName = "ChaseTimer" + gameObject.GetInstanceID();
        attackTimerName = "AttackTimer" + gameObject.GetInstanceID();
        state = EnemyState.Random;
        agent.maxSpeed = speed;
        prevPos = transform.position;
    }

    private void Start() {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        lastPlayerPos = player.position;
    }

    private void Update() {
        HandleStates();
        UpdateState();
    }

    private void HandleStates() {
        switch (state) {
            case EnemyState.Random:
                MoveRandom(centre.position, randomRadius, waitTime, waitTimerName);
                break;

            case EnemyState.Chase:
                if (Time.time - lastPathUpdateTime >= pathUpdateCooldown &&
                    Vector2.Distance(player.position, lastPlayerPos) > 0.5f) {
                    MoveToTarget(player, 0f, ref chaseTimer, chaseTimerName);
                    lastPathUpdateTime = Time.time;
                    lastPlayerPos = player.position;
                }
                break;

            case EnemyState.Attack:
                if (!isAttacking) AttackPlayer();
                break;
        }
    }

    private void UpdateState() {
        float dist = Vector2.Distance(player.position, transform.position);
        float buffer = 0.1f;

        if (dist > chaseRadius + buffer && state != EnemyState.Random) {
            ClearTimers();
            agent.maxSpeed = speed;
            agent.canMove = true;
            state = EnemyState.Random;
        }
        else if (dist <= chaseRadius - buffer && dist > attackRadius + buffer && state != EnemyState.Chase) {
            ClearTimers();
            agent.maxSpeed = chaseSpeed;
            agent.canMove = true;
            state = EnemyState.Chase;
        }
        else if (dist <= attackRadius - buffer && state != EnemyState.Attack) {
            ClearTimers();
            agent.SetPath(null);
            seeker.StartPath(transform.position, transform.position);
            state = EnemyState.Attack;
            prevPos = transform.position;
            AttackPlayer();
        }
    }

    private void AttackPlayer() {
        isAttacking = true;
        ExecuteAttack();
    }

    private void ExecuteAttack() {
        Vector2 faceDir = SnapToNearestDirection(player.position - transform.position);

        agent.SetPath(null);
        agent.canMove = false;

        EnemyAnimation anim = GetComponent<EnemyAnimation>();
        EnemyStats stats = GetComponent<EnemyStats>();
        var attack = GetComponent<MeleeAttack>();

        if (attackType == AttackType.Slash) attack.Slash(faceDir, spread, stats.atk, stats.luck, stats.pushbackForce);
        else attack.Thrust(faceDir, range, stats.atk, stats.luck, stats.pushbackForce);

        float animTime = attackType == AttackType.Slash ? anim.GetSlashAnimationTime() : anim.GetThrustAnimationTime();

        FunctionTimer.CreateSceneTimer(() => {
            Vector2 retreatDir = (transform.position - player.position).normalized;
            agent.canMove = true;

            float diff = attackRadius - (prevPos - (Vector2)player.position).magnitude;
            Vector2 pos = prevPos + retreatDir * ((diff > 0 ? diff : 0) - 0.2f);

            seeker.StartPath(transform.position, pos);

            float time = (pos - (Vector2)transform.position).magnitude / speed;
            FunctionTimer.CreateSceneTimer(() => isAttacking = false, time + 0.2f);
        }, animTime, attackTimerName);
    }

    public override Vector2 GetMoveDir() {
        if (state == EnemyState.Attack) {
            Vector2 dir = player.position - transform.position;
            return SnapToNearestDirection(dir);
        }

        return base.GetMoveDir();
    }

    private Vector2 SnapToNearestDirection(Vector2 dir) {
        dir.Normalize();
        Vector2[] directions = { Vector2.right, Vector2.left, Vector2.up, Vector2.down };
        float maxDot = float.MinValue;
        Vector2 bestDir = Vector2.right;

        foreach (var d in directions) {
            float dot = Vector2.Dot(dir, d);
            if (dot > maxDot) {
                maxDot = dot;
                bestDir = d;
            }
        }

        return bestDir;
    }

    private void ClearTimers() {
        FunctionTimer.DestroySceneTimer(waitTimerName);
        FunctionTimer.DestroySceneTimer(chaseTimerName);
        FunctionTimer.DestroySceneTimer(attackTimerName);
    }
}
