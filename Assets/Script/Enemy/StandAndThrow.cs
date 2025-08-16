using Game.Utils;

using UnityEngine;

public class StandAndThrow : MovementBase {
    private enum EnemyState {
        Random,
        Chase,
        Run,
        Throw,
    }

    private Transform player;
    private FunctionTimer runTimer;
    private FunctionTimer chaseTimer;
    private string runTimerName;
    private string chaseTimerName;
    private string waitTimerName;
    private string attackTimerName;

    private float elapsed = 0;

    private EnemyState state;
    private Vector2 prevDir;

    [SerializeField] private float speed;
    [SerializeField] private float runSpeed;

    [SerializeField] private float randomRadius;
    [SerializeField] private float chaseRadius;
    [SerializeField] private float throwRadius;
    [SerializeField] private float runRadius;

    [SerializeField] private float waitTime;
    [SerializeField] private float throwTime;
    [SerializeField] private Transform centre;

    [SerializeField] private Transform hand;
    [SerializeField] private Transform boomarangPref;

    [SerializeField] private SpriteRenderer visual;

    private float updateTime;

    private void Awake() {
        InitBasicComponents();
        waitTimerName = "WaitTimer" + gameObject.GetInstanceID();
        runTimerName = "RunTimer" + gameObject.GetInstanceID();
        chaseTimerName = "ChaseTimer" + gameObject.GetInstanceID();
        attackTimerName = "AttackTimer" + gameObject.GetInstanceID();
        state = EnemyState.Random;
        agent.maxSpeed = speed;
        updateTime = 0.02f;
    }

    private void Start() {
        player = GameObject.FindGameObjectWithTag("Player").transform;
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
                MoveToTarget(player, updateTime, ref chaseTimer, chaseTimerName);
                break;
            case EnemyState.Run:
                float dist = Vector2.Distance(player.position, transform.position);
                float extraSpeed = 1 / (dist != 0 ? dist : 0.1f);
                extraSpeed = Mathf.Clamp(extraSpeed, 0, runSpeed * 0.4f);
                agent.maxSpeed = runSpeed + extraSpeed;

                RunFromTarget(player, runRadius, updateTime, ref runTimer, runTimerName);
                break;
            case EnemyState.Throw:
                if (agent.canMove) agent.canMove = false;
                Throw();
                break;
        }
    }

    private void UpdateState() {
        float dist = Vector2.Distance(player.transform.position, transform.position);
        float buffer = 0.1f;

        if (dist > chaseRadius + buffer && state != EnemyState.Random) {
            ClearTimers();
            seeker.StartPath(transform.position, transform.position);
            agent.maxSpeed = speed;
            agent.canMove = true;
            state = EnemyState.Random;
        }
        else if (dist < chaseRadius - buffer && dist > throwRadius + buffer && state != EnemyState.Chase) {
            ClearTimers();
            agent.maxSpeed = runSpeed;
            seeker.StartPath(transform.position, transform.position);
            agent.canMove = true;
            state = EnemyState.Chase;
        }
        else if (dist <= throwRadius - buffer && dist > runRadius + buffer && state != EnemyState.Throw) {
            Rigidbody2D rb = GetComponent<Rigidbody2D>();
            rb.linearVelocity = Vector2.zero;
            elapsed = 0;

            ClearTimers();
            agent.maxSpeed = speed;
            seeker.StartPath(transform.position, transform.position);
            agent.canMove = true;
            state = EnemyState.Throw;
        }
        else if (dist <= runRadius - buffer && state != EnemyState.Run) {
            ClearTimers();
            agent.maxSpeed = runSpeed;
            seeker.StartPath(transform.position, transform.position);
            agent.canMove = true;
            state = EnemyState.Run;
        }
    }

    private void Throw() {
        if (elapsed < throwTime) {
            elapsed += Time.deltaTime;
        }
        else {
            Vector2 dir = player.position - transform.position;
            dir = SnapToNearestDirection(dir);

            EnemyAnimation anim = GetComponent<EnemyAnimation>();
            anim.PlayShootAnimation(dir);

            FunctionTimer.CreateSceneTimer(() => {
                Projectile boomarang = Instantiate(boomarangPref).GetComponent<Projectile>();
                boomarang.transform.position = hand.GetChild(0).position;

                Vector2 dir = (player.position - hand.GetChild(0).position).normalized;
                EnemyStats stats = GetComponent<EnemyStats>();

                boomarang.Setup(dir, 14, 12, stats.atk, stats.luck, LayerMask.GetMask("Player"));
            }, anim.GetShootAnimationTime() * 0.3f, attackTimerName);

            elapsed = 0;
        }
    }

    public override Vector2 GetMoveDir() {
        if (state == EnemyState.Throw) {
            Vector2 dir = player.position - transform.position;
            Vector2 fixedDir = SnapToNearestDirection(dir);

            if (prevDir != fixedDir) {
                prevDir = fixedDir;
                return fixedDir;
            }
            else {
                return Vector2.zero;
            }
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
        FunctionTimer.DestroySceneTimer(runTimerName);
        FunctionTimer.DestroySceneTimer(chaseTimerName);
        FunctionTimer.DestroySceneTimer(attackTimerName);
    }
}
