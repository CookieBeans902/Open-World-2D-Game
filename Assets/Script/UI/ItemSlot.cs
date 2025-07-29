using TMPro;

using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour {
    // public Image icon;
    // public Button button;
    public TextMeshProUGUI count;

    public Transform contentBox;
    public InventoryItem item;
    public Character ch;

    // public void OnDrop(PointerEventData eventData) {
    //     GameObject obj = eventData.pointerDrag;

    //     if (obj != null) {
    //         ItemUI equip = obj.GetComponent<ItemUI>();

    //         if (equip == null) return;

    //         if (ch.CanEquip(Equipment.Create(equip.equipment))) {
    //             // if (equipment != null) StatsUIManager.Instance.UnequipSlot(equipment);
    //             item = InventoryItem.Create(equip.equipment.item);
    //             StatsUIManager.Instance.UnequipSlot(Equipment.Create(equip.equipment));
    //             StatsUIManager.Instance.UpdateStats();
    //         }
    //     }
    // }

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
