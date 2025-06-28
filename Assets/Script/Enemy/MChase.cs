using Game.Utils;
using UnityEngine;

public class MChase : MovementBase {
    private MoveStates moveState;
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
        moveState = MoveStates.Random;
        agent.maxSpeed = speed;
    }

    private void Start() {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Update() {
        switch (moveState) {
            case MoveStates.Random:
                MoveRandom(centre.position, radius, waitTime, waitTimerName);
                break;
            case MoveStates.Chasing:
                MoveToTarget(player, updateTime, ref chaseTimer, chaseTimerName);
                break;
        }

        UpdateState();
    }

    private void UpdateState() {
        if (VectorHandler.GetDistance(transform.position, player.position) < chaseRadius) {
            if (moveState != MoveStates.Chasing) {
                agent.maxSpeed = chaseSpeed;
                moveState = MoveStates.Chasing;
            }
            // FunctionTimer.DestroyTimer(waitTimerName);
        }
        else {
            if (moveState != MoveStates.Random) {
                agent.maxSpeed = speed;
                moveState = MoveStates.Random;
                if (!moveBackToCentre) centre.position = transform.position;
                seeker.StartPath(transform.position, transform.position);
                hasStarted = true;
            }
        }
    }
}
