using System;

using Game.Utils;

using UnityEngine;

public class FireBreath : MonoBehaviour {
    [SerializeField] private Animator anim;
    [SerializeField] private AnimationClip start;
    [SerializeField] private AnimationClip end;
    [SerializeField] private float damageRate = 0.01f;
    [SerializeField] private SpriteRenderer sprite;
    private float elapsed = 0;

    private void OnTriggerStay2D(Collider2D collision) {
        IStats stats = collision.GetComponent<IStats>();
        if (collision.CompareTag("Player") && stats != null) DealDamage(stats);
    }

    private void DealDamage(IStats stats) {
        if (elapsed < damageRate) {
            elapsed += Time.deltaTime;
        }
        else {
            stats.TakeDamage(10, 10);
            elapsed = 0;
        }
    }

    public void Setup(Vector2 s, Vector2 dir) {
        sprite = GetComponent<SpriteRenderer>();
        if (Vector2.Angle(Vector2.up, dir) <= 45) sprite.sortingOrder = -1;
        else sprite.sortingOrder = 1;

        transform.position = s;
        transform.right = dir;
        anim.CrossFade(start.name, 0.4f, 0);
        SetDestroy();
    }

    public float GetAnimTime() {
        return start.length + end.length;
    }

    private void SetDestroy() {
        FunctionTimer.CreateSceneTimer(() => {
            anim.CrossFade(end.name, 0.4f, 0);
            FunctionTimer.CreateSceneTimer(() => {
                Destroy(gameObject);
            }, end.length);
        }, start.length);
    }
}
