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

            PopupManager.RequestPopup(dmg.ToString(), transform.position, 10, Color.red);
        }
        else {
            PopupManager.RequestPopup("miss", transform.position, 8, Color.white); ;
        }

        shared.healthbar.SetFill((float)charData.curHp / charData.MHP);
    }

    public void RecoverHp(float amount) {
        Character charData = CharacterManager.Instance?.characters[shared.charId];
        if (charData == null) return;

        float luck = charData.LUCK;
        float a = amount - charData.DEF * 2 + charData.MHP * 0.2f;
        float chance = Random.Range(0, luck + 10);

        if (chance <= luck) {
            a *= Random.Range(1.1f, 1.4f);
            a = Mathf.Clamp(a, 0, charData.MHP - charData.curHp);
            float hp = charData.curHp + a;
            charData.SetCurHp(hp);
            PopupManager.RequestPopup(a.ToString(), transform.position, 10, Color.green);
        }
        else {
            a *= Random.Range(0.8f, 1f);
            a = Mathf.Clamp(a, 0, charData.MHP - charData.curHp);
            float hp = charData.curHp + a;
            charData.SetCurHp(hp);
            PopupManager.RequestPopup(a.ToString(), transform.position, 10, Color.green);
        }

        shared.healthbar.SetFill((float)charData.curHp / charData.MHP);
    }
}
