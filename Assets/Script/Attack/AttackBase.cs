using UnityEngine;

public class AttackBase : MonoBehaviour {
    protected void PerformSlash(Vector2 dir, float spread, float atk, float luck, int layerMask) {
        float height = 0, width = 0;
        float elongationFactor = 1.2f;

        if (dir == Vector2.right) {
            height = elongationFactor * spread;
            width = spread;
        }
        else if (dir == Vector2.up) {
            height = spread;
            width = elongationFactor * spread;
        }
        else if (dir == Vector2.left) {
            height = elongationFactor * spread;
            width = spread;
        }
        else if (dir == Vector2.down) {
            height = spread;
            width = elongationFactor * spread;
        }
        Vector2 size = new Vector2(width, height);
        Vector2 center = (Vector2)transform.position + dir * spread / 2;

        Collider2D[] hits = Physics2D.OverlapBoxAll(center, size, 0, layerMask);

        foreach (var hit in hits) {
            Debug.Log("Hit enemy: " + hit.name);
            IStats stats = hit.GetComponent<IStats>();
            Rigidbody2D rb = hit.GetComponent<Rigidbody2D>();

            if (stats != null) {
                stats.TakeDamage(atk, luck);
            }

            if (rb != null) {
                Vector2 forceDir = hit.transform.position - transform.position;
                float forceMag = 4;
                rb.AddForce(forceDir * forceMag * rb.mass, ForceMode2D.Impulse);
            }
        }

    }

    protected void PerformThrust(Vector2 dir, float range, float atk, float luck, int layerMask) {
        float height = 0, width = 0;
        float compressionFactor = 0.3f;

        if (dir == Vector2.right) {
            height = compressionFactor * range;
            width = range;
        }
        else if (dir == Vector2.up) {
            height = range;
            width = compressionFactor * range;
        }
        else if (dir == Vector2.left) {
            height = compressionFactor * range;
            width = range;
        }
        else if (dir == Vector2.down) {
            height = range;
            width = compressionFactor * range;
        }
        Vector2 size = new Vector2(width, height);
        Vector2 center = (Vector2)transform.position + dir * range / 2;

        Collider2D[] hits = Physics2D.OverlapBoxAll(center, size, 0, layerMask);

        foreach (var hit in hits) {
            IStats stats = hit.GetComponent<IStats>();
            Rigidbody2D rb = hit.GetComponent<Rigidbody2D>();

            if (stats != null) {
                stats.TakeDamage(atk, luck);
            }

            if (rb != null) {
                Vector2 forceDir = hit.transform.position - transform.position;
                float forceMag = 4;
                rb.AddForce(forceDir * forceMag, ForceMode2D.Impulse);
            }
        }
    }

    // protected void PerformShoot(GameObject ammo, Vector2 velocity, Vector3 start, float range, int layerMask) {
    //     Arrow arrow = Instantiate(ammo).GetComponent<Arrow>();
    //     arrow.transform.position = start;
    //     if (arrow == null) return;

    //     arrow.Setup(velocity.normalized, velocity.magnitude, range, layerMask);
    // }
}
