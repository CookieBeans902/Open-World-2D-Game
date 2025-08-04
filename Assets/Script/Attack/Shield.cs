using System;

using Game.Utils;

using UnityEngine;

public class Shield : MonoBehaviour {
    private float elapsed = 0;
    [SerializeField] private float damageRate = 0.1f;

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

    public void Setup(float destroyTime, Action onComplete) {
        transform.localPosition = Vector2.zero;
        SetDestroy(onComplete, destroyTime);
    }

    private void SetDestroy(Action onCOmplete, float destroyTime) {
        FunctionTimer.CreateSceneTimer(() => {
            onCOmplete();
            Destroy(gameObject);
        }, destroyTime);
    }
}
