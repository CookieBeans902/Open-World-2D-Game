using System;
using System.Collections.Generic;

using UnityEditor;

using UnityEngine;

[System.Serializable]
public class Equipment {
    public SlotType slot;
    public EquipmentType type;
    public EquipmentRange range;
    public EquipmentWield weild;
    public GameObject equipmentUI;
    public List<Class> validClasses;
    public string equipmentName;
    public bool isEquiped;

    public int hpBuff;
    public int atkBuff;
    public int matkBuff;
    public int defBuff;
    public int mdefBuff;
    public int agiBuff;
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
            // icon = equipment.icon,
            equipmentName = equipment.equipmentName,
            hpBuff = equipment.hpBuff,
            atkBuff = equipment.atkBuff,
            matkBuff = equipment.matkBuff,
            defBuff = equipment.defBuff,
            mdefBuff = equipment.mdefBuff,
            agiBuff = equipment.agiBuff,
            luckBuff = equipment.luckBuff,
            item = equipment.item != null ? InventoryItem.Create(equipment.item) : null,
            isEquiped = false,
        };
    }
}