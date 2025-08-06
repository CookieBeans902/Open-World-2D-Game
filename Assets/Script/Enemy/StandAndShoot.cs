using System.Collections;

using Game.Utils;

using UnityEngine;

public class StandAndShoot : MovementBase {
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


    [SerializeField] private float speed;
    [SerializeField] private float runSpeed;

    [SerializeField] private float randomRadius;
    [SerializeField] private float chaseRadius;
    [SerializeField] private float shootRadius;
    [SerializeField] private float runRadius;

    [SerializeField] private float waitTime;
    [SerializeField] private float shootTime;
    [SerializeField] private Transform centre;

    [SerializeField] private Transform bow;
    [SerializeField] private Transform arrowPref;

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
                Shoot();
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
        else if (dist < randomRadius && dist > shootRadius && state != EnemyState.Chase) {
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

    private void Shoot() {
        if (elapsed < shootTime) {
            elapsed += Time.deltaTime;
        }
        else {
            Vector2 dir = (player.position - transform.position).normalized;
            dir = SnapToNearestDirection(dir);

            if (dir == Vector2.up)
                visual.sortingLayerID = SortingLayer.NameToID("AboveChar");

            bow.right = dir;

            EnemyAnimation anim = GetComponent<EnemyAnimation>();
            anim.PlayShootAnimation(dir);

            FunctionTimer.CreateSceneTimer(() => {
                Projectile arrow = Instantiate(arrowPref).GetComponent<Projectile>();
                arrow.transform.position = bow.GetChild(0).position;

                Vector2 targetDir = (player.position - bow.GetChild(0).position).normalized;
                EnemyStats stats = GetComponent<EnemyStats>();

                arrow.Setup(targetDir, 14, 42, stats.atk, stats.luck, LayerMask.GetMask("Player"));

                FunctionTimer.CreateSceneTimer(() => {
                    if (dir == Vector2.up)
                        visual.sortingLayerID = SortingLayer.NameToID("Character");
                }, 0.3f);
            }, anim.GetShootAnimationTime());

            elapsed = 0;
        }
    }

    private IEnumerator FlyToPlayer(Vector3 start, Vector3 target, Transform arrow) {
        Vector2 dir = (target - start).normalized;
        float t = 0;
        float u = 10, a = 9.8f;
        float reachTime = Vector2.Distance(target, start) / u;

        while (t < reachTime) {
            Vector2 p = (Vector2)start + (dir * u * t);

            Vector3 vp = dir * u;
            Vector3 vz = new Vector3(0, 0, u - 3 * a * t);

            arrow.position = p;
            arrow.right = (vp - vz).normalized;

            float i = 1f, f = 0.5f;
            float s = i + f * (u - vz.magnitude) / u;
            arrow.localScale = Vector3.one * s;

            t += Time.deltaTime;
            yield return null;
        }

        arrow.position = target;
        Destroy(arrow.gameObject);
    }

    public override Vector2 GetMoveDir() {
        if (state == EnemyState.Attack) {
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
}
