using System.Collections.Generic;
using System.Linq;

using Game.Utils;

using UnityEngine;

public class RunAndHeal : MovementBase {
    private enum EnemyState {
        Random,
        Run,
    }

    private Transform player;
    private FunctionTimer runTimer;
    private string runTimerName;
    private string waitTimerName;

    private float elapsed = 0;

    private EnemyState state;


    [SerializeField] private float speed;
    [SerializeField] private float runSpeed;
    [SerializeField] private float healAmount;

    [SerializeField] private float randomRadius;
    [SerializeField] private float runRadius;
    [SerializeField] private float healRadius;

    [SerializeField] private float waitTime;
    [SerializeField] private float healDelay;
    [SerializeField] private Transform centre;

    private float updateTime;

    private void Awake() {
        InitBasicComponents();
        waitTimerName = "WaitTimer" + gameObject.GetInstanceID();
        runTimerName = "RunTimer" + gameObject.GetInstanceID();
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
            case EnemyState.Run:
                float dist = Vector2.Distance(player.position, transform.position);
                float extraSpeed = 1 / (dist != 0 ? dist : 0.1f);
                extraSpeed = Mathf.Clamp(extraSpeed, 0, runSpeed * 0.4f);
                agent.maxSpeed = runSpeed + extraSpeed;

                RunFromTarget(player, runRadius, updateTime, ref runTimer, runTimerName);
                break;
        }

        HealAllies();
    }

    private void UpdateState() {
        float dist = Vector2.Distance(player.transform.position, transform.position);
        float buffer = 0.1f;


        if (dist > runRadius + buffer && state != EnemyState.Random) {
            Rigidbody2D rb = GetComponent<Rigidbody2D>();
            rb.linearVelocity = Vector2.zero;
            elapsed = 0;

            ClearTimers();
            agent.maxSpeed = speed;
            seeker.StartPath(transform.position, transform.position);
            agent.canMove = true;
            state = EnemyState.Random;
        }
        else if (dist <= runRadius - buffer && state != EnemyState.Run) {
            ClearTimers();
            agent.maxSpeed = runSpeed;
            seeker.StartPath(transform.position, transform.position);
            agent.canMove = true;
            state = EnemyState.Run;
        }
    }

    private void ClearTimers() {
        FunctionTimer.DestroySceneTimer(waitTimerName);
        FunctionTimer.DestroySceneTimer(runTimerName);
    }

    private void HealAllies() {
        if (state == EnemyState.Run) return;
        if (elapsed < healDelay) {
            elapsed += Time.deltaTime;
        }
        else {
            Vector2 dir = player.position - transform.position;
            dir = SnapToNearestDirection(dir);

            seeker.StartPath(transform.position, transform.position);
            agent.canMove = false;

            EnemyAnimation anim = GetComponent<EnemyAnimation>();
            anim.PlayCastAnimation(dir);

            FunctionTimer.CreateSceneTimer(() => {
                Collider2D[] enemies = Physics2D.OverlapCircleAll(transform.position, healRadius);

                foreach (Collider2D e in enemies) {
                    int layer = 1 << e.gameObject.layer;

                    if ((layer & LayerMask.GetMask("Enemy")) != 0) {
                        IStats stats = e.GetComponent<IStats>();
                        stats?.RecoverHp(healAmount);
                    }
                }

                agent.canMove = true;
            }, anim.GetCastAnimationTime());
            elapsed = 0;
        }
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
