using UnityEngine;

[CreateAssetMenu(fileName = "ItemSO", menuName = "Scriptable Objects/ItemSO")]
public class ItemSO : ScriptableObject {
    public ItemType itemType;
    public Sprite icon;
    public string itemName;
    public string itemDesc;
    public bool canConsume;
    public bool canDestroy;
    public bool canSell;
    public int maxStack;
    public int buyPrice;
    public int sellPrice;
    public EquipmentSO equipment;
}
