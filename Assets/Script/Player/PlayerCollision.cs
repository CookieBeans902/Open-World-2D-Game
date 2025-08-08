using UnityEngine;

public class PlayerCollision : MonoBehaviour {
    private float nerfFactor = 0.4f;
    private void OnCollisionEnter2D(Collision2D collision) {
        Collider2D enemy = Physics2D.OverlapCircle(transform.position, 0.8f, LayerMask.GetMask("Enemy"));

        if (enemy != null) {
            PlayerShared shared = GetComponent<PlayerShared>();
            float emass = enemy.GetComponent<Rigidbody2D>().mass;
            float pmass = GetComponent<Rigidbody2D>().mass;

            nerfFactor = pmass / (pmass + emass * 2);
            shared.move.speedNerf = shared.move.moveSpeed * nerfFactor;
        }
    }

    void OnCollisionExit2D(Collision2D collision) {
        PlayerShared shared = GetComponent<PlayerShared>();
        shared.move.speedNerf = 0;
    }
}
