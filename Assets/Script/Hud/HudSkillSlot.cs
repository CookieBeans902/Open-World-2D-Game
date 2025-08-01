using System.Collections;

using Unity.VisualScripting;

using UnityEngine;
using UnityEngine.UI;

public class HudSkillSlot : MonoBehaviour {
    public Skill skill;
    public Image icon;
    public Image cooldown;
    public int index;

    private bool canUse = true;

    private void Start() {
        skill = null;
        // UpdateSlot(skill);
    }

    private void Update() {
        if (Input.GetKeyDown(GetKeyCode())) {
            UseSkill();
        }
    }

    public void UpdateSlot(Skill newSkill) {
        skill = newSkill;
        if (newSkill != null) {
            icon.sprite = newSkill.icon;
            icon.color = Color.white.WithAlpha(1);
        }
        else {
            icon.sprite = null;
            icon.color = Color.white.WithAlpha(0);
        }
    }

    public void UseSkill() {
        if (index == 4 && canUse) {
            StartCoroutine(ShowCooldown(0.6f));
            canUse = false;
        }
        if (skill != null && canUse) {
            Debug.Log("Used");
            StartCoroutine(ShowCooldown(skill.cooldownTime));
            canUse = false;
        }
    }

    private IEnumerator ShowCooldown(float cooldownTime) {
        float elapsed = 0;
        cooldown.fillAmount = 1;

        while (elapsed < cooldownTime) {
            float t = elapsed / cooldownTime;
            cooldown.fillAmount = 1 - t;

            elapsed += Time.deltaTime;
            yield return null;
        }

        cooldown.fillAmount = 0;
        canUse = true;
        yield return null;
    }

    private KeyCode GetKeyCode() {
        switch (index) {
            case 1:
                return KeyCode.J;
            case 2:
                return KeyCode.K;
            case 3:
                return KeyCode.L;
            default:
                return KeyCode.Space;
        }
    }
}
