using System;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;

[System.Serializable]
public class Character {
    public Class characterClass { get; private set; }
    public string characterName { get; private set; }
    public int curLvl { get; private set; }
    public int maxLvl { get; private set; }
    public int curExp { get; private set; }
    public int curHp { get; private set; }
    public bool canDualWeild { get; private set; }

    public StatField<int, int> maxExp { get; private set; }
    public StatField<int, int> baseMhp { get; private set; }
    public StatField<int, int> baseAtk { get; private set; }
    public StatField<int, int> baseMatk { get; private set; }
    public StatField<int, int> baseDef { get; private set; }
    public StatField<int, int> baseMdef { get; private set; }
    public StatField<int, int> baseAgi { get; private set; }
    public StatField<int, int> baseLuck { get; private set; }

    // Dictionary to mantain list of currently equiped equipments
    public Dictionary<string, Equipment> equipments { get; private set; }
    public List<string> slots { get; private set; }

    public List<Skill> skills;
    public Skill skill1 { get; private set; }
    public Skill skill2 { get; private set; }
    public Skill skill3 { get; private set; }

    // Getters for base properties
    public int MaxExp => (int)(maxExp.init + (maxExp.final - maxExp.init) * Mathf.Pow((float)curLvl / maxLvl, maxExp.pow));
    public int BaseMHP => (int)(baseMhp.init + (baseMhp.final - baseMhp.init) * Mathf.Pow((float)curLvl / maxLvl, baseMhp.pow));
    public int BaseATK => (int)(baseAtk.init + (baseAtk.final - baseAtk.init) * Mathf.Pow((float)curLvl / maxLvl, baseAtk.pow));
    public int BaseMATK => (int)(baseMatk.init + (baseMatk.final - baseMatk.init) * Mathf.Pow((float)curLvl / maxLvl, baseMatk.pow));
    public int BaseDEF => (int)(baseDef.init + (baseDef.final - baseDef.init) * Mathf.Pow((float)curLvl / maxLvl, baseDef.pow));
    public int BaseMDEF => (int)(baseMdef.init + (baseMdef.final - baseMdef.init) * Mathf.Pow((float)curLvl / maxLvl, baseMdef.pow));
    public int BaseAGI => (int)(baseAgi.init + (baseAgi.final - baseAgi.init) * Mathf.Pow((float)curLvl / maxLvl, baseAgi.pow));
    public int BaseLUCK => (int)(baseLuck.init + (baseLuck.final - baseLuck.init) * Mathf.Pow((float)curLvl / maxLvl, baseLuck.pow));

    // Getters for buffs
    public int MHPBuff => equipments.Values.Sum(e => e?.hpBuff ?? 0);
    public int ATKBuff => equipments.Values.Sum(e => e?.atkBuff ?? 0);
    public int MATKBuff => equipments.Values.Sum(e => e?.matkBuff ?? 0);
    public int DEFBuff => equipments.Values.Sum(e => e?.defBuff ?? 0);
    public int MDEFBuff => equipments.Values.Sum(e => e?.mdefBuff ?? 0);
    public int AGIBuff => equipments.Values.Sum(e => e?.agiBuff ?? 0);
    public int LUCKBuff => equipments.Values.Sum(e => e?.luckBuff ?? 0);

    // Getters for properties with buff
    public int MHP => BaseMHP + MHPBuff;
    public int ATK => BaseATK + ATKBuff;
    public int MATK => BaseMATK + MATKBuff;
    public int DEF => BaseDEF + DEFBuff;
    public int MDEF => BaseMDEF + MDEFBuff;
    public int AGI => BaseAGI + AGIBuff;
    public int LUCK => BaseLUCK + LUCKBuff;


    /// <summary> To create a character object from a SO</summary>
    /// <param name="character"> The SO of the character you want to create</param>
    public Character(CharacterSO character) {

        characterName = character.characterName;
        characterClass = character.characterClass;

        curLvl = character.curLvl;
        maxLvl = character.maxLvl;

        maxExp = character.maxExp;

        baseMhp = character.baseMhp;
        baseAtk = character.baseAtk;
        baseMatk = character.baseMatk;
        baseDef = character.baseDef;
        baseMdef = character.baseMdef;
        baseAgi = character.baseAgi;
        baseLuck = character.baseLuck;

        canDualWeild = character.canDualWeild;

        skills = character.skills.Select(s => Skill.Create(s)).ToList();

        equipments = new Dictionary<string, Equipment>();
        initEqupments(character);

        curExp = 0;
        curHp = MHP;
    }


    /// <summary>To take damage on hit</summary>
    /// <param name="damage">Value of damage</param>
    public void TakeDamage(int damage) {
        if (damage < 0) return;
        if (damage == 0) damage = 1;

        curHp -= damage;
        if (curHp < 0) curHp = 0;
    }


