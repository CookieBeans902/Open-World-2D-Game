using System.Collections.Generic;

using Game.Utils;

using UnityEngine;

public class EnemyStats : MonoBehaviour, IStats {
    public float curHp;
    public float mhp;
    public float atk;
    public float def;
    public float agi;
    public float luck;
    public float pushbackForce;

    [SerializeField] private Healthbar healthbar;

    [SerializeField] private List<Transform> collectibles;
    [SerializeField] private List<int> minCount;
    [SerializeField] private List<int> maxCount;
    [SerializeField] private float dropSpeed = 6;
    [SerializeField] private float dropTime = 0.7f;

    private bool isDefeated;

    private void Awake() {
        curHp = mhp;
    }

    /// <summary>
    /// This is to be called whenever you want the target to take damage.
    /// </summary>
    /// <param name="atk">atk of the attacker.</param>
    /// <param name="luck">luck of the attacker.</param>
    public void TakeDamage(float atk, float luck) {
        if (isDefeated) return;

        float chance = Random.Range(0, luck + (agi / 2));

        if (chance <= luck) {
            float dmg = 3 * atk - 2 * def;
            if (dmg < 0) dmg = 1;

            float hp = curHp - dmg;
            hp = Mathf.Clamp(hp, 0, mhp);
            curHp = hp;

            PopupManager.RequestDamagePopup(dmg.ToString(), transform.position, 6, Color.red);

            if (hp <= 0) OnDefeat();
        }
        else {
            PopupManager.RequestDamagePopup("Miss", transform.position, 4, Color.white);
        }

        healthbar.SetFill((float)curHp / mhp);
    }

    public void RecoverHp(float amount) {
        if (isDefeated) return;

        float a = amount - def * 2 + mhp * 0.2f;
        float chance = Random.Range(0, luck + 10);

        if (chance <= luck) {
            a *= Random.Range(1.1f, 1.4f);
            a = Mathf.Clamp(a, 0, mhp - curHp);
            float hp = curHp + a;
            hp = Mathf.Clamp(hp, 0, mhp);
            curHp = hp;

            PopupManager.RequestDamagePopup(a.ToString(), transform.position, 6, Color.green);
        }
        else {
            a *= Random.Range(0.8f, 1f);
            a = Mathf.Clamp(a, 0, mhp - curHp);
            float hp = curHp + a;
            hp = Mathf.Clamp(hp, 0, mhp);
            curHp = hp;

            PopupManager.RequestDamagePopup(a.ToString(), transform.position, 6, Color.green);
        }

        healthbar.SetFill((float)curHp / mhp);
    }

    private void OnDefeat() {
        if (isDefeated) return;


        isDefeated = true;
        MovementBase move = GetComponent<MovementBase>();
        EnemyAnimation anim = GetComponent<EnemyAnimation>();

        move.transform.position = transform.position;
        move.FreezeMovement();
        move.enabled = false;

        anim.PlayHurtAnimation();

        FunctionTimer.CreateGlobalTimer(() => {
            if (gameObject == null) return;
            for (int i = 0; i < collectibles.Count; i++) {
                int c = Random.Range(minCount[i], maxCount[i]);

                for (int j = 0; j < c; j++) {
                    Collectible collectible = Instantiate(collectibles[i]).GetComponent<Collectible>();
                    collectible.transform.position = transform.position;

                    collectible.Setup(dropSpeed, dropTime);
                }
            }

            Destroy(gameObject);
        }, anim.GetHurtAnimationTime() + 0.1f);
    }
}
