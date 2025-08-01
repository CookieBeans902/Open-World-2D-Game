using UnityEngine;

public class NpcRandomMovement : MovementBase {
    [SerializeField] private float speed;
    [SerializeField] private float radius;
    [SerializeField] private float waitTime;
    // [SerializeField] private Transform center;

    private Vector2 center;

    private void Awake() {
        InitBasicComponents();
        center = transform.position;
        agent.maxSpeed = speed;
    }

    private void Update() {
        MoveRandom(center, radius, waitTime);
    }
}
