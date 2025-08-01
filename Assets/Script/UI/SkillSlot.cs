using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SkillSlot : MonoBehaviour, IDropHandler {
    // public Image icon;
    // public Button button;
    // public GameObject selected;

    public Transform contentBox;
    public Skill skill;
    public int slot = -1;
    public Character ch;

    public void OnDrop(PointerEventData eventData) {
        GameObject obj = eventData.pointerDrag;

        if (obj != null) {
            SkillUI skill = obj.GetComponent<SkillUI>();

            if (skill == null) return;

            if (skill.skill.slot != slot) {
                StatsUIManager.Instance.EquipSkill(skill.skill, slot);
                Destroy(obj);

                StatsUIManager.Instance.UpdateSkills();
            }
        }
    }

    // public UnityEvent onSingleClick = new UnityEvent();
    // public UnityEvent onDoubleClick = new UnityEvent();
    // public float doubleClickThreshold = 0.3f;
    // private float lastClickTime = -1f;

    // private void Start() {
    //     button.onClick.RemoveAllListeners();
    //     button.onClick.AddListener(HandleClick);
    // }

    // private void HandleClick() {
    //     float timeSinceLastClick = Time.time - lastClickTime;

    //     if (timeSinceLastClick <= doubleClickThreshold) {
    //         onDoubleClick?.Invoke();
    //         lastClickTime = -1f;
    //     }
    //     else {
    //         lastClickTime = Time.time;
    //         onSingleClick.Invoke();
    //     }
    // }
}
