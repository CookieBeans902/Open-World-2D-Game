using UnityEngine;

public class MRandom : MovementBase {
    [SerializeField] private float speed;
    [SerializeField] private float radius;
    [SerializeField] private float waitTime;
    [SerializeField] private Transform centre;

    private void Awake() {
        InitBasicComponents();
        agent.maxSpeed = speed;
    }

    private void Update() {
        MoveRandom(centre.position, radius, waitTime);
    }
}
