using Game.Utils;

using Pathfinding;

using UnityEngine;

public class StayAndAttack : MovementBase {
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

    private bool isAttacking;
    private float elapsed = 0;

    private EnemyState state;

    [SerializeField] private bool moveBackToCentre;
    [SerializeField] private bool moveInSteps;

    [SerializeField] private float speed;
    [SerializeField] private float chaseSpeed;

    [SerializeField] private float randomRadius;
    [SerializeField] private float chaseRadius;
    [SerializeField] private float attackRadius;

    [SerializeField] private float attackDelay;
    [SerializeField] private float waitTime;
    [SerializeField] private float stepTime;
    [SerializeField] private Transform centre;

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

    private void HandleStates() {
        switch (state) {
            case EnemyState.Random:
                MoveRandom(centre.position, randomRadius, waitTime, waitTimerName);
                break;
            case EnemyState.Chase:
                MoveToTarget(player, updateTime, ref chaseTimer, chaseTimerName);
                break;
            case EnemyState.Attack:
                AttackPlayer();
                break;
        }

        if (moveInSteps) MoveInSteps();
    }

    private void UpdateState() {
        float dist = Vector2.Distance(player.transform.position, transform.position);
        if (dist <= chaseRadius && dist > attackRadius && state != EnemyState.Chase) {
            ClearTimers();
            agent.maxSpeed = chaseSpeed;
            agent.canMove = true;
            state = EnemyState.Chase;
        }
        else if (dist > chaseRadius && state != EnemyState.Random) {
            ClearTimers();
            agent.maxSpeed = speed;
            agent.canMove = true;
            state = EnemyState.Random;
        }
        else if (dist <= attackRadius && state != EnemyState.Attack) {
            ClearTimers();
            agent.SetPath(null);
            seeker.StartPath(transform.position, transform.position);
            state = EnemyState.Attack;

            AttackPlayer();
        }
    }

    private void MoveInSteps() {
        if (elapsed < stepTime) {
            elapsed += Time.deltaTime;
            if (elapsed > stepTime * 0.4) agent.canMove = false;

            Debug.Log(elapsed);
            Debug.Log(agent.canMove);
        }
        else {
            agent.canMove = true;
            elapsed = 0;
        }
    }

    private void AttackPlayer() {
        if (isAttacking) return;

        meleeAttack.Slash();
        agent.canMove = false;
        isAttacking = true;

        // Vector2 dir = player.position - transform.position;
        // Rigidbody2D playerRb = player.GetComponent<Rigidbody2D>();
        // playerRb.AddForce(Vector2.left * 10, ForceMode2D.Impulse);

        FunctionTimer.CreateSceneTimer(() => {
            agent.canMove = true;
            isAttacking = false;
        }, attackDelay);
    }

    private void ClearTimers() {
        FunctionTimer.DestroySceneTimer(waitTimerName);
        FunctionTimer.DestroySceneTimer(chaseTimerName);
        FunctionTimer.DestroySceneTimer(attackTimerName);
    }

    public override Vector2 GetMoveDir() {
        if (state == EnemyState.Attack) {
            Vector2 dir = player.position - transform.position;

            if (Vector2.Angle(Vector2.right, dir) <= 45)
                return Vector2.right;
            else if (Vector2.Angle(Vector2.left, dir) <= 45)
                return Vector2.left;
            else if (Vector2.Angle(Vector2.up, dir) <= 45)
                return Vector2.up;
            else if (Vector2.Angle(Vector2.down, dir) <= 45)
                return Vector2.down;
        }

        return base.GetMoveDir();
    }
}
