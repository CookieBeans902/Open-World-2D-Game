using UnityEngine;
using TMPro;
using System.Collections.Generic;
using UnityEngine.UI;
using Unity.VisualScripting;
using System.Linq;
using System;

public class StatsUIManager : MonoBehaviour
{
    public event EventHandler OnSkillChange;
    public event EventHandler OnStatChange;

    public static StatsUIManager Instance;

    // prefab of the inventoryUI slot
    [SerializeField] private GameObject slotPref;
    [SerializeField] private GameObject skillSlotPref;

    // To mantain a pool of slots
    private Queue<Transform> slotPool = new Queue<Transform>();
    private Queue<Transform> skillSlotPool = new Queue<Transform>();

    List<Transform> equipSlots = Enumerable.Repeat<Transform>(null, 6).ToList();


    [SerializeField] private GameObject uiPref;
    [SerializeField] private GameObject fieldPref;
    [SerializeField] private GameObject itemUiPref;
    [SerializeField] private GameObject skillUiPref;
    private Dictionary<string, GameObject> fields = new Dictionary<string, GameObject>();
    private GameObject statsUI;
    private Character charData;
    private bool isActive;
    private int currIndex;
    private Animator activeAnimator;
    private GameObject curSelected;

    private void Awake()
    {
        if (StatsUIManager.Instance == null)
        {
            // this is to make this object persist across scenes
            DontDestroyOnLoad(gameObject);
            Instance = this;

            currIndex = 0;
            isActive = false;
        }
        else if (StatsUIManager.Instance != this)
        {
            // destroy self if an instance is already present to ensure there is only one manager
            Destroy(gameObject);
            return;
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (!isActive)
            {
                statsUI = Instantiate(uiPref);

                statsUI.GetComponent<StatsUI>().slot1.slot = 1;
                statsUI.GetComponent<StatsUI>().slot2.slot = 2;
                statsUI.GetComponent<StatsUI>().slot3.slot = 3;

                equipSlots[(int)SlotType.Head] = statsUI.GetComponent<StatsUI>().headSlot;
                equipSlots[(int)SlotType.Body] = statsUI.GetComponent<StatsUI>().bodySlot;
                equipSlots[(int)SlotType.Boots] = statsUI.GetComponent<StatsUI>().bootSlot;
                equipSlots[(int)SlotType.Accessory] = statsUI.GetComponent<StatsUI>().accessorySlot;
                equipSlots[(int)SlotType.Hand1] = statsUI.GetComponent<StatsUI>().hand1Slot;
                equipSlots[(int)SlotType.Hand2] = statsUI.GetComponent<StatsUI>().hand2Slot;

                // Button prevButton = statsUI.GetComponent<StatsUI>().prevButton;
                // Button nextButton = statsUI.GetComponent<StatsUI>().nextButton;
                // nextButton.onClick.AddListener(() => {
                //     int length = CharacterManager.Instance.characters.Count;
                //     if (currIndex < length - 1) {
                //         currIndex++;
                //         statsUI.GetComponent<StatsUI>().desc.text = "";
                //         UpdateUI();
                //     }
                // });
                // prevButton.onClick.AddListener(() => {
                //     if (currIndex > 0) {
                //         currIndex--;
                //         statsUI.GetComponent<StatsUI>().desc.text = "";
                //         UpdateUI();
                //     }
                // });

                Button close = statsUI.GetComponent<StatsUI>().closeButton;
                close.onClick.RemoveAllListeners();
                close.onClick.AddListener(() => ClearUI());

                isActive = true;
                UpdateUI();
            }
            else
            {
                ClearUI();
            }
        }
    }


    /// <summary> To update the ui </summary>
    public void UpdateUI()
    {
        if (statsUI == null)
            return;
        if (fields == null)
            return;
        if (!isActive)
            return;

        // Updating character stats
        charData = CharacterManager.Instance.characters[currIndex];

        InitFields();
        UpdateInventory();

        UpdateEquipments();
        UpdateStats();

        UpdateSkills();


        Canvas.ForceUpdateCanvases();
    }

