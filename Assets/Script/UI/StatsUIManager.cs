using UnityEngine;
using TMPro;
using System.Collections.Generic;
using UnityEngine.UI;
using Unity.VisualScripting;
using System.Linq;

public class StatsUIManager : MonoBehaviour {
    public static StatsUIManager Instance;

    // prefab of the inventoryUI slot
    [SerializeField] private GameObject slotPref;
    [SerializeField] private GameObject skillSlotPref;

    // To mantain a pool of slots
    private Queue<Transform> slotPool = new Queue<Transform>();
    private Queue<Transform> skillSlotPool = new Queue<Transform>();


    [SerializeField] private GameObject uiPref;
    [SerializeField] private GameObject fieldPref;
    private Dictionary<string, GameObject> fields = new Dictionary<string, GameObject>();
    private GameObject statsUI;
    private Character charData;
    private bool isActive;
    private int currIndex;
    private Animator activeAnimator;
    private GameObject curSelected;


    private void Awake() {
        if (StatsUIManager.Instance == null) {
            // this is to make this object persist across scenes
            DontDestroyOnLoad(gameObject);
            Instance = this;

            currIndex = 0;
            isActive = false;
        }
        else if (StatsUIManager.Instance != this) {
            // destroy self if an instance is already present to ensure there is only one manager
            Destroy(gameObject);
            return;
        }
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.E)) {
            if (!isActive) {
                statsUI = Instantiate(uiPref);
                Button prevButton = statsUI.GetComponent<StatsUI>().prevButton;
                Button nextButton = statsUI.GetComponent<StatsUI>().nextButton;
                nextButton.onClick.AddListener(() => {
                    int length = CharacterManager.Instance.characters.Count;
                    currIndex = currIndex < length - 1 ? currIndex + 1 : currIndex;
                    UpdateUI();
                });
                prevButton.onClick.AddListener(() => {
                    currIndex = currIndex > 0 ? currIndex - 1 : currIndex;
                    UpdateUI();
                });

                Button close = statsUI.GetComponent<StatsUI>().closeButton;
                close.onClick.RemoveAllListeners();
                close.onClick.AddListener(() => ClearUI());

                isActive = true;
                UpdateUI();
            }
            else {
                ClearUI();
            }
        }
    }


    /// <summary> To update the ui </summary>
    public void UpdateUI() {
        if (statsUI == null)
            return;
        if (fields == null)
            return;
        if (!isActive)
            return;

        // Updating character stats
        charData = CharacterManager.Instance.characters[currIndex];

        InitFieldsAndEquips();
        UpdateEquipments();

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


        // Updating the inventory
        Dictionary<string, InventoryItem> dict = InventoryManager.Instance.items;
        if (dict == null) return;

        foreach (Transform slot in statsUI.GetComponent<StatsUI>().inventoryContent) ReturnSlotToPool(slot);

        ItemSlot item;

        foreach (KeyValuePair<string, InventoryItem> p in dict) {
            int count = p.Value.count;
            int maxStack = p.Value.maxStack;
            InventoryItem i = p.Value;

            while (count > 0) {
                item = GetSlotFromPool().GetComponent<ItemSlot>();
                item.icon.sprite = p.Value.icon;
                item.count.text = (count >= maxStack ? maxStack : count).ToString();
                count -= maxStack;

                Animator anim = item.animator;
                GameObject select = item.selected;

                item.onSingleClick.RemoveAllListeners();
                item.onSingleClick.AddListener(() => {
                    if (activeAnimator != item.animator) {
                        if (activeAnimator != null) activeAnimator.enabled = false;
                        activeAnimator = anim;
                        anim.enabled = true;

                        if (curSelected != null) curSelected.SetActive(false);
                        curSelected = select;
                        select.SetActive(true);

                        ShowItemDesc(i);
                    }
                });

                item.onDoubleClick.RemoveAllListeners();
                item.onDoubleClick.AddListener(() => {
                    if (activeAnimator != null) activeAnimator.enabled = false;
                    if (curSelected != null) curSelected.SetActive(false);

                    ReturnSlotToPool(item.transform);
                    EquipSlot(Equipment.Create(i.equipment));
                });
            }
        }

        // Update the skills
        foreach (Transform slot in statsUI.GetComponent<StatsUI>().skillContent) ReturnSkillSlotToPool(slot);
        List<Skill> skills = charData.skills;

        foreach (Skill skill in skills) {
            SkillSlot skillSlot = GetSkillSlotFromPool().GetComponent<SkillSlot>();

            skillSlot.icon.sprite = skill.icon;

            string name = skill.skillName;
            string desc = skill.skillDesc;
            GameObject select = skillSlot.selected;

            Character ch = charData;

            skillSlot.onSingleClick.RemoveAllListeners();
            skillSlot.onSingleClick.AddListener(() => {
                if (activeAnimator != null) activeAnimator.enabled = false;

                if (!select.activeSelf) {
                    if (curSelected != null) curSelected.SetActive(false);
                    curSelected = select;
                    select.SetActive(true);
                }

                statsUI.GetComponent<StatsUI>().desc.text = $"{name}\n{desc}";
            });

            skillSlot.onDoubleClick.RemoveAllListeners();
            skillSlot.onDoubleClick.AddListener(() => {
                if (activeAnimator != null) activeAnimator.enabled = false;
                if (curSelected != null) curSelected.SetActive(false);
            });
        }

        Canvas.ForceUpdateCanvases();
    }

    /// <summary> To remove the ui from the screen</summary>
    private void ClearUI() {
        GameObject toDestroy = statsUI;
        statsUI = null;
        toDestroy.SetActive(false);
        Destroy(toDestroy);
        // descPool = new Queue<Transform>();
        fields = new Dictionary<string, GameObject>();
        // pickups = new List<GameObject>();
        slotPool = new Queue<Transform>();
        isActive = false;
    }


    /// <summary> To update the equpments ui in the slots</summary>
    private void UpdateEquipments() {
        List<Transform> slots = new List<Transform>();
        List<Equipment> equips = charData.equipments.Values.ToList();

        slots.Add(statsUI.GetComponent<StatsUI>().headSlot);
        slots.Add(statsUI.GetComponent<StatsUI>().bodySlot);
        slots.Add(statsUI.GetComponent<StatsUI>().hand2Slot);
        slots.Add(statsUI.GetComponent<StatsUI>().hand1Slot);
        slots.Add(statsUI.GetComponent<StatsUI>().bootSlot);
        slots.Add(statsUI.GetComponent<StatsUI>().AccessorySlot);

        for (int i = 0; i < equips.Count; i++) {
            EquipmentSlot slot = slots[i].GetComponent<EquipmentSlot>();
            Equipment e = equips[i];

            if (e != null) {
                GameObject equipmentUI = e.equipmentUI;
                equipmentUI.GetComponent<RectTransform>().anchoredPosition = slot.GetComponent<RectTransform>().anchoredPosition;

                // Animator anim = slot.animator;
                // GameObject select = slot.selected;
                // SlotType slotType = e.slot;

                // slot.onSingleClick.RemoveAllListeners();
                // slot.onSingleClick.AddListener(() => {
                //     if (activeAnimator != slot.animator) {
                //         if (activeAnimator != null) activeAnimator.enabled = false;
                //         activeAnimator = anim;
                //         anim.enabled = true;

                //         if (curSelected != null) curSelected.SetActive(false);
                //         curSelected = select;
                //         select.SetActive(true);

                //         ShowItemDesc(e.item);
                //     }
                //     UpdateUI();
                // });

                // slot.onDoubleClick.RemoveAllListeners();
                // slot.onDoubleClick.AddListener(() => {
                //     if (activeAnimator != null) activeAnimator.enabled = false;
                //     if (curSelected != null) curSelected.SetActive(false);
                //     UnequipSlot(e);
                // });
            }
            else {
                // slot.icon.sprite = null;
                // slot.icon.color = new Color().WithAlpha(0);

                // if (activeAnimator != null) activeAnimator.enabled = false;
                // if (curSelected != null) curSelected.SetActive(false);
                // slot.onSingleClick.RemoveAllListeners();
                // slot.onDoubleClick.RemoveAllListeners();
            }
        }
    }


    /// <summary> To update the value of a field </summary>
    /// <params name="name">name of the field</params>
    /// <params name="baseValue">value without equipment buffs</params>
    /// <params name="value">finnal value after adding equipment buffs</params>
    private void UpdateField(string name, int baseValue, int value) {
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


    /// <summary> To set field positions and pickups </summary>
    private void InitFieldsAndEquips() {
        if (statsUI == null) {
            return;
        }

        if (fields == null || fields.Count == 0) {
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

    /// <summary>To show an item description when it is clicked</summary>
    private void ShowItemDesc(InventoryItem item) {
        if (statsUI == null || item == null) return;

        string desc = "";

        if (item.itemType == ItemType.Equipment) {
            Equipment e = Equipment.Create(item.equipment);
            desc += e.hpBuff != 0 ? $"HP + {e.hpBuff}\n" : "";
            desc += e.defBuff != 0 ? $"DEF + {e.defBuff}\n" : "";
            desc += e.atkBuff != 0 ? $"ATK + {e.atkBuff}\n" : "";
            desc += e.mdefBuff != 0 ? $"MDEF + {e.mdefBuff}\n" : "";
            desc += e.matkBuff != 0 ? $"MATK + {e.matkBuff}\n" : "";
            desc += e.agiBuff != 0 ? $"AGI + {e.agiBuff}\n" : "";
            desc += e.luckBuff != 0 ? $"LUCK + {e.luckBuff}" : "";
        }
        else {
            desc = item.itemDesc;
        }
        statsUI.GetComponent<StatsUI>().desc.text = desc;
    }


    /// <summary>To equip a slot</summary>
    private void EquipSlot(Equipment equipment) {
        if (charData == null || equipment == null) return;

        Equipment prevEquip = charData.GetEquipment(equipment.slot);
        if (prevEquip != null) {
            charData.Unequip(prevEquip.slot);
            InventoryManager.Instance.AddItem(prevEquip.item, 1);
        }

        charData.Equip(equipment);
        InventoryManager.Instance.RemoveItem(equipment.item.itemName, 1);

        UpdateUI();
    }

    /// <summary>To unequip a slot</summary>
    private void UnequipSlot(Equipment equipment) {
        if (charData == null || equipment == null) return;

        charData.Unequip(equipment.slot);
        InventoryManager.Instance.AddItem(equipment.item, 1);

        UpdateUI();
    }

    /// <summary> To get a slot from the slot pool</summary>
    /// <returns>A slot from the slot pool</returns>
    private Transform GetSlotFromPool() {
        if (slotPool.Count > 0) {
            Transform slot = slotPool.Dequeue();
            slot.gameObject.SetActive(true);
            return slot;
        }
        else {
            return Instantiate(slotPref, statsUI.GetComponent<StatsUI>().inventoryContent).transform;
        }
    }


    /// <summary> To return a slot back to the pool </summary>
    /// <params name="slot"> The slot gameobject you want to return </params>
    private void ReturnSlotToPool(Transform slot) {
        slot.gameObject.SetActive(false);
        slotPool.Enqueue(slot);
    }

    /// <summary> To get a skill slot from the slot pool</summary>
    /// <returns>A slot from the slot pool</returns>
    private Transform GetSkillSlotFromPool() {
        if (skillSlotPool.Count > 0) {
            Transform slot = skillSlotPool.Dequeue();
            slot.gameObject.SetActive(true);
            return slot;
        }
        else {
            return Instantiate(skillSlotPref, statsUI.GetComponent<StatsUI>().skillContent).transform;
        }
    }

    /// <summary> To return a skill slot back to the pool </summary>
    /// <params name="slot"> The slot gameobject you want to return </params>
    private void ReturnSkillSlotToPool(Transform slot) {
        slot.gameObject.SetActive(false);
        skillSlotPool.Enqueue(slot);
    }
}
