using UnityEngine;

public class EnemyStats : MonoBehaviour, IStats {
    public float curHp;
    public float mhp;
    public float atk;
    public float def;
    public float agi;
    public float luck;
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
        }

        healthbar.SetFill((float)curHp / mhp);
    }
}