    /// <summary> To remove the ui from the screen</summary>
    private void ClearUI()
    {
        GameObject toDestroy = statsUI;
        statsUI = null;
        toDestroy.SetActive(false);
        Destroy(toDestroy);
        // descPool = new Queue<Transform>();
        fields = new Dictionary<string, GameObject>();
        // pickups = new List<GameObject>();
        slotPool = new Queue<Transform>();
        skillSlotPool = new Queue<Transform>();
        isActive = false;
    }


    /// <summary> To set field positions and pickups </summary>
    private void InitFields()
    {
        if (statsUI == null)
        {
            return;
        }

        if (fields == null || fields.Count == 0)
        {
            fields = new Dictionary<string, GameObject>();
            fields["Max Health"] = Instantiate(fieldPref, statsUI.GetComponent<StatsUI>().fieldsContent);
            fields["Attack"] = Instantiate(fieldPref, statsUI.GetComponent<StatsUI>().fieldsContent);
            fields["Magic Attack"] = Instantiate(fieldPref, statsUI.GetComponent<StatsUI>().fieldsContent);
            fields["Defence"] = Instantiate(fieldPref, statsUI.GetComponent<StatsUI>().fieldsContent);
            fields["Magic Defence"] = Instantiate(fieldPref, statsUI.GetComponent<StatsUI>().fieldsContent);
            fields["Agility"] = Instantiate(fieldPref, statsUI.GetComponent<StatsUI>().fieldsContent);
            fields["Luck"] = Instantiate(fieldPref, statsUI.GetComponent<StatsUI>().fieldsContent);
        }
    }

    public void UpdateInventory()
    {
        Dictionary<string, InventoryItem> dict = InventoryManager.Instance.items;
        if (dict == null) return;

        foreach (Transform islot in statsUI.GetComponent<StatsUI>().inventoryContent) ReturnSlotToPool(islot);
        List<ActiveItemSlot> activeSlots = statsUI.GetComponent<StatsUI>().activeSlots;

        for (int i = 0; i < activeSlots.Count; i++)
        {
            foreach (Transform ui in activeSlots[i].contentBox) Destroy(ui.gameObject);
            activeSlots[i].count.text = "";
            activeSlots[i].item = null;
        }

        ItemSlot slot;
        foreach (KeyValuePair<string, InventoryItem> p in dict)
        {
            InventoryItem i = p.Value;
            if (i.isActive) continue;

            int count = i.count;
            int maxStack = i.maxStack;
            Sprite icon = i.icon;

            while (count > 0)
            {
                slot = GetSlotFromPool().GetComponent<ItemSlot>();
                slot.item = i;

                ItemUI itemUI = Instantiate(itemUiPref, slot.contentBox).GetComponent<ItemUI>();
                itemUI.GetComponent<Image>().sprite = icon;
                itemUI.item = i;
                itemUI.isActive = false;

                slot.count.text = (count >= maxStack ? maxStack : count).ToString();
                count -= maxStack;
            }

            if (i.slotNumber != -1)
            {
                ActiveItemSlot activeSlot = statsUI.GetComponent<StatsUI>().activeSlots[i.slotNumber - 1];
                activeSlot.item = i;
                activeSlot.count.text = i.count.ToString();

                ItemUI itemUI = Instantiate(itemUiPref, activeSlot.contentBox).GetComponent<ItemUI>();
                itemUI.GetComponent<Image>().sprite = icon;
                itemUI.item = i;
                itemUI.isActive = true;
            }
        }

        InventoryManager.Instance.TriggerChange();
    }


