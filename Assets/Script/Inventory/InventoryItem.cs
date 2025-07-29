using UnityEngine;

[System.Serializable]
public class InventoryItem {
    public ItemType itemType;
    public Sprite icon;
    public string itemName;
    public string itemDesc;
    public bool canConsume;
    public bool canDestroy;
    public bool canSell;
    public int maxStack;
    public int count;
    public int buyPrice;
    public int sellPrice;

    public int slotNumber = -1;
    public bool isActive = false;

    // corresponding equipment it represents
    public EquipmentSO equipment;

    /// <summary> To create an item object from a SO</summary>
    /// <param name="equipment"> The SO of the item you want to create</param>
    public static InventoryItem Create(ItemSO item) {
        if (item == null) return null;

        InventoryItem i = new InventoryItem {
            itemType = item.itemType,
            icon = item.icon,
            itemName = item.itemName,
            itemDesc = item.itemDesc,
            canConsume = item.canConsume,
            canDestroy = item.canDestroy,
            canSell = item.canSell,
            maxStack = item.maxStack,
            count = 1, // default item count
            buyPrice = item.buyPrice,
            sellPrice = item.sellPrice,
            equipment = item.equipment
        };

        return i;
    }
}