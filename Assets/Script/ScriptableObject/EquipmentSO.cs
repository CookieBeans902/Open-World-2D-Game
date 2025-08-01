using System;
using System.Collections.Generic;

using UnityEngine;

[CreateAssetMenu(fileName = "EquipmentSO", menuName = "Scriptable Objects/EquipmentSO")]
public class EquipmentSO : ScriptableObject {
    public SlotType slot;
    public EquipmentType type;
    public EquipmentRange range;
    public EquipmentWield weild;
    public List<Class> validClasses;
    public string equipmentName;
    public ItemSO itemSO;
    public int hpBuff;
    public int atkBuff;
    public int matkBuff;
    public int defBuff;
    public int mdefBuff;
    public int agiBuff;
    public int luckBuff;
}
