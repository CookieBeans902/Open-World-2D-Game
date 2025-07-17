using Game.Utils;

using UnityEditor.ShaderGraph.Internal;

using UnityEngine;

public class Arrow : MonoBehaviour {
    private float range;
    private float elapsed;
    private float time;
    private float speed;
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
        rb.linearVelocity = Vector2.zero;
        rb.bodyType = RigidbodyType2D.Kinematic;
        rb.simulated = false;

        transform.SetParent(collision.transform);
        if (collision.contactCount > 0) {
            Debug.Log(collision.gameObject);

            Vector2 point = collision.contacts[0].point;
            Debug.Log(point);
            transform.parent = collision.transform;

            Vector3 pos = transform.position;
            transform.position = new Vector3(pos.x, pos.y, collision.transform.position.z + 0.1f) + (Vector3)dir * 0.1f;
        }

        IStats stats = collision.collider.GetComponent<IStats>();
        if (stats != null) stats.TakeDamage(10, 10);
    }

    public void Setup(Vector2 dir, float speed, float range, int layerMask) {
        time = range / speed;
        elapsed = 0;
        float mass = rb.mass;
        this.dir = dir;
        Vector2 velocity = dir * speed;
        transform.rotation = VectorHandler.RotationFromVector(dir);

        rb.AddForce(mass * velocity, ForceMode2D.Impulse);
    }

    private void DestroySelf() {
        Destroy(gameObject);
    }
}
