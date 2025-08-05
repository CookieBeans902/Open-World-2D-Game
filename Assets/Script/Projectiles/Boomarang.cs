using Game.Utils;

using UnityEngine;

public class Boomarang : MonoBehaviour {
    // [SerializeField] private Animator animator;
    // [SerializeField] private AnimationClip dummy;
    // [SerializeField] private AnimationClip hit;
    // [SerializeField] private AnimationClip disappear;

    private float range;
    private float elapsed;
    private float time;
    private float speed;
    private bool isDestroyed;
    private Vector2 dir;
    private Rigidbody2D rb;

    private void Awake() {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update() {
        if (elapsed > time) {
            DestroySelf();
        }
        else {
            elapsed += Time.deltaTime;
        }
    }

    void OnCollisionEnter2D(Collision2D collision) {
        IStats stats = collision.collider.GetComponent<IStats>();
        if (stats != null) {
            stats.TakeDamage(10, 10);
            // Destroy(gameObject);
        }
        DestroySelf();
    }

    public void Setup(Vector2 dir, float speed, float range, int layerMask) {
        time = range / speed;
        elapsed = 0;
        float mass = rb.mass;
        this.dir = dir;
        Vector2 velocity = dir * speed;
        transform.rotation = VectorHandler.RotationFromVector(dir);

        rb.AddForce(mass * velocity, ForceMode2D.Impulse);
        rb.AddTorque(speed * 1.5f, ForceMode2D.Impulse);
    }

    private void DestroySelf() {
        Destroy(gameObject);
    }
}
