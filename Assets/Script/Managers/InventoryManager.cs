using System;
using System.Collections.Generic;
using System.Data;
using Game.Utils;
using UnityEngine;

public class InventoryManager : MonoBehaviour,IDataPersistence {
    public event EventHandler OnInventoryChange;
    public static InventoryManager Instance { get; private set; }

    /* A scriptable Object which has a list of items already defined for easy addition and removal of items,
       you define it in the editor and assign the ItemSO objects to it's feilds. (Check the Scripts/ScriptableObject folder) */
    [SerializeField] private List<ItemSO> initItemList;
    public int maxSlots { get; private set; }
    public Dictionary<string, ItemSO> initItemDict;
    // to mantain a dictionary of current items in the inventory
    public Dictionary<string, InventoryItem> items { get; private set; }

    private void Awake()
    {
        if (InventoryManager.Instance == null)
        {
            // this is to make this object persist across scenes
            DontDestroyOnLoad(gameObject);
            Instance = this;

            items = new Dictionary<string, InventoryItem>();
            maxSlots = 10;
        }
        else if (InventoryManager.Instance != this)
        {
            // destroy self if an instance is already present to ensure there is only one manager
            Destroy(gameObject);
            return;
        }
        //Initialize the Dictionary for references
        initItemDict ??= new();
        foreach (var item in initItemList)
        {
            initItemDict.Add(item.itemName, item);
        }
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.A)) {
            InventoryItem item = InventoryItem.Create(initItemDict["Apple"]);
            AddItem(item, 1);
        }
        if (Input.GetKeyDown(KeyCode.B)) {
            InventoryItem item = InventoryItem.Create(initItemDict["Banana"]);
            AddItem(item, 1);
        }
        if (Input.GetKeyDown(KeyCode.R)) {
            RemoveItem("Apple", 1);
        }
        // if (Input.GetKeyDown(KeyCode.S)) {
        //     ShowItems();
        // }
    }


    /// <summary>To add an item to the inventory</summary>
    /// <param name="item">The item you want to add</param>
    /// <param name="amount">The amount you want to add</param>
    public void AddItem(InventoryItem item, int amount) {
        if (item == null)
            return;

        string name = item.itemName;
        item.count = amount;
        if (items.ContainsKey(name)) {
            int slotsRequired = (int)Mathf.Ceil((items[name].count + amount * 1.0f) / item.maxStack);
            int slotsPresent = maxSlots - TotalSlotCount() + SlotCount(name);

            if (slotsPresent >= slotsRequired)
                items[name].count += amount;
            else
                Debug.Log("Inventory is full");
        }
        else {
            int slotsRequired = (int)Mathf.Ceil(amount / (item.maxStack != 0 ? item.maxStack : 1) * 1.0f);
            int slotsPresent = maxSlots - TotalSlotCount();

            if (slotsPresent >= slotsRequired) {
                items[name] = item;
            }
            else {
                Debug.Log("Inventory is full");
            }
        }
        StatsUIManager.Instance.UpdateUI();
        TriggerChange();
    }


    /// <summary>To remove an item from the inventory</summary>
    /// <param name="name">Name of the item you want to remove</param>
    /// <param name="amount">The amount you want to remove</param>
    public void RemoveItem(string name, int amount) {
        if (items == null)
            return;

        if (items.ContainsKey(name)) {
            items[name].count -= amount;
            if (items[name].count <= 0) items.Remove(name);
        }
        else {
            Debug.Log(name + " ins't in the inventory");
        }
        StatsUIManager.Instance.UpdateUI();

        TriggerChange();
    }


    /// <summary>To get the total number of slots occupied</summary>
    /// <returns>The total number of slots occupied</returns>
    public int TotalSlotCount() {
        int c = 0;
        foreach (KeyValuePair<string, InventoryItem> p in items)
            c += (int)Mathf.Ceil(p.Value.count / p.Value.maxStack * 1.0f);

        return c;
    }


    /// <summary>To get the number of slots occupied by the specified item</summary>
    /// <param name="name">The name of the item you want to look up for</param>
    /// <returns>The number of slots occupied by the item</returns>
    public int SlotCount(string name) {
        if (!items.ContainsKey(name))
            return 0;

        return (int)Mathf.Ceil(items[name].count / items[name].maxStack * 1.0f);
    }


    /// <summary>Show all the items in the distionary, useful while debugging</summary>
    private void ShowItems() {
        foreach (KeyValuePair<string, InventoryItem> i in items) {
            Debug.Log(i.Key + " " + i.Value.count);
        }
    }

    public void TriggerChange() {
        OnInventoryChange?.Invoke(this, EventArgs.Empty);
    }

    public void LoadData(GameData gameData)
    {
        items.Clear();
        foreach (var item in gameData.inventoryItems)
        {
            InventoryItem inventoryItem = InventoryItem.Create(initItemDict[item.itemName]);
            inventoryItem.count = item.count;
            inventoryItem.slotNumber = item.slot;
            items.Add(item.itemName, inventoryItem);
        }
        Debug.Log("Loaded Data");
        FunctionTimer.CreateGlobalTimer(()=>OnInventoryChange?.Invoke(this, EventArgs.Empty),0.01f);
    }

    public void SaveData(GameData gameData)
    {
        //Saving inventory Items
        gameData.inventoryItems.Clear();
        foreach (KeyValuePair<string, InventoryItem> pair in items)
        {
            ItemSaveData data = new()
            {
                itemName = pair.Key,
                count = pair.Value.count,
                slot = pair.Value.slotNumber
            };
            gameData.inventoryItems.Add(data);
        }
    }
}
