using System;

using Game.Utils;

using UnityEngine;

public class FireBreath : MonoBehaviour {
    private int targetMasks;
    private float atk;
    private float luck;
    [SerializeField] private Animator anim;
    [SerializeField] private AnimationClip start;
    [SerializeField] private AnimationClip end;
    [SerializeField] private float damageRate = 0.01f;
    [SerializeField] private SpriteRenderer sprite;
    private float elapsed = 0;

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

    public void Setup(Vector2 s, Vector2 dir, float atk, float luck, int targetMasks) {
        sprite = GetComponent<SpriteRenderer>();
        if (Vector2.Angle(Vector2.up, dir) <= 45) sprite.sortingOrder = -1;
        else sprite.sortingOrder = 1;

        this.targetMasks = targetMasks;
        this.atk = atk;
        this.luck = luck;

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