    /// <summary>To equip an equipment</summary>
    /// <param name="equipment">Equipment you want it to equip</param>
    public void Equip(Equipment equipment) {
        // if (InventoryManager.Instance.SlotCount(equipment.equipmentName) == 0)
        //     return;
        if (equipment == null)
            return;

        if (!CanEquip(equipment)) {
            Debug.Log("This character can't equip" + equipment.equipmentName);
            return;
        }

        switch (equipment.slot) {
            case SlotType.Head:
                equipments["head"] = equipment;
                break;
            case SlotType.Body:
                equipments["body"] = equipment;
                break;
            case SlotType.Hand1:
                equipments["hand1"] = equipment;
                break;
            case SlotType.Hand2:
                equipments["hand2"] = equipment;
                break;
            case SlotType.Boots:
                equipments["boots"] = equipment;
                break;
            case SlotType.Accessory:
                equipments["accessory"] = equipment;
                break;
        }
    }

    /// <summary>To unequip an equipment</summary>
    /// <param name="slot">Equipment slot you want it to unequip</param>
    public void Unequip(SlotType slot) {
        switch (slot) {
            case SlotType.Head:
                if (equipments["head"] == null) return;
                equipments["head"] = null;
                // equipments["head"].isEquiped = false;
                break;
            case SlotType.Body:
                if (equipments["body"] == null) return;
                equipments["body"] = null;
                // equipments["body"].isEquiped = false;
                break;
            case SlotType.Hand1:
                if (equipments["hand1"] == null) return;
                equipments["hand1"] = null;
                // equipments["hand1"].isEquiped = false;
                break;
            case SlotType.Hand2:
                if (equipments["hand2"] == null) return;
                equipments["hand2"] = null;
                // equipments["hand2"].isEquiped = false;
                break;
            case SlotType.Boots:
                if (equipments["boots"] == null) return;
                equipments["boots"] = null;
                // equipments["boots"].isEquiped = false;
                break;
            case SlotType.Accessory:
                if (equipments["accessory"] == null) return;
                equipments["accessory"] = null;
                // equipments["accessory"].isEquiped = false;
                break;
        }
    }

    public void EquipSkill(Skill skill, int slot) {
        if (slot == 1) skill1 = skill;
        else if (slot == 2) skill2 = skill;
        else if (slot == 3) skill3 = skill;

        foreach (Skill s in skills) {
            if (s.skillName == skill.skillName) s.slot = slot;
        }
    }

    public void UnequipSkill(int slot) {
        Skill skill = null;
        if (slot == 1) {
            skill = skill1;
            skill1 = null;
        }
        else if (slot == 2) {
            skill = skill2;
            skill2 = null;
        }
        else if (slot == 3) {
            skill = skill3;
            skill3 = null;
        }

        if (skill == null) return;
        foreach (Skill s in skills) {
            if (s.skillName == skill.skillName) s.slot = -1;
        }
    }

    public bool CanEquip(Equipment e) {
        if (e == null) return false;

        foreach (Class c in e.validClasses) {
            if (c == characterClass) return true;
        }

        return false;
    }

    public Equipment GetEquipment(SlotType slot) {
        switch (slot) {
            case SlotType.Head:
                return equipments["head"];
            case SlotType.Body:
                return equipments["body"];
            case SlotType.Boots:
                return equipments["boots"];
            case SlotType.Accessory:
                return equipments["accessory"];
            case SlotType.Hand1:
                return equipments["hand1"];
            case SlotType.Hand2:
                return equipments["hand2"];
            default:
                return null;
        }
    }


    /// <summary>To show stats of the character, useful while debugging</summary>
    public void ShowStats() {
        Debug.Log("maxHealth: " + BaseMHP + " + " + MHPBuff);
        Debug.Log("attack: " + BaseATK + " + " + ATKBuff);
        Debug.Log("magicAttack: " + BaseMATK + " + " + MATKBuff);
        Debug.Log("defence: " + BaseDEF + " + " + DEFBuff);
        Debug.Log("magicDefence: " + BaseMDEF + " + " + MDEFBuff);
        Debug.Log("agility: " + BaseAGI + " + " + AGIBuff);
        Debug.Log("luck: " + BaseLUCK + " + " + LUCKBuff);
    }

    private void initEqupments(CharacterSO character) {
        equipments["head"] = Equipment.Create(character.head, true);
        equipments["body"] = Equipment.Create(character.body, true);
        equipments["hand1"] = Equipment.Create(character.hand1, true);
        equipments["hand2"] = Equipment.Create(character.hand2, true);
        equipments["boots"] = Equipment.Create(character.boots, true);
        equipments["accessory"] = Equipment.Create(character.accessory, true);
    }

    public List<Equipment> GetEquipmentsList() {
        List<Equipment> e = Enumerable.Repeat<Equipment>(null, 6).ToList();

        e[(int)SlotType.Head] = equipments.ContainsKey("head") ? equipments["head"] : null;
        e[(int)SlotType.Body] = equipments.ContainsKey("body") ? equipments["body"] : null;
        e[(int)SlotType.Boots] = equipments.ContainsKey("boots") ? equipments["boots"] : null;
        e[(int)SlotType.Accessory] = equipments.ContainsKey("accessory") ? equipments["accessory"] : null;
        e[(int)SlotType.Hand1] = equipments.ContainsKey("hand1") ? equipments["hand1"] : null;
        e[(int)SlotType.Hand2] = equipments.ContainsKey("hand2") ? equipments["hand2"] : null;

        return e;
    }

    public void SetCurHp(float hp) {
        hp = Mathf.Clamp(hp, 0, MHP);
        curHp = (int)hp;
    }
}