using TMPro;

using UnityEngine;
using UnityEngine.EventSystems;

public class ActiveItemSlot : MonoBehaviour, IDropHandler {
    public Transform contentBox;
    public InventoryItem item;
    public int keyNumber;
    public Character ch;

    public TextMeshProUGUI count;
    [SerializeField] private TextMeshProUGUI keyText;

    private void Start() {
        keyText.text = keyNumber.ToString();
    }

    public void OnDrop(PointerEventData eventData) {
        GameObject obj = eventData.pointerDrag;

        if (obj != null) {
            ItemUI item = obj.GetComponent<ItemUI>();

            if (item == null || item.item.itemType == ItemType.Equipment) return;

            StatsUIManager.Instance.SetActiveItem(item.item, keyNumber);

            StatsUIManager.Instance.UpdateInventory();
            Destroy(obj);

            // if (ch.CanEquip(equipment) && equipment.slot == slotType) {
            // item = item.item;
            // StatsUIManager.Instance.EquipSlot(equipment);

            // StatsUIManager.Instance.UpdateUI();
            // Destroy(obj);
            // }
        }
    }
}
