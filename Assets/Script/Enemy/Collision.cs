using UnityEngine;

public class Collision : MonoBehaviour {
    void OnCollisionStay2D(Collision2D collision) {
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        rb.linearVelocity *= 0.5f;
    }
}
