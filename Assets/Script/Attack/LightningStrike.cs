using System;

using Game.Utils;

using UnityEngine;

public class LightningStrike : MonoBehaviour {
    private int targetMasks;
    private float atk;
    private float luck;
    [SerializeField] private AnimationClip clip;

    void OnTriggerEnter2D(Collider2D collider) {
        IStats stats = collider.GetComponent<IStats>();
        int curMask = 1 << collider.gameObject.layer;
        if (stats != null && (curMask & targetMasks) != 0) {
            stats.TakeDamage(atk, luck);
        }
    }

    public void Setup(Vector2 pos, float atk, float luck, int targetMasks) {
        transform.position = pos;

        this.targetMasks = targetMasks;
        this.atk = atk;
        this.luck = luck;

        SetDestroy();
    }

    private void SetDestroy() {
        FunctionTimer.CreateSceneTimer(() => {
            Destroy(gameObject);
        }, clip.length);
    }
}
