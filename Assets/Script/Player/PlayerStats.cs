using UnityEngine;

public class PlayerStats : MonoBehaviour, IStats {
    private PlayerShared shared;

    private void Start() {
        shared = GetComponent<PlayerShared>();
    }

    public void TakeDamage(float atk, float luck) {
        Character charData = CharacterManager.Instance?.characters[shared.charId];
        if (charData == null) return;

        float agi = charData.AGI;
        float chance = Random.Range(0, luck + (agi / 2));

        if (chance <= luck) {
            float def = charData.DEF;
            float dmg = 3 * atk - 2 * def;
            if (dmg < 0) dmg = 1;

            float hp = charData.curHp - dmg;
            charData.SetCurHp(hp);

            Debug.Log(dmg);
        }

        shared.healthbar.SetFill((float)charData.curHp / charData.MHP);
    }
}
