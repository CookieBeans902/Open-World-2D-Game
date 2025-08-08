using UnityEngine;

public class Collision : MonoBehaviour {
    void OnCollisionStay2D(Collision2D collision) {
        Collider2D[] nearbyEnemies = Physics2D.OverlapCircleAll(transform.position, 0.8f, LayerMask.GetMask("Enemy"));

        foreach (Collider2D enemy in nearbyEnemies) {
            if (enemy.gameObject == this.gameObject) continue;

            Vector2 away = enemy.transform.position - transform.position;
            float mag = 1 / (away.magnitude != 0 ? away.magnitude : 0.3f);
            enemy.GetComponent<Rigidbody2D>().AddForce(away.normalized * mag, ForceMode2D.Impulse);
        }
    }
}