    /// <summary> To update the equpments ui in the slots</summary>
    public void UpdateEquipments()
    {
        List<Equipment> equips = charData.GetEquipmentsList();

        for (int i = 0; i < equips.Count; i++)
        {
            EquipmentSlot slot = equipSlots[i].GetComponent<EquipmentSlot>();
            foreach (Transform ui in slot.contentBox) Destroy(ui.gameObject);

            Equipment equip = equips[i];

            if (equip != null)
            {
                slot.item = equip.item;

                ItemUI itemUI = Instantiate(itemUiPref, slot.contentBox).GetComponent<ItemUI>();
                itemUI.GetComponent<Image>().sprite = equip.item.icon;
                itemUI.item = equip.item;
            }
            else
            {
                slot.item = null;
            }
        }
    }

    public void UpdateSkills()
    {
        SkillSlot slot1 = statsUI.GetComponent<StatsUI>().slot1;
        SkillSlot slot2 = statsUI.GetComponent<StatsUI>().slot2;
        SkillSlot slot3 = statsUI.GetComponent<StatsUI>().slot3;

        foreach (Transform slot in statsUI.GetComponent<StatsUI>().skillContent) ReturnSkillSlotToPool(slot);
        foreach (Transform ui in slot1.contentBox) Destroy(ui.gameObject);
        foreach (Transform ui in slot2.contentBox) Destroy(ui.gameObject);
        foreach (Transform ui in slot3.contentBox) Destroy(ui.gameObject);
        List<Skill> skills = charData.skills;

        foreach (Skill skill in skills)
        {
            if (skill.slot == -1)
            {
                SkillSlot slot = GetSkillSlotFromPool().GetComponent<SkillSlot>();
                slot.skill = skill;

                SkillUI skillUI = Instantiate(skillUiPref, slot.contentBox).GetComponent<SkillUI>();
                skillUI.skill = skill;
                skillUI.slot = -1;
                skillUI.GetComponent<Image>().sprite = skill.icon;
            }
            else
            {
                SkillSlot slot = null;

                if (skill.slot == 1) slot = slot1;
                else if (skill.slot == 2) slot = slot2;
                else if (skill.slot == 3) slot = slot3;

                if (slot == null) continue;

                slot.skill = skill;

                SkillUI skillUI = Instantiate(skillUiPref, slot.contentBox).GetComponent<SkillUI>();
                skillUI.skill = skill;
                skillUI.slot = -1;
                skillUI.GetComponent<Image>().sprite = skill.icon;
            }
        }

        OnSkillChange?.Invoke(this, EventArgs.Empty);
    }

    public void UpdateStats()
    {
        string charName = charData.characterName;
        statsUI.GetComponent<StatsUI>().nameText.text = charName;
        string charLevel = $"{charData.characterClass} (Level {charData.curLvl})";
        statsUI.GetComponent<StatsUI>().levelText.text = charLevel;

        UpdateField("Max Health", charData.BaseMHP, charData.MHP);
        UpdateField("Attack", charData.BaseATK, charData.ATK);
        UpdateField("Magic Attack", charData.BaseMATK, charData.MATK);
        UpdateField("Defence", charData.BaseDEF, charData.DEF);
        UpdateField("Magic Defence", charData.BaseMDEF, charData.MDEF);
        UpdateField("Agility", charData.BaseAGI, charData.AGI);
        UpdateField("Luck", charData.BaseLUCK, charData.LUCK);

        OnStatChange?.Invoke(this, EventArgs.Empty);
    }

    /// <summary> To update the value of a field </summary>
    /// <params name="name">name of the field</params>
    /// <params name="baseValue">value without equipment buffs</params>
    /// <params name="value">finnal value after adding equipment buffs</params>
    public void UpdateField(string name, int baseValue, int value)
    {
        if (fields == null)
            fields = new Dictionary<string, GameObject>();

        GameObject field = fields[name];
        TextMeshProUGUI text = field.GetComponent<TextMeshProUGUI>();
        name = $"<color=white>{name}</color>";
        string baseVal = $"<color=white>{baseValue}</color>";
        string buffVal = $"<color=green>{value - baseValue}</color>";
        text.text = $"{name} : {baseVal} + {buffVal}";

        fields[name] = field;
    }


