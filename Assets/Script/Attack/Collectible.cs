using System.Collections;

using UnityEngine;

public class Collectible : MonoBehaviour {
    private Transform target = null;
    private bool reached;
    private float elapsed = 0;
    private float increaseFactor = 7;
    private float flightTime;
    void OnTriggerEnter2D(Collider2D collider) {
        if (collider != null) {
            int l = 1 << collider.gameObject.layer;
            if ((l & LayerMask.GetMask("Player")) != 0) target = collider.transform;
            else if ((l & LayerMask.GetMask("Wall")) != 0) elapsed = flightTime;
        }
    }

    private void Update() {
        if (target != null) {
            Vector2 dir = (target.position - transform.position).normalized;

            transform.position += (Vector3)dir * (Time.deltaTime * increaseFactor);
            increaseFactor += Time.deltaTime * 2f;

            if ((transform.position - target.position).magnitude <= 0.3f) Destroy(gameObject);
        }
    }

    private void FixedUpdate() {
        if (!reached) {
            if (elapsed < flightTime) {
                elapsed += Time.deltaTime;
            }
            else {
                StopPhysics();
                reached = true;
            }
        }
    }

    public void Jump(float xrange, float yrange, float flightTime) {
        this.flightTime = flightTime;

        float vx = Random.Range(-xrange, xrange);
        float vy = Random.Range(-yrange * 0.3f, yrange);

        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;

        Vector2 velocity = new Vector2(vx, vy);
        rb.AddForce(velocity * rb.mass, ForceMode2D.Impulse);
    }

    private void StopPhysics() {
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0;
        rb.linearVelocity = Vector2.zero;
        rb.constraints = RigidbodyConstraints2D.FreezePosition | RigidbodyConstraints2D.FreezeRotation;
    }
}

