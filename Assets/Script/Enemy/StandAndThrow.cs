using Game.Utils;

using UnityEngine;

public class StandAndThrow : MovementBase {
    private enum EnemyState {
        Random,
        Chase,
        Run,
        Attack,
    }

    private Transform player;
    private MeleeAttack meleeAttack;
    private FunctionTimer runTimer;
    private FunctionTimer chaseTimer;
    private string runTimerName;
    private string chaseTimerName;
    private string waitTimerName;

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
            case EnemyState.Run:
                float dist = Vector2.Distance(player.position, transform.position);
                float extraSpeed = 1 / (dist != 0 ? dist : 0.1f);
                extraSpeed = Mathf.Clamp(extraSpeed, 0, runSpeed * 0.4f);
                agent.maxSpeed = runSpeed + extraSpeed;

                RunFromTarget(player, runRadius, updateTime, ref runTimer, runTimerName);
                break;
            case EnemyState.Attack:
                if (agent.canMove) agent.canMove = false;
                Throw();
                break;
        }
    }

    private void UpdateState() {
        float dist = Vector2.Distance(player.transform.position, transform.position);
        if (dist > randomRadius && state != EnemyState.Random) {
            ClearTimers();
            agent.maxSpeed = speed;
            agent.canMove = true;
            state = EnemyState.Random;
        }
        else if (dist < randomRadius && dist > throwRadius && state != EnemyState.Chase) {
            ClearTimers();
            agent.maxSpeed = speed;
            agent.canMove = true;
            state = EnemyState.Chase;
        }
        else if (dist <= throwRadius && dist > runRadius && state != EnemyState.Attack) {
            ClearTimers();
            agent.maxSpeed = speed;
            agent.canMove = true;
            state = EnemyState.Attack;
        }
        else if (dist <= runRadius && state != EnemyState.Run) {
            ClearTimers();
            agent.maxSpeed = runSpeed;
            agent.canMove = true;
            state = EnemyState.Run;
        }
    }

    private void ClearTimers() {
        FunctionTimer.DestroySceneTimer(waitTimerName);
        FunctionTimer.DestroySceneTimer(runTimerName);
        FunctionTimer.DestroySceneTimer(chaseTimerName);
    }

    private void Throw() {
        if (elapsed < throwTime) {
            elapsed += Time.deltaTime;
        }
        else {
            Vector2 dir = player.position - transform.position;

            if (Vector2.Angle(Vector2.right, dir) <= 45)
                dir = Vector2.right;
            else if (Vector2.Angle(Vector2.left, dir) <= 45)
                dir = Vector2.left;
            else if (Vector2.Angle(Vector2.up, dir) <= 45)
                dir = Vector2.up;
            else if (Vector2.Angle(Vector2.down, dir) <= 45)
                dir = Vector2.down;

            hand.right = dir;
            if (dir == Vector2.up) visual.sortingLayerID = SortingLayer.NameToID("AboveChar");

            EnemyAnimation anim = GetComponent<EnemyAnimation>();
            anim.PlayShootAnimation(dir);

            FunctionTimer.CreateSceneTimer(() => {
                Projectile boomarang = Instantiate(boomarangPref).GetComponent<Projectile>();
                boomarang.transform.position = hand.GetChild(0).position;

                Vector2 dir = (player.position - hand.GetChild(0).position).normalized;
                EnemyStats stats = GetComponent<EnemyStats>();

                boomarang.Setup(dir, 14, 12, stats.atk, stats.luck, LayerMask.GetMask("Player"));

                FunctionTimer.CreateSceneTimer(() => {
                    if (dir == Vector2.up) visual.sortingLayerID = SortingLayer.NameToID("Character");
                }, 0.3f);
            }, anim.GetShootAnimationTime() * 0.3f);

            elapsed = 0;
        }
    }

    public override Vector2 GetMoveDir() {
        if (state == EnemyState.Attack) {
            Vector2 dir = player.position - transform.position;
            Vector2 fixedDir = Vector2.right; ;

            if (Vector2.Angle(Vector2.right, dir) <= 45)
                fixedDir = Vector2.right;
            else if (Vector2.Angle(Vector2.left, dir) <= 45)
                fixedDir = Vector2.left;
            else if (Vector2.Angle(Vector2.up, dir) <= 45)
                fixedDir = Vector2.up;
            else if (Vector2.Angle(Vector2.down, dir) <= 45)
                fixedDir = Vector2.down;

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
}
