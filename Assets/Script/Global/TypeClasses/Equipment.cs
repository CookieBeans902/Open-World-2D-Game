using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[System.Serializable]
public class Equipment {
    public EquipmentSlot slot;
    public EquipmentType type;
    public EquipmentRange range;
    public EquipmentWield weild;
    public List<Class> validClasses;
    public Sprite icon;

    public string equipmentName;

    public bool canSell;
    public bool isEquiped;

    public int healthBuff;
    public int attackBuff;
    public int magicAttackBuff;
    public int defenceBuff;
    public int magicDefenceBuff;
    public int agilityBuff;
    public int luckBuff;
    public int buyPrice;
    public int sellPrice;
    public InventoryItem item;

    // for some custom special effect (not used anywhere yet)
    public Action specialEffect;

    /// <summary> To create an equipment object from a SO</summary>
    /// <param name="equipment"> The SO of the equipment you want to create</param>
    public static Equipment Create(EquipmentSO equipment) {
        if (equipment == null) return null;

        return new Equipment {
            slot = equipment.slot,
            type = equipment.type,
            range = equipment.range,
            weild = equipment.weild,
            validClasses = equipment.validClasses,
            icon = equipment.icon,
            equipmentName = equipment.equipmentName,
            canSell = equipment.canSell,
            buyPrice = equipment.buyPrice,
            sellPrice = equipment.sellPrice,
            healthBuff = equipment.healthBuff,
            attackBuff = equipment.attackBuff,
            magicAttackBuff = equipment.magicAttackBuff,
            defenceBuff = equipment.defenceBuff,
            magicDefenceBuff = equipment.magicDefenceBuff,
            agilityBuff = equipment.agilityBuff,
            luckBuff = equipment.luckBuff,
            item = equipment.item != null ? InventoryItem.Create(equipment.item) : null,
            isEquiped = false,
        };
    }
}