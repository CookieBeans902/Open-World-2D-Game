using Game.Utils;

using UnityEngine;

public class DragonBehaviour : MovementBase {
    private enum EnemyState {
        Random,
        Chase,
        Attack,
    }

    private Transform player;
    private MeleeAttack meleeAttack;
    private FunctionTimer chaseTimer;
    private string chaseTimerName;
    private string waitTimerName;
    private string attackTimerName;

    private int attackPhase = 0;
    private bool fixDir;
    private Vector2? fixPos;

    private EnemyState state;

    [SerializeField] private bool moveBackToCentre;

    [SerializeField] private float speed;
    [SerializeField] private float chaseSpeed;

    [SerializeField] private float randomRadius;
    [SerializeField] private float chaseRadius;
    [SerializeField] private float attackRadius;

    [SerializeField] private float attackDelay;
    [SerializeField] private float spread;
    [SerializeField] private float waitTime;
    [SerializeField] private Transform centre;

    [SerializeField] private Transform firePoint;
    [SerializeField] private Transform fireBreathPref;
    [SerializeField] private SpriteRenderer visual;

    private float updateTime;

    private void Awake() {
        InitBasicComponents();
        waitTimerName = "WaitTimer" + gameObject.GetInstanceID();
        chaseTimerName = "ChaseTimer" + gameObject.GetInstanceID();
        attackTimerName = "AttackTimer" + gameObject.GetInstanceID();
        state = EnemyState.Random;
        agent.maxSpeed = speed;
        updateTime = 0.02f;
    }

    private void Start() {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        meleeAttack = GetComponent<MeleeAttack>();
    }

    private void Update() {
        HandleStates();
        UpdateState();
    }

    private void FixedUpdate() {
        if (fixPos != null) GetComponent<Rigidbody2D>().MovePosition(fixPos ?? Vector2.zero);
    }

    private void HandleStates() {
        switch (state) {
            case EnemyState.Random:
                MoveRandom(centre.position, randomRadius, waitTime, waitTimerName);
                break;
            case EnemyState.Chase:
                MoveToTarget(player, updateTime, ref chaseTimer, chaseTimerName);
                break;
            case EnemyState.Attack:
                if (attackPhase != 2) {
                    float dist = Vector2.Distance(player.position, transform.position);

                    if (dist <= 2 && attackPhase != 1) attackPhase = 1;
                    if (dist > 2 && attackPhase != 0) attackPhase = 0;

                    if (attackPhase == 0) {
                        MoveToTarget(player, updateTime, ref chaseTimer, chaseTimerName);
                    }
                    else if (attackPhase == 1) {
                        float chance = Random.Range(0, 10);
                        if (chance < 4) FireBreath();
                        else AttackPlayer();
                    }
                }
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
            AttackPlayer();
        }
    }

    private void AttackPlayer() {
        float time = 0.3f;
        attackPhase = 2;

        FunctionTimer.CreateSceneTimer(() => {
            agent.SetPath(null);
            agent.canMove = false;
            ExecuteAttack();

            FunctionTimer.CreateSceneTimer(() => {
                agent.canMove = true;
                Vector2 c = player.position;
                Vector2 dir = ((Vector2)transform.position - c).normalized;
                Vector2 target = c + (dir * (attackRadius * 0.9f));
                agent.canMove = true;
                seeker.StartPath(transform.position, target);

                FunctionTimer.CreateSceneTimer(() => {
                    attackPhase = 0;
                }, time);
            }, time);
        }, time);
    }

    private void FireBreath() {
        Vector2 dir = (player.position - transform.position).normalized;
        if (Vector2.Angle(Vector2.right, dir) <= 45)
            dir = Vector2.right;
        else if (Vector2.Angle(Vector2.left, dir) <= 45)
            dir = Vector2.left;
        else if (Vector2.Angle(Vector2.up, dir) <= 45)
            dir = Vector2.up;
        else if (Vector2.Angle(Vector2.down, dir) <= 45)
            dir = Vector2.down;

        if (dir == Vector2.up)
            visual.sortingLayerID = SortingLayer.NameToID("AboveChar");

        agent.SetPath(null);
        agent.canMove = false;
        fixDir = true;
        fixPos = transform.position;

        EnemyAnimation anim = GetComponent<EnemyAnimation>();
        anim.PlayCastAnimation(dir);

        attackPhase = 2;

        FunctionTimer.CreateSceneTimer(() => {
            EnemyStats stats = GetComponent<EnemyStats>();

            FireBreath fireBreath = Instantiate(fireBreathPref).GetComponent<FireBreath>();
            fireBreath.Setup(firePoint.position, dir, stats.atk * 0.7f, stats.luck, LayerMask.GetMask("Player"));

            FunctionTimer.CreateSceneTimer(() => {
                fixDir = false;
                agent.canMove = true;
                attackPhase = 0;
                fixPos = null;
            }, fireBreath.GetAnimTime() + 0.2f);
        }, anim.GetCastAnimationTime() * 0.2f);
    }

    private void ExecuteAttack() {
        Vector2 faceDir = player.position - transform.position;
        faceDir = SnapToNearestDirection(faceDir);

        EnemyStats stats = GetComponent<EnemyStats>();
        GetComponent<MeleeAttack>().Slash(faceDir, spread, stats.atk, stats.luck, stats.pushbackForce);
    }

    private void ClearTimers() {
        FunctionTimer.DestroySceneTimer(waitTimerName);
        FunctionTimer.DestroySceneTimer(chaseTimerName);
        FunctionTimer.DestroySceneTimer(attackTimerName);
    }

    public override Vector2 GetMoveDir() {
        if (state == EnemyState.Attack) {
            if (fixDir) return Vector2.zero;

            Vector2 dir = player.position - transform.position;
            dir = SnapToNearestDirection(dir);

            return dir;
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
}
