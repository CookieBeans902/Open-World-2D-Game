using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Character {
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

    public int currHealth { get; private set; }
    public int maxHealth { get; private set; }
    public int attack { get; private set; }
    public int magicAttack { get; private set; }
    public int defence { get; private set; }
    public int magicDefence { get; private set; }
    public int agility { get; private set; }
    public int luck { get; private set; }

    public Dictionary<string, Equipment> equipments { get; private set; }
    public List<string> slots { get; private set; }

    /// <summary> To create a character object from a SO</summary>
    /// <param name="character"> The SO of the character you want to create</param>
    public Character(CharacterSO character) {

        characterName = character.characterName;
        characterClass = character.characterClass;

        currLevel = character.currLevel;
        maxLevel = character.maxLevel;

        maxExperience = character.maxExperience;

        baseMaxHealth = character.baseMaxHealth;
        baseAttack = character.baseAttack;
        baseMagicAttack = character.baseMagicAttack;
        baseDefence = character.baseDefence;
        baseMagicDefence = character.baseMagicDefence;
        baseAgility = character.baseAgility;
        baseLuck = character.baseLuck;

        canDualWeild = character.canDualWeild;

        slots = new List<string> { "head", "body", "hand1", "hand2", "boots", "accessory", };

        equipments = new Dictionary<string, Equipment>();
        equipments["head"] = Equipment.Create(character.head);
        equipments["body"] = Equipment.Create(character.body);
        equipments["hand1"] = Equipment.Create(character.hand1);
        equipments["hand2"] = Equipment.Create(character.hand2);
        equipments["boots"] = Equipment.Create(character.boots);
        equipments["accessory"] = Equipment.Create(character.accessory);

        ApplyEquipmentsBuffs();

        currExperience = 0;
        currHealth = maxHealth;
    }


    /// <summary>To take damage on hit</summary>
    /// <param name="damage">Value of damage</param>
    public void TakeDamage(int damage) {
        if (damage < 0) return;
        if (damage == 0) damage = 1;

        currHealth -= damage;
        if (currHealth < 0) currHealth = 0;
    }


    /// <summary>To equip an equipment</summary>
    /// <param name="equipment">Equipment you want it to equip</param>
    public void Equip(Equipment equipment) {
        if (InventoryManager.Instance.SlotCount(equipment.equipmentName) == 0)
            return;
        if (equipment == null)
            return;
        bool canEquip = false;
        foreach (Class c in equipment.validClasses) {
            if (c == characterClass) {
                canEquip = true;
                break;
            }
        }
        if (!canEquip) {
            Debug.Log("This character can't equip" + equipment.equipmentName);
            return;
        }

        switch (equipment.slot) {
            case EquipmentSlot.Head:
                equipments["head"] = equipment;
                break;
            case EquipmentSlot.Body:
                equipments["body"] = equipment;
                break;
            case EquipmentSlot.Hand1:
                equipments["hand1"] = equipment;
                break;
            case EquipmentSlot.Hand2:
                equipments["hand2"] = equipment;
                break;
            case EquipmentSlot.Boots:
                equipments["boots"] = equipment;
                break;
            case EquipmentSlot.Accessory:
                equipments["accessory"] = equipment;
                break;
        }
        // equipment.isEquiped = true;
        ApplyEquipmentsBuffs();
    }

    /// <summary>To unequip an equipment</summary>
    /// <param name="slot">Equipment slot you want it to unequip</param>
    public void Unequip(EquipmentSlot slot) {
        switch (slot) {
            case EquipmentSlot.Head:
                if (equipments["head"] == null) return;
                equipments["head"] = null;
                // equipments["head"].isEquiped = false;
                break;
            case EquipmentSlot.Body:
                if (equipments["body"] == null) return;
                equipments["body"] = null;
                // equipments["body"].isEquiped = false;
                break;
            case EquipmentSlot.Hand1:
                if (equipments["hand1"] == null) return;
                equipments["hand1"] = null;
                // equipments["hand1"].isEquiped = false;
                break;
            case EquipmentSlot.Hand2:
                if (equipments["hand2"] == null) return;
                equipments["hand2"] = null;
                // equipments["hand2"].isEquiped = false;
                break;
            case EquipmentSlot.Boots:
                if (equipments["boots"] == null) return;
                equipments["boots"] = null;
                // equipments["boots"].isEquiped = false;
                break;
            case EquipmentSlot.Accessory:
                if (equipments["accessory"] == null) return;
                equipments["accessory"] = null;
                // equipments["accessory"].isEquiped = false;
                break;
        }
        ApplyEquipmentsBuffs();
    }


    /// <summary>To apply the buffs current equiments have</summary>
    private void ApplyEquipmentsBuffs() {
        int healthBuff = 0, attackBuff = 0, magicAttackBuff = 0, defenceBuff = 0, magicDefenceBuff = 0, agilityBuff = 0, luckBuff = 0;

        foreach (KeyValuePair<string, Equipment> p in equipments) {
            healthBuff += p.Value != null ? p.Value.healthBuff : 0;
            attackBuff += p.Value != null ? p.Value.attackBuff : 0;
            magicAttackBuff += p.Value != null ? p.Value.magicAttackBuff : 0;
            defenceBuff += p.Value != null ? p.Value.defenceBuff : 0;
            magicDefenceBuff += p.Value != null ? p.Value.magicDefenceBuff : 0;
            agilityBuff += p.Value != null ? p.Value.agilityBuff : 0;
            luckBuff += p.Value != null ? p.Value.luckBuff : 0;
        }

        maxHealth = baseMaxHealth + healthBuff;
        attack = baseAttack + attackBuff;
        magicAttack = baseMagicAttack + magicAttackBuff;
        defence = baseDefence + defenceBuff;
        magicDefence = baseMagicDefence + magicDefenceBuff;
        agility = baseAgility + agilityBuff;
        luck = baseLuck + luckBuff;
    }


    /// <summary>To show stats of the character, useful while debugging</summary>
    public void ShowStats() {
        Debug.Log("maxHealth: " + baseMaxHealth + " + " + (maxHealth - baseMaxHealth));
        Debug.Log("attack: " + baseAttack + " + " + (attack - baseAttack));
        Debug.Log("magicAttack: " + baseMagicAttack + " + " + (magicAttack - baseMagicAttack));
        Debug.Log("defence: " + baseDefence + " + " + (defence - baseDefence));
        Debug.Log("magicDefence: " + baseMagicDefence + " + " + (magicDefence - baseMagicDefence));
        Debug.Log("agility: " + baseAgility + " + " + (agility - baseAgility));
        Debug.Log("luck: " + baseLuck + " + " + (luck - baseLuck));
    }
}