using UnityEngine;

public class EnemyStats : MonoBehaviour, IStats {
    public float curHp { get; private set; }
    [SerializeField] private float mhp;
    [SerializeField] private float atk;
    [SerializeField] private float def;
    [SerializeField] private float agi;
    [SerializeField] private float luck;
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
        // float damage = atk - def;
        // damage = damage <= 0 ? 1 : damage;

        // float chanceRange = agi - luck;
        // chanceRange = chanceRange < 1 ? 1 : chanceRange;
        // chanceRange = Mathf.Log(chanceRange, 2);
        // float chance = Random.Range(0, chanceRange + 8);

        // if (chance > chanceRange) {
        //     curHp -= (int)damage;
        //     curHp = curHp < 0 ? 0 : curHp;
        // }
        curHp -= atk;

        curHp = Mathf.Clamp(curHp, 0, mhp);
        float val = (float)curHp / mhp;
        healthbar.SetFill(val);
        // if (curHp <= 0) Destroy(gameObject);
    }
}
