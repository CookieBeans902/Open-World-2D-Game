using System.Collections;

using Game.Utils;

using Unity.VisualScripting;

using UnityEngine;

public class DisappearAndAttack : MovementBase {
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
    private string circleTimerName;

    private float elapsed = 0;
    private float delay = 1;

    private float negWeigth = 1;
    private float posWeigth = 1;
    private int attackPhase = 0;
    private int sign = 1;

    private Coroutine fadeCoroutine;

    private EnemyState state;

    [SerializeField] private AttackType attackType;
    [SerializeField] private float speed;
    [SerializeField] private float chaseSpeed;
    [SerializeField] private float circleSpeed;

    [SerializeField] private float randomRadius;
    [SerializeField] private float chaseRadius;
    [SerializeField] private float circleRadius;

    [SerializeField] private float spread;
    [SerializeField] private float range;
    [SerializeField] private float attackCooldown = 0.2f;
    [SerializeField] private float waitTime;
    [SerializeField] private Transform centre;

    [SerializeField] private SpriteRenderer visual;
    [SerializeField] private Transform healthbar;
    [SerializeField] private Collider2D hitCollider;

    private float updateTime;

    private void Awake() {
        InitBasicComponents();
        waitTimerName = "WaitTimer" + gameObject.GetInstanceID();
        chaseTimerName = "ChaseTimer" + gameObject.GetInstanceID();
        circleTimerName = "CircleTimer" + gameObject.GetInstanceID();
        state = EnemyState.Attack;
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
                HandleAttack();
                break;
        }
    }

    private void UpdateState() {
        float dist = Vector2.Distance(player.transform.position, transform.position);
        float buffer = 0.1f;

        if (dist > chaseRadius + buffer && state != EnemyState.Random) {
            ClearTimers();
            agent.maxSpeed = speed;
            agent.canMove = true;
            state = EnemyState.Random;
        }
        else if (dist <= chaseRadius - buffer && dist > circleRadius + buffer && state != EnemyState.Chase) {
            ClearTimers();
            agent.maxSpeed = chaseSpeed;
            agent.canMove = true;
            state = EnemyState.Chase;
        }
        else if (dist <= circleRadius - buffer && state != EnemyState.Attack) {
            ClearTimers();
            agent.SetPath(null);
            seeker.StartPath(transform.position, transform.position);
            agent.maxSpeed = circleSpeed;
            state = EnemyState.Attack;

            AttackPlayer();
        }
    }

    private void ClearTimers() {
        FunctionTimer.DestroySceneTimer(waitTimerName);
        FunctionTimer.DestroySceneTimer(chaseTimerName);
        FunctionTimer.DestroySceneTimer(circleTimerName);
    }

    private void HandleAttack() {
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

    private IEnumerator fadein() {
        float elapsed = 0;
        float fadeTime = 0.3f;
        agent.canMove = false;

        healthbar.gameObject.SetActive(false);
        hitCollider.enabled = false;

        while (elapsed <= fadeTime) {
            float t = elapsed / fadeTime;
            visual.color = Color.white.WithAlpha(1 - t);

            elapsed += Time.deltaTime;
            yield return null;
        }

        visual.color = Color.white.WithAlpha(0);
        Vector2 dir = -player.GetComponent<PlayerMove>().playerDir;
        transform.position = player.position + (Vector3)(dir * 1.5f);

        StartCoroutine(fadeout());
    }

    private IEnumerator fadeout() {
        float elapsed = 0;
        float fadeTime = 0.3f;

        while (elapsed <= fadeTime) {
            float t = elapsed / fadeTime;
            visual.color = Color.white.WithAlpha(t);

            elapsed += Time.deltaTime;
            yield return null;
        }

        visual.color = Color.white.WithAlpha(1);

        healthbar.gameObject.SetActive(true);
        hitCollider.enabled = true;
        agent.canMove = true;

        ExecuteAttack();

        FunctionTimer.CreateSceneTimer(() => {
            Vector2 c = player.position;
            Vector2 dir = ((Vector2)transform.position - c).normalized;
            Vector2 target = c + (dir * (circleRadius - 0.3f));
            seeker.StartPath(transform.position, target);

            FunctionTimer.CreateSceneTimer(() => {
                float chance = Random.Range(0, negWeigth + posWeigth);
                if (chance < negWeigth) { sign = -1; negWeigth++; }
                else { sign = 1; posWeigth++; }

                if (Mathf.Abs(negWeigth - posWeigth) >= 6) { negWeigth = 1; posWeigth = 1; }

                fadeCoroutine = null;
                attackPhase = 0;
                elapsed = 0;
            }, 0.3f);

        }, 0.3f);
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

            Vector2 d = -player.GetComponent<PlayerMove>().playerDir;
            Vector2 pos = player.position + (Vector3)(d * 1.5f);

            Collider2D collider = Physics2D.OverlapPoint(pos);

            float chance = Random.Range(0, 10);
            if (collider == null && chance < 3) {
                FunctionTimer.CreateSceneTimer(() => {
                    fadeCoroutine = StartCoroutine(fadein());
                }, 0.2f);
            }
            else {
                FunctionTimer.CreateSceneTimer(() => {
                    ExecuteAttack();

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
    }

    private void ExecuteAttack() {
        Vector2 faceDir = player.position - transform.position;
        faceDir = SnapToNearestDirection(faceDir);

        EnemyStats stats = GetComponent<EnemyStats>();
        if (attackType == AttackType.Slash) GetComponent<MeleeAttack>().Slash(faceDir, spread, stats.atk, stats.luck, stats.pushbackForce);
        else GetComponent<MeleeAttack>().Thrust(faceDir, range, stats.atk, stats.luck, stats.pushbackForce);

        player.GetComponent<PlayerMove>().DisableMovement();

        FunctionTimer.CreateSceneTimer(() => {
            player.GetComponent<PlayerMove>().EnableMovement();
        }, attackCooldown);
    }

    public override Vector2 GetMoveDir() {
        if (state == EnemyState.Attack) {
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