    /// <summary>To show an item description when it is clicked</summary>
    public void ShowItemDesc(InventoryItem item)
    {
        if (statsUI == null || item == null) return;

        string desc = $"{item.itemName}\n";

        if (item.itemType == ItemType.Equipment)
        {
            Equipment e = Equipment.Create(item.equipment);
            desc += e.hpBuff != 0 ? $"HP + {e.hpBuff}\n" : "";
            desc += e.defBuff != 0 ? $"DEF + {e.defBuff}\n" : "";
            desc += e.atkBuff != 0 ? $"ATK + {e.atkBuff}\n" : "";
            desc += e.mdefBuff != 0 ? $"MDEF + {e.mdefBuff}\n" : "";
            desc += e.matkBuff != 0 ? $"MATK + {e.matkBuff}\n" : "";
            desc += e.agiBuff != 0 ? $"AGI + {e.agiBuff}\n" : "";
            desc += e.luckBuff != 0 ? $"LUCK + {e.luckBuff}" : "";
        }
        else
        {
            desc += item.itemDesc;
        }
        statsUI.GetComponent<StatsUI>().desc.text = desc;
    }

    public void ShowSkillDesc(Skill skill)
    {
        if (statsUI == null || skill == null) return;

        string desc = $"{skill.skillName}\n";
        desc += skill.skillDesc;

        statsUI.GetComponent<StatsUI>().desc.text = desc;
    }


    /// <summary>To equip an equipment in a slot</summary>
    public void EquipSlot(Equipment equipment)
    {
        if (charData == null || equipment == null) return;

        Equipment prevEquipment = charData.GetEquipment(equipment.slot);
        if (prevEquipment != null)
        {
            prevEquipment.item.isActive = false;
            InventoryManager.Instance.AddItem(prevEquipment.item, 1);
        }

        charData.Equip(equipment);
        InventoryManager.Instance.RemoveItem(equipment.item.itemName, 1);
        equipment.item.isActive = true;
    }

    /// <summary>To unequip an equipment from a slot</summary>
    public void UnequipSlot(Equipment equipment)
    {
        if (charData == null || equipment == null) return;

        charData.Unequip(equipment.slot);
        InventoryManager.Instance.AddItem(equipment.item, 1);
        equipment.item.isActive = false;
    }


    /// <summary>To equip an skill in a slot</summary>
    public void EquipSkill(Skill skill, int slot)
    {
        if (charData == null || skill == null) return;

        SkillSlot skillSlot = null;

        if (slot == 1)
            skillSlot = statsUI.GetComponent<StatsUI>().slot1;
        else if (slot == 2)
            skillSlot = statsUI.GetComponent<StatsUI>().slot2;
        else if (slot == 3)
            skillSlot = statsUI.GetComponent<StatsUI>().slot3;

        if (skillSlot != null)
        {
            Skill prevSkill = skillSlot.skill;
            if (prevSkill != null)
            {
                if (skill.slot == -1)
                {
                    charData.UnequipSkill(slot);
                }
                else
                {
                    if (prevSkill.slot == 1) statsUI.GetComponent<StatsUI>().slot1.skill = prevSkill;
                    else if (prevSkill.slot == 2) statsUI.GetComponent<StatsUI>().slot2.skill = prevSkill;
                    else if (prevSkill.slot == 3) statsUI.GetComponent<StatsUI>().slot3.skill = prevSkill;
                    prevSkill.slot = skill.slot;
                    charData.EquipSkill(prevSkill, skill.slot);
                }
            }

            skill.slot = slot;
            skillSlot.skill = skill;
            charData.EquipSkill(skill, slot);
        }
    }

