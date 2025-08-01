using Game.Utils;

using UnityEngine;

public class RunAndHeal : MovementBase {
    private enum EnemyState {
        Random,
        Run,
    }

    private Transform player;
    private MeleeAttack meleeAttack;
    private FunctionTimer runTimer;
    private string runTimerName;
    private string waitTimerName;
    // private string attackTimerName;

    // private bool isAttacking;
    private float elapsed = 0;

    private EnemyState state;

    [SerializeField] private bool moveBackToCentre;

    [SerializeField] private float speed;
    [SerializeField] private float runSpeed;

    [SerializeField] private float randomRadius;
    [SerializeField] private float runRadius;

    [SerializeField] private float waitTime;
    [SerializeField] private float healTime;
    [SerializeField] private Transform centre;

    private float updateTime;

    private void Awake() {
        InitBasicComponents();
        waitTimerName = "WaitTimer" + gameObject.GetInstanceID();
        runTimerName = "ChaseTimer" + gameObject.GetInstanceID();
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
        if (dist <= runRadius && state != EnemyState.Run) {
            ClearTimers();
            agent.maxSpeed = runSpeed;
            agent.canMove = true;
            state = EnemyState.Run;
        }
        else if (dist > runRadius && state != EnemyState.Random) {
            ClearTimers();
            agent.maxSpeed = speed;
            agent.canMove = true;
            state = EnemyState.Random;
        }
    }

    private void ClearTimers() {
        FunctionTimer.DestroySceneTimer(waitTimerName);
        FunctionTimer.DestroySceneTimer(runTimerName);
    }

    private void HealAllies() {
        if (state == EnemyState.Run) return;
        if (elapsed < healTime) {
            elapsed += Time.deltaTime;
        }
        else {
            // Heal Logic
        }
    }
}
