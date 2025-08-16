using UnityEngine;

[CreateAssetMenu(fileName = "ItemSO", menuName = "Scriptable Objects/ItemSO")]
public class ItemSO : ScriptableObject {
    public ItemType itemType;
    public EffectType effectType;
    public Sprite icon;
    public string itemName;
    public bool canConsume;
    public bool canDestroy;
    public bool canSell;
    public int maxStack = 1;
    public int buyPrice;
    public int sellPrice;
    public int effectValue;
    public float cooldownTime;
    public EquipmentSO equipment;

    [TextArea]
    public string itemDesc;
}
