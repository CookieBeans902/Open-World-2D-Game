using UnityEngine;

[CreateAssetMenu(fileName = "CharacterSO", menuName = "Scriptable Objects/CharacterSO")]
public class CharacterSO : ScriptableObject {
    public string characterName;
    public Class characterClass;

    public int currLevel;
    public int maxLevel;

    public int currExperience;
    public int maxExperience;

    public int baseMaxHealth;
    public int baseAttack;
    public int baseMagicAttack;
    public int baseDefence;
    public int baseMagicDefence;
    public int baseAgility;
    public int baseLuck;

    public bool canDualWeild;

    public EquipmentSO head;
    public EquipmentSO body;
    public EquipmentSO boots;
    public EquipmentSO hand1;
    public EquipmentSO hand2;
    public EquipmentSO accessory;
}
