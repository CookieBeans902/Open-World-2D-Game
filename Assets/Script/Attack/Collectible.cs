using System.Collections;

using UnityEngine;

public class Collectible : MonoBehaviour {
    private Transform target = null;
    private bool reached;
    private float elapsed = 0;
    private float increaseFactor = 7;
    private float flightTime;

    [SerializeField] private int count = 1;
    [SerializeField] private ItemSO item;
    void OnTriggerStay2D(Collider2D collider) {
        if (!reached) return;
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

            if ((transform.position - target.position).magnitude <= 0.3f) {
                PopupManager.RequestItemPopup(item.name, count, item.icon, transform.position, Color.white, 4);
                InventoryManager.Instance?.AddItem(InventoryItem.Create(item), count);

                Destroy(gameObject);
            }
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

    public void Setup(float speed, float flightTime) {
        this.flightTime = flightTime;

        float vx = Random.Range(-speed, speed);
        float vy = Random.Range(-speed, speed);

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

