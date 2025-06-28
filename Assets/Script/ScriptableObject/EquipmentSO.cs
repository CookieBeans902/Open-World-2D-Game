using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EquipmentSO", menuName = "Scriptable Objects/EquipmentSO")]
public class EquipmentSO : ScriptableObject {
    public EquipmentSlot slot;
    public EquipmentType type;
    public EquipmentRange range;
    public EquipmentWield weild;
    public List<Class> validClasses;
    public Sprite icon;
    public string equipmentName;
    public bool canSell;
    public int buyPrice;
    public int sellPrice;
    public int healthBuff;
    public int attackBuff;
    public int magicAttackBuff;
    public int defenceBuff;
    public int magicDefenceBuff;
    public int agilityBuff;
    public int luckBuff;
    public ItemSO item;
}