    /// <summary>To unequip an skill from a slot</summary>
    public void UnequipSkill(Skill skill, int slot)
    {
        if (charData == null || skill == null) return;
        SkillSlot skillSlot = null;

        if (slot == 1)
            skillSlot = statsUI.GetComponent<StatsUI>().slot1;
        else if (slot == 2)
            skillSlot = statsUI.GetComponent<StatsUI>().slot2;
        else if (slot == 3)
            skillSlot = statsUI.GetComponent<StatsUI>().slot3;

        if (skillSlot != null)
        {
            skill.slot = -1;
            skillSlot.skill = null;
            charData.UnequipSkill(slot);
        }
    }

    /// <summary>To equip an equipment in a slot</summary>
    public void SetActiveItem(InventoryItem item, int keyNumber)
    {
        if (charData == null || item == null) return;
        ActiveItemSlot newSlot = statsUI.GetComponent<StatsUI>().activeSlots[keyNumber - 1];

        if (item.slotNumber != -1)
        {
            if (newSlot.item != null)
            {
                ActiveItemSlot exchangeSlot = statsUI.GetComponent<StatsUI>().activeSlots[item.slotNumber - 1];

                newSlot.item.slotNumber = item.slotNumber;

                exchangeSlot.count.text = newSlot.item.count.ToString();
                exchangeSlot.item = newSlot.item;
            }
            else
            {

            }
        }

        item.slotNumber = keyNumber;
        newSlot.count.text = item.count.ToString();
        newSlot.item = item;
    }

    /// <summary>To unequip an equipment from a slot</summary>
    public void RemoveActiveItem(InventoryItem item)
    {
        if (charData == null || item == null || item.slotNumber == -1) return;

        ActiveItemSlot slot = statsUI.GetComponent<StatsUI>().activeSlots[item.slotNumber - 1];
        item.slotNumber = -1;
        slot.count.text = "";
        slot.item = null;
    }


    /// <summary> To get a slot from the slot pool</summary>
    /// <returns>A slot from the slot pool</returns>
    private Transform GetSlotFromPool()
    {
        if (slotPool.Count > 0)
        {
            Transform slot = slotPool.Dequeue();
            slot.gameObject.SetActive(true);
            return slot;
        }
        else
        {
            return Instantiate(slotPref, statsUI.GetComponent<StatsUI>().inventoryContent).transform;
        }
    }


    /// <summary> To return a slot back to the pool </summary>
    /// <params name="slot"> The slot gameobject you want to return </params>
    private void ReturnSlotToPool(Transform slot)
    {
        ItemSlot s = slot.GetComponent<ItemSlot>();
        s.item = null;
        foreach (Transform ui in s.contentBox) Destroy(ui.gameObject);
        slot.gameObject.SetActive(false);
        slotPool.Enqueue(slot);
    }

    /// <summary> To get a skill slot from the slot pool</summary>
    /// <returns>A slot from the slot pool</returns>
    private Transform GetSkillSlotFromPool()
    {
        if (skillSlotPool.Count > 0)
        {
            Transform slot = skillSlotPool.Dequeue();
            slot.gameObject.SetActive(true);
            return slot;
        }
        else
        {
            return Instantiate(skillSlotPref, statsUI.GetComponent<StatsUI>().skillContent).transform;
        }
    }

    /// <summary> To return a skill slot back to the pool </summary>
    /// <params name="slot"> The slot gameobject you want to return </params>
    private void ReturnSkillSlotToPool(Transform slot)
    {
        SkillSlot s = slot.GetComponent<SkillSlot>();
        s.skill = null;
        foreach (Transform ui in s.contentBox) Destroy(ui.gameObject);
        slot.gameObject.SetActive(false);
        skillSlotPool.Enqueue(slot);
    }

    public void TriggerSkillChange()
    {
        OnSkillChange?.Invoke(this, EventArgs.Empty);
    }

    public void TriggerStatChange()
    {
        OnStatChange?.Invoke(this, EventArgs.Empty);
    }
}
