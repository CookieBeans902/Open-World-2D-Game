using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using UnityEngine.UI;

public class EquipmentSlot : MonoBehaviour, IDropHandler {
    public Transform contentBox;
    public InventoryItem item;
    public SlotType slotType;
    public Character ch;

    public void OnDrop(PointerEventData eventData) {
        GameObject obj = eventData.pointerDrag;

        if (obj != null) {
            ItemUI equip = obj.GetComponent<ItemUI>();

            if (equip == null || equip.item.itemType != ItemType.Equipment) return;
            Equipment equipment = Equipment.Create(equip.item.equipment);

            if (ch.CanEquip(equipment) && equipment.slot == slotType) {
                item = equip.item;
                StatsUIManager.Instance.EquipSlot(equipment);

                StatsUIManager.Instance.UpdateUI();
                Destroy(obj);
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
