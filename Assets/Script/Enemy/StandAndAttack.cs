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
    private MeleeAttack meleeAttack;
    private FunctionTimer chaseTimer;
    private string chaseTimerName;
    private string waitTimerName;
    private string attackTimerName;

    private int attackPhase = 0;
    private bool isAttacking;
    private float elapsed = 0;

    private EnemyState state;

    [SerializeField] private AttackType attackType;
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
                if (attackPhase != 2) {
                    float dist = Vector2.Distance(player.position, transform.position);

                    if (dist <= 2 && attackPhase != 1) attackPhase = 1;
                    if (dist > 2 && attackPhase != 0) attackPhase = 0;

                    if (attackPhase == 0) {
                        MoveToTarget(player, updateTime, ref chaseTimer, chaseTimerName);
                    }
                    else if (attackPhase == 1) {
                        AttackPlayer();
                    }
                }
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

    private void ExecuteAttack() {
        Vector2 faceDir = player.position - transform.position;

        if (Vector2.Angle(Vector2.right, faceDir) <= 45)
            faceDir = Vector2.right;
        else if (Vector2.Angle(Vector2.left, faceDir) <= 45)
            faceDir = Vector2.left;
        else if (Vector2.Angle(Vector2.up, faceDir) <= 45)
            faceDir = Vector2.up;
        else if (Vector2.Angle(Vector2.down, faceDir) <= 45)
            faceDir = Vector2.down;

        if (attackType == AttackType.Slash) GetComponent<MeleeAttack>().Slash(faceDir, 2);
        else GetComponent<MeleeAttack>().Thrust(faceDir, 2);
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
