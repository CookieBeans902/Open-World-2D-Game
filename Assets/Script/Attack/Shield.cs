using System;

using Game.Utils;

using UnityEngine;

public class Shield : MonoBehaviour {
    private int targetMasks;
    private float atk;
    private float luck;
    private float elapsed = 0;
    [SerializeField] private float damageRate = 0.05f;

    private void OnTriggerStay2D(Collider2D collider) {
        IStats stats = collider.GetComponent<IStats>();
        int curMask = 1 << collider.gameObject.layer;

        if (stats != null && (curMask & targetMasks) != 0) DealDamage(stats);
    }

    private void DealDamage(IStats stats) {
        if (elapsed < damageRate) {
            elapsed += Time.deltaTime;
        }
        else {
            stats.TakeDamage(atk, luck);
            elapsed = 0;
        }
    }

    public void Setup(float destroyTime, float atk, float luck, int targetMasks) {
        this.targetMasks = targetMasks;
        this.atk = atk;
        this.luck = luck;

        transform.localPosition = Vector2.zero;
        SetDestroy(destroyTime);
    }

    private void SetDestroy(float destroyTime) {
        FunctionTimer.CreateSceneTimer(() => {
            Destroy(gameObject);
        }, destroyTime);
    }
}
