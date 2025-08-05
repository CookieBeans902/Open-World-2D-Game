using Game.Utils;

using UnityEngine;

public class ElfBehaviour : MovementBase {
    private enum EnemyState {
        Random,
        Chase,
        Run,
        Attack,
    }

    private Transform player;
    private FunctionTimer runTimer;
    private FunctionTimer chaseTimer;
    private string runTimerName;
    private string chaseTimerName;
    private string waitTimerName;

    private float elapsed = 0;

    private EnemyState state;
    private Vector2 prevDir;

    private bool canShield;
    private bool hasShield;


    [SerializeField] private float speed;
    [SerializeField] private float runSpeed;

    [SerializeField] private float randomRadius;
    [SerializeField] private float chaseRadius;
    [SerializeField] private float shootRadius;
    [SerializeField] private float runRadius;

    [SerializeField] private float waitTime;
    [SerializeField] private float shootTime;
    [SerializeField] private Transform centre;

    [SerializeField] private Transform wand;
    [SerializeField] private Transform centrePoint;
    [SerializeField] private Transform lightningPref;
    [SerializeField] private Transform lightningStrikePref;
    [SerializeField] private Transform shieldPref;

    private float updateTime;

    private void Awake() {
        InitBasicComponents();
        waitTimerName = "WaitTimer" + gameObject.GetInstanceID();
        runTimerName = "RunTimer" + gameObject.GetInstanceID();
        chaseTimerName = "ChaseTimer" + gameObject.GetInstanceID();
        state = EnemyState.Random;
        canShield = true;
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

                if (dist < runRadius * 0.6f) SummonShield();

                RunFromTarget(player, runRadius, updateTime, ref runTimer, runTimerName);
                break;

            case EnemyState.Attack:
                if (agent.canMove) agent.canMove = false;
                Vector2 dir = player.position - transform.position;

                if (Vector2.Angle(Vector2.right, dir) <= 45)
                    dir = Vector2.right;
                else if (Vector2.Angle(Vector2.left, dir) <= 45)
                    dir = Vector2.left;
                else if (Vector2.Angle(Vector2.up, dir) <= 45)
                    dir = Vector2.up;
                else if (Vector2.Angle(Vector2.down, dir) <= 45)
                    dir = Vector2.down;

                float chance = Random.Range(0, 10);
                if (chance < 4) LightningStrike(dir);
                else Shoot(dir);
                break;

        }
    }

    private void UpdateState() {
        float dist = Vector2.Distance(player.transform.position, transform.position);
        if (dist > chaseRadius && state != EnemyState.Random) {
            ClearTimers();
            agent.maxSpeed = speed;
            agent.canMove = true;
            state = EnemyState.Random;
        }
        else if (dist < chaseRadius && dist > shootRadius && state != EnemyState.Chase) {
            ClearTimers();
            agent.maxSpeed = speed;
            agent.canMove = true;
            state = EnemyState.Chase;
        }
        else if (dist <= shootRadius && dist > runRadius && state != EnemyState.Attack) {
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

    private void Shoot(Vector2 dir) {
        if (elapsed < shootTime) {
            elapsed += Time.deltaTime;
        }
        else {
            EnemyAnimation anim = GetComponent<EnemyAnimation>();
            anim.PlayThrustAnimation(dir);
            wand.right = dir;

            FunctionTimer.CreateSceneTimer(() => {
                Arrow lightning = Instantiate(lightningPref).GetComponent<Arrow>();
                lightning.transform.position = wand.GetChild(0).position;
                lightning.Setup((player.position - wand.GetChild(0).position).normalized, 14, 12, LayerMask.GetMask("Player"));
            }, anim.GetThrustAnimationTime());

            elapsed = 0;
        }
    }

    private void LightningStrike(Vector2 dir) {
        if (elapsed < shootTime) {
            elapsed += Time.deltaTime;
        }
        else {
            EnemyAnimation anim = GetComponent<EnemyAnimation>();
            anim.PlayCastAnimation(dir);

            FunctionTimer.CreateSceneTimer(() => {
                Vector2 pos = player.position;
                LightningStrike lightningStrike = Instantiate(lightningStrikePref).GetComponent<LightningStrike>();
                lightningStrike.Setup(pos, () => { });
            }, anim.GetCastAnimationTime());
            elapsed = 0;
        }
    }

    private void SummonShield() {
        if (!canShield) return;
        if (hasShield) return;

        float chance = Random.Range(0, 10);
        if (chance < 6) {
            canShield = false;
            FunctionTimer.CreateSceneTimer(() => canShield = true, 3);
            return;
        }

        Vector2 dir = player.position - transform.position;
        if (Vector2.Angle(Vector2.right, dir) <= 45)
            dir = Vector2.right;
        else if (Vector2.Angle(Vector2.left, dir) <= 45)
            dir = Vector2.left;
        else if (Vector2.Angle(Vector2.up, dir) <= 45)
            dir = Vector2.up;
        else if (Vector2.Angle(Vector2.down, dir) <= 45)
            dir = Vector2.down;

        EnemyAnimation anim = GetComponent<EnemyAnimation>();
        anim.PlayCastAnimation(dir);
        hasShield = true;

        FunctionTimer.CreateSceneTimer(() => {
            Shield shield = Instantiate(shieldPref, centrePoint).GetComponent<Shield>();
            shield.Setup(3, () => hasShield = false);
        }, anim.GetCastAnimationTime());
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
