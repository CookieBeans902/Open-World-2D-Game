using System.Collections.Generic;

using UnityEngine;

[CreateAssetMenu(fileName = "CharacterSO", menuName = "Scriptable Objects/CharacterSO")]
public class CharacterSO : ScriptableObject {
    public string characterName;
    public Class characterClass;
    public bool canDualWeild;
    public int curLvl;
    public int maxLvl;
    public float currExp;

    public StatField<float, float> maxExp;
    public StatField<int, int> baseMhp;
    public StatField<int, int> baseAtk;
    public StatField<int, int> baseMatk;
    public StatField<int, int> baseDef;
    public StatField<int, int> baseMdef;
    public StatField<int, int> baseAgi;
    public StatField<int, int> baseLuck;

    public EquipmentSO head;
    public EquipmentSO body;
    public EquipmentSO boots;
    public EquipmentSO hand1;
    public EquipmentSO hand2;
    public EquipmentSO accessory;

    public List<SkillSO> skills;
}
