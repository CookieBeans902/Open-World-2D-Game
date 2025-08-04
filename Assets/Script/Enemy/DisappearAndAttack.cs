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

    private EnemyState state;
    private Coroutine fadeCoroutine;

    [SerializeField] private bool moveBackToCentre;

    [SerializeField] private float speed;
    [SerializeField] private float chaseSpeed;
    [SerializeField] private float circleSpeed;

    [SerializeField] private float randomRadius;
    [SerializeField] private float chaseRadius;
    [SerializeField] private float circleRadius;

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
        else if (dist <= circleRadius && state != EnemyState.Attack) {
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
        Vector2 dir = -player.GetComponent<Movement>().playerDir;
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

            float chance = Random.Range(0, 10);
            if (chance < 3) {
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

        if (Vector2.Angle(Vector2.right, faceDir) <= 45)
            faceDir = Vector2.right;
        else if (Vector2.Angle(Vector2.left, faceDir) <= 45)
            faceDir = Vector2.left;
        else if (Vector2.Angle(Vector2.up, faceDir) <= 45)
            faceDir = Vector2.up;
        else if (Vector2.Angle(Vector2.down, faceDir) <= 45)
            faceDir = Vector2.down;
        GetComponent<MeleeAttack>().Slash(faceDir);
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
