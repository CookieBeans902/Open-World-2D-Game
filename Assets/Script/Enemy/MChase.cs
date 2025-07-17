using Game.Utils;

using UnityEngine;

public class MChase : MovementBase {
    private MoveStates moveState;
    private Transform player;
    private MeleeAttack meleeAttack;
    private FunctionTimer chaseTimer;
    private string chaseTimerName;
    private string waitTimerName;

    [SerializeField] private bool moveBackToCentre;
    [SerializeField] private float speed;
    [SerializeField] private float chaseSpeed;
    [SerializeField] private float radius;
    [SerializeField] private float chaseRadius;
    [SerializeField] private float waitTime;
    [SerializeField] private Transform centre;

    private float updateTime;

    private void Awake() {
        InitBasicComponents();
        waitTimerName = "WaitTimer" + gameObject.GetInstanceID();
        chaseTimerName = "ChaseTimer" + gameObject.GetInstanceID();
        moveState = MoveStates.Random;
        agent.maxSpeed = speed;
        updateTime = 0.2f;
    }

    private void Start() {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        meleeAttack = GetComponent<MeleeAttack>();
    }

    private void Update() {
        switch (moveState) {
            case MoveStates.Random:
                if (!canMove) return;
                MoveRandom(centre.position, radius, waitTime, waitTimerName);
                break;
            case MoveStates.Chasing:
                if (!canMove) return;
                MoveToTarget(player, updateTime, ref chaseTimer, chaseTimerName);
                break;
        }

        UpdateState();
    }

    private void UpdateState() {
        float dist = VectorHandler.GetDistance(transform.position, player.position);

        if (dist < endSep + 0.6f) meleeAttack.canAttack = true;
        else meleeAttack.canAttack = false;

        if (dist < chaseRadius && moveState != MoveStates.Chasing) {
            agent.maxSpeed = chaseSpeed;
            moveState = MoveStates.Chasing;
        }
        else if (dist >= chaseRadius && moveState != MoveStates.Random) {
            agent.maxSpeed = speed;
            moveState = MoveStates.Random;
            if (!moveBackToCentre) centre.position = transform.position;
            seeker.StartPath(transform.position, transform.position);
            hasStarted = true;
        }
    }
}
