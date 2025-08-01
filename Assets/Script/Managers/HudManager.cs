using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HudManager : MonoBehaviour {
    public HudManager Instance { get; private set; }

    [SerializeField] private TextMeshProUGUI playerName;
    [SerializeField] private Image healthFill;
    [SerializeField] private Image expFill;
    [SerializeField] private List<HudItemSlot> itemSlots = Enumerable.Repeat<HudItemSlot>(null, 6).ToList();
    [SerializeField] private List<HudSkillSlot> skillSlots = Enumerable.Repeat<HudSkillSlot>(null, 3).ToList();

    private Character charData;
    private bool isActive;
    private int currIndex;

    private void Awake() {
        if (StatsUIManager.Instance == null) {
            // this is to make this object persist across scenes
            DontDestroyOnLoad(gameObject);
            Instance = this;

            currIndex = 0;
            isActive = true;
        }
        else if (StatsUIManager.Instance != this) {
            // destroy self if an instance is already present to ensure there is only one manager
            Destroy(gameObject);
            return;
        }
    }

    private void Start() {
        InventoryManager.Instance.OnInventoryChange += UpdateActiveItems;
        Debug.Log("Added the function");
        StatsUIManager.Instance.OnSkillChange += UpdateActiveSkills;
        StatsUIManager.Instance.OnStatChange += UpdatePlayerStats;
    }


    public void UpdateActiveItems(System.Object sender, EventArgs e) {
        List<InventoryItem> items = InventoryManager.Instance.items?.Values.ToList();
        if (items == null) return;

        foreach (HudItemSlot slot in itemSlots) slot.UpdateSlot(null);

        foreach (InventoryItem i in items) {
            if (i.slotNumber != -1) itemSlots[i.slotNumber - 1].UpdateSlot(i);
        }
    }

    public void UpdateActiveSkills(System.Object sender, EventArgs e) {
        charData = CharacterManager.Instance.characters[currIndex];
        List<Skill> skills = charData.skills;

        foreach (HudSkillSlot slot in skillSlots) slot.UpdateSlot(null);

        foreach (Skill s in skills) {
            if (s.slot != -1) {
                skillSlots[s.slot - 1].UpdateSlot(s);
            }
        }
    }

    public void UpdatePlayerStats(System.Object sender, EventArgs e) {
        charData = CharacterManager.Instance.characters[currIndex];
        playerName.text = $"{charData.characterName} Lv {charData.curLvl}";

        healthFill.fillAmount = (float)charData.curHp / charData.MHP;
        expFill.fillAmount = (float)charData.curExp / charData.MaxExp;
    }
}
