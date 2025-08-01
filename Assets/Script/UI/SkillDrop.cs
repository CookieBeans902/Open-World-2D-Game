using UnityEngine;
using UnityEngine.EventSystems;

public class SkillDrop : MonoBehaviour, IDropHandler {
    public void OnDrop(PointerEventData eventData) {
        GameObject obj = eventData.pointerDrag;

        if (obj != null) {
            SkillUI skill = eventData.pointerDrag.GetComponent<SkillUI>();

            if (skill != null && skill.skill.slot != -1) {
                if (skill.skill != null) StatsUIManager.Instance.UnequipSkill(skill.skill, skill.skill.slot);
                Destroy(skill.gameObject);
                StatsUIManager.Instance.UpdateSkills();
            }
        }
    }
}
