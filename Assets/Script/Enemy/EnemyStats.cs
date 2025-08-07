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

    private void Awake() {
        curHp = mhp;
    }

    /// <summary>
    /// This is to be called whenever you want the target to take damage.
    /// </summary>
    /// <param name="atk">atk of the attacker.</param>
    /// <param name="luck">luck of the attacker.</param>
    public void TakeDamage(float atk, float luck) {
        float chance = Random.Range(0, luck + (agi / 2));

        if (chance <= luck) {
            float dmg = 3 * atk - 2 * def;
            if (dmg < 0) dmg = 1;

            float hp = curHp - dmg;
            hp = Mathf.Clamp(hp, 0, mhp);
            curHp = hp;

            PopupManager.RequestPopup(dmg.ToString(), transform.position, 10, Color.red);
        }
        else {
            PopupManager.RequestPopup("Miss", transform.position, 10, Color.white);
        }

        healthbar.SetFill((float)curHp / mhp);
    }

    public void RecoverHp(float amount) {
        float a = amount - def * 2 + mhp * 0.2f;
        float chance = Random.Range(0, luck + 10);

        if (chance <= luck) {
            a *= Random.Range(1.1f, 1.4f);
            a = Mathf.Clamp(a, 0, mhp - curHp);
            float hp = curHp + a;
            hp = Mathf.Clamp(hp, 0, mhp);
            curHp = hp;

            PopupManager.RequestPopup(a.ToString(), transform.position, 10, Color.green);
        }
        else {
            a *= Random.Range(0.8f, 1f);
            a = Mathf.Clamp(a, 0, mhp - curHp);
            float hp = curHp + a;
            hp = Mathf.Clamp(hp, 0, mhp);
            curHp = hp;

            PopupManager.RequestPopup(a.ToString(), transform.position, 10, Color.green);
        }

        healthbar.SetFill((float)curHp / mhp);
    }
}
