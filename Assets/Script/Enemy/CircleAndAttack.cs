using Game.Utils;

using UnityEngine;

public class CircleAndAttack : MovementBase {
    private enum EnemyState {
        Random,
        Chase,
        Circle,
    }

    private Transform player;
    private MeleeAttack meleeAttack;
    private FunctionTimer chaseTimer;
    private FunctionTimer circleTimer;
    private string chaseTimerName;
    private string waitTimerName;
    private string circleTimerName;

    private float elapsed = 0;
    private float delay = 1;

    private float negWeigth = 1;
    private float posWeigth = 1;
    private int attackPhase = 0;
    private int sign = 1;

    private EnemyState state;

    [SerializeField] private bool moveBackToCentre;

    [SerializeField] private float speed;
    [SerializeField] private float chaseSpeed;
    [SerializeField] private float circleSpeed;

    [SerializeField] private float randomRadius;
    [SerializeField] private float chaseRadius;
    [SerializeField] private float circleRadius;

    [SerializeField] private float waitTime;
    [SerializeField] private Transform centre;

    private float updateTime;

    private void Awake() {
        InitBasicComponents();
        waitTimerName = "WaitTimer" + gameObject.GetInstanceID();
        chaseTimerName = "ChaseTimer" + gameObject.GetInstanceID();
        circleTimerName = "CircleTimer" + gameObject.GetInstanceID();
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
            case EnemyState.Circle:
                CirclePlayer();
                break;
        }
    }

    private void UpdateState() {
        float dist = Vector2.Distance(player.transform.position, transform.position);
        if (dist <= chaseRadius && dist > circleRadius && state != EnemyState.Chase) {
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
        else if (dist <= circleRadius && state != EnemyState.Circle) {
            ClearTimers();
            agent.SetPath(null);
            seeker.StartPath(transform.position, transform.position);
            agent.maxSpeed = circleSpeed;
            state = EnemyState.Circle;

            AttackPlayer();
        }
    }

    private void ClearTimers() {
        FunctionTimer.DestroySceneTimer(waitTimerName);
        FunctionTimer.DestroySceneTimer(chaseTimerName);
        FunctionTimer.DestroySceneTimer(circleTimerName);
    }

    private void CirclePlayer() {
        if (attackPhase != 0) return;

        if (elapsed <= delay) {
            if (agent.canMove) agent.canMove = false;

            transform.RotateAround(player.position, Vector3.forward * sign, circleSpeed * 10 * Time.deltaTime);
            transform.rotation = Quaternion.Euler(0, 0, 0);
            elapsed += Time.deltaTime;
        }
        else {
            AttackPlayer();
        }
    }

    private void AttackPlayer() {
        if (!agent.canMove) agent.canMove = true;
        if (attackPhase == 0) {
            Vector2 c = player.position;
            float angle = Random.Range(-120, 120);
            Vector2 dir = Quaternion.Euler(0, 0, angle) * ((Vector2)transform.position - c).normalized;
            Vector2 target = c + dir;

            seeker.StartPath(transform.position, target);
            attackPhase = 1;

            FunctionTimer.CreateSceneTimer(() => {
                GetComponent<MeleeAttack>().Slash();

                FunctionTimer.CreateSceneTimer(() => {
                    Vector2 target = c + (dir * (circleRadius - 0.3f));
                    seeker.StartPath(transform.position, target);

                    FunctionTimer.CreateSceneTimer(() => {
                        float chance = Random.Range(0, negWeigth + posWeigth);
                        if (chance < negWeigth) { sign = -1; negWeigth++; }
                        else { sign = 1; posWeigth++; }

                        if (Mathf.Abs(negWeigth - posWeigth) >= 6) { negWeigth = 1; posWeigth = 1; }

                        attackPhase = 0;
                        elapsed = 0;
                    }, 0.3f);

                }, 0.3f);
            }, 0.3f);
        }
    }
    public override Vector2 GetMoveDir() {
        if (state == EnemyState.Circle) {
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
