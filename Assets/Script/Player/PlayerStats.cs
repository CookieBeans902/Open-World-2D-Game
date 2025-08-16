using UnityEngine;

public class PlayerStats : MonoBehaviour, IStats {
    private PlayerShared shared;

    private void Start() {
        shared = GetComponent<PlayerShared>();
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.C)) {
            Character charData = CharacterManager.Instance?.characters[shared.charId];
            if (charData == null) return;

            charData.AddExp(60);
            HudManager.Instance?.RequestStatUpdate();
        }
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

            dmg = (int)dmg;
            float hp = charData.curHp - dmg;
            charData.SetCurHp(hp);

            PopupManager.RequestDamagePopup(dmg.ToString(), transform.position, 6, Color.red);
        }
        else {
            PopupManager.RequestDamagePopup("miss", transform.position, 4, Color.white); ;
        }

        shared.healthbar.SetFill((float)charData.curHp / charData.MHP);
        HudManager.Instance?.RequestStatUpdate();
    }

    public void RecoverHp(float amount) {
        Character charData = CharacterManager.Instance?.characters[shared.charId];
        if (charData == null) return;

        float luck = charData.LUCK;
        float a = amount - charData.DEF * 2 + charData.MHP * 0.2f;
        float chance = Random.Range(0, luck + 10);

        if (chance <= luck) {
            a *= (int)(Random.Range(1.1f, 1.4f) * amount);
            a = Mathf.Clamp(a, 0, charData.MHP - charData.curHp);
            int hp = charData.curHp + (int)a;
            charData.SetCurHp(hp);
            PopupManager.RequestDamagePopup(a.ToString(), transform.position, 6, Color.green);
        }
        else {
            a *= (int)(Random.Range(0.8f, 1f) * amount);
            a = Mathf.Clamp(a, 0, charData.MHP - charData.curHp);
            int hp = charData.curHp + (int)a;
            charData.SetCurHp(hp);
            PopupManager.RequestDamagePopup(a.ToString(), transform.position, 6, Color.green);
        }

        shared.healthbar.SetFill((float)charData.curHp / charData.MHP);
        HudManager.Instance?.RequestStatUpdate();
    }
}
