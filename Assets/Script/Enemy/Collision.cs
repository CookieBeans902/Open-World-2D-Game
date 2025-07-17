using UnityEngine;

public class Collision : MonoBehaviour {
    void OnCollisionStay2D(Collision2D collision) {
        Rigidbody2D rbOwn = GetComponent<Rigidbody2D>();
        rbOwn.linearVelocity *= 0.1f;
        if (collision.collider.CompareTag("Player")) {
            Debug.Log("Collided with player");

            Rigidbody2D rb = collision.collider.GetComponent<Rigidbody2D>();
            Vector2 dir = (collision.transform.position - transform.position).normalized;
            rb.AddForce(10 * dir, ForceMode2D.Force);
        }
    }
}
