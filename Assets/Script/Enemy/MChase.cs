using Game.Utils;
using UnityEngine;

public class MChase : MovementBase {
    private MoveState moveState;
    private Transform player;
    private FunctionTimer chaseTimer;
    private string chaseTimerName;
    private string waitTimerName;

    [SerializeField] private bool moveBackToCentre;
    [SerializeField] private float speed;
    [SerializeField] private float chaseSpeed;
    [SerializeField] private float radius;
    [SerializeField] private float chaseRadius;
    [SerializeField] private float updateTime;
    [SerializeField] private float waitTime;
    [SerializeField] private Transform centre;

    private void Awake() {
        InitBasicComponents();
        waitTimerName = "WaitTimer" + gameObject.GetInstanceID();
        chaseTimerName = "ChaseTimer" + gameObject.GetInstanceID();
        moveState = MoveState.Random;
        agent.maxSpeed = speed;
    }

    private void Start() {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Update() {
        switch (moveState) {
            case MoveState.Random:
                MoveRandom(centre.position, radius, waitTime, waitTimerName);
                break;
            case MoveState.Chasing:
                MoveToTarget(player, updateTime, ref chaseTimer, chaseTimerName);
                break;
        }

        UpdateState();
    }

    private void UpdateState() {
        if (VectorHandler.GetDistance(transform.position, player.position) < chaseRadius) {
            if (moveState != MoveState.Chasing) {
                agent.maxSpeed = chaseSpeed;
                moveState = MoveState.Chasing;
            }
            // FunctionTimer.DestroyTimer(waitTimerName);
        }
        else {
            if (moveState != MoveState.Random) {
                agent.maxSpeed = speed;
                moveState = MoveState.Random;
                if (!moveBackToCentre) centre.position = transform.position;
                seeker.StartPath(transform.position, transform.position);
                hasStarted = true;
            }
        }
    }
}
