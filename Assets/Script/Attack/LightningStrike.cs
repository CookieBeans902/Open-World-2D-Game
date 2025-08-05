using System;

using Game.Utils;

using UnityEngine;

public class LightningStrike : MonoBehaviour {
    // [SerializeField] private Animator anim;
    [SerializeField] private AnimationClip start;
    // [SerializeField] private AnimationClip end;
    // [SerializeField] private float damageRate = 0.01f;
    // [SerializeField] private SpriteRenderer sprite;
    // private float elapsed = 0;

    private void OnTriggerEnter2D(Collider2D collision) {
        IStats stats = collision.GetComponent<IStats>();
        if (collision.CompareTag("Player") && stats != null) stats.TakeDamage(10, 10);
    }

    // private void DealDamage(IStats stats) {
    //     if (elapsed < damageRate) {
    //         elapsed += Time.deltaTime;
    //     }
    //     else {
    //         stats.TakeDamage(10, 10);
    //         elapsed = 0;
    //     }
    // }

    public void Setup(Vector2 s, Action onCOmplete) {
        transform.position = s;
        SetDestroy(onCOmplete);
    }

    private void SetDestroy(Action onCOmplete) {
        FunctionTimer.CreateSceneTimer(() => {
            onCOmplete();
            Destroy(gameObject);
        }, start.length);
    }
}
