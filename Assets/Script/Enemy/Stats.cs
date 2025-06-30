using UnityEngine;

public class Stats : MonoBehaviour {
    public int curHp { get; private set; }
    [SerializeField] private int mhp;
    [SerializeField] private float atk;
    [SerializeField] private float def;
    [SerializeField] private float agi;
    [SerializeField] private float luck;
    [SerializeField] private Healthbar healthbar;

    /// <summary>
    /// This is to be called whenever you want the target to take damage.
    /// </summary>
    /// <param name="atk">atk of the attacker.</param>
    /// <param name="luck">luck of the attacker.</param>
    public void TakeDamage(float atk, float luck) {
        float damage = atk - def;
        damage = damage <= 0 ? 1 : damage;

        float chanceRange = agi - luck;
        chanceRange = chanceRange < 1 ? 1 : chanceRange;
        chanceRange = Mathf.Log(chanceRange, 2);
        float chance = Random.Range(0, chanceRange + 8);

        if (chance > chanceRange) {
            curHp -= (int)damage;
            curHp = curHp < 0 ? 0 : curHp;
        }

        float val = (float)curHp / mhp;
        healthbar.SetFill(val);
    }

    public void curHP() {
        Debug.Log(curHp + "/" + mhp);
    }
}
