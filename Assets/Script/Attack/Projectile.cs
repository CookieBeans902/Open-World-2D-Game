using Game.Utils;

using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Playables;

public class Projectile : MonoBehaviour {
    private float elapsed;
    private float time;

    private int targetMasks;
    private float atk;
    private float luck;

    private Rigidbody2D rb;

    private void Awake() {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update() {
        if (elapsed > time) DestroySelf();
        else elapsed += Time.deltaTime;
    }

    void OnTriggerEnter2D(Collider2D collider) {
        IStats stats = collider.GetComponent<IStats>();
        int curMask = 1 << collider.gameObject.layer;
        if (stats != null && (curMask & targetMasks) != 0) stats.TakeDamage(atk, luck);

        if ((curMask & LayerMask.GetMask("Enemy")) == 0) DestroySelf();

        Debug.Log(collider);
    }

    public void Setup(Vector2 dir, float speed, float range, float atk, float luck, int targetMasks) {
        elapsed = 0;
        time = range / speed;

        this.targetMasks = targetMasks;
        this.atk = atk;
        this.luck = luck;

        float mass = rb.mass;
        Vector2 velocity = dir * speed;

        transform.rotation = VectorHandler.RotationFromVector(dir);
        rb.AddForce(mass * velocity, ForceMode2D.Impulse);
    }

    private void DestroySelf() {
        Destroy(gameObject);
    }
}
