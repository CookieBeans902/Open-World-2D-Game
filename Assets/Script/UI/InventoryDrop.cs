using UnityEngine;
using UnityEngine.EventSystems;

public class InventoryDrop : MonoBehaviour, IDropHandler {
    public void OnDrop(PointerEventData eventData) {
        GameObject obj = eventData.pointerDrag;

        if (obj != null) {
            ItemUI item = eventData.pointerDrag.GetComponent<ItemUI>();

            if (item != null) {

                if (item.item.itemType == ItemType.Equipment && item.item.isActive) {
                    Equipment equipment = Equipment.Create(item.item.equipment);
                    equipment.item = item.item;

                    if (equipment != null) StatsUIManager.Instance.UnequipSlot(Equipment.Create(item.item.equipment));
                }
                else {
                    if (item.item.slotNumber != -1) StatsUIManager.Instance.RemoveActiveItem(item.item);
                }

                StatsUIManager.Instance.UpdateUI();
                Destroy(item.gameObject);
            }
        }
    }
}
