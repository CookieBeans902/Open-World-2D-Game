// using System.Collections.Generic;

// using TMPro;

// using Unity.VisualScripting;

// using UnityEngine;
// using UnityEngine.UI;

// public class InventoryUIManager : MonoBehaviour {
//     public static InventoryUIManager Instance;

//     // prefab of the inventoryUI slot
//     [SerializeField] private GameObject slotPref;

//     // To mantain a pool of slots
//     private Queue<Transform> slotPool = new Queue<Transform>();
//     private InventoryUI inventoryUI;
//     private Transform scrollview;
//     private Transform content;
//     private bool isActive;
//     // private int height = 480;
//     // private int width = 480;
//     // private int cellSize = 100;
//     // private int xspacing = 10;
//     // private int yspacing = 10;

//     // private void Awake() {
//     //     if (InventoryUIManager.Instance == null) {
//     //         // this is to make this object persist across scenes
//     //         DontDestroyOnLoad(gameObject);
//     //         Instance = this;
//     //     }
//     //     else if (InventoryUIManager.Instance != this) {
//     //         // destroy self if an instance is already present to ensure there is only one manager
//     //         Destroy(gameObject);
//     //         return;
//     //     }
//     // }

//     // void Update() {
//     //     if (Input.GetKeyDown(KeyCode.I)) {
//     //         if (!isActive) {
//     //             if (inventoryUI == null)
//     //                 // inventoryUI = Instantiate(uiPref).GetComponent<InventoryUI>();
//     //                 inventoryUI.GetComponent<RectTransform>().sizeDelta = new Vector3(width, height);

//     //             scrollview = inventoryUI.scrollView;
//     //             content = inventoryUI.content;
//     //             isActive = true;

//     //             Button close = inventoryUI.transform.GetChild(0).GetChild(2).GetComponent<Button>();
//     //             close.onClick.RemoveAllListeners();
//     //             close.onClick.AddListener(() => ClearUI());
//     //             UpdateUI();
//     //         }
//     //         else {
//     //             ClearUI();
//     //         }
//     //     }
//     // }


//     /// <summary> To update the inventoryUI UI whenever it it changed </summary>
//     public void UpdateUI() {
//         if (!isActive) return;
//         Dictionary<string, InventoryItem> dict = InventoryManager.Instance.items;
//         if (dict == null) return;

//         // clear up the content gameObject first
//         foreach (Transform slot in content) ReturnSlotToPool(slot);

//         ContentItemUI item;
//         int num = 0;

//         foreach (KeyValuePair<string, InventoryItem> p in dict) {
//             int c = p.Value.count;
//             int s = p.Value.maxStack;
//             while (c > 0) {
//                 item = GetSlotFromPool().GetComponent<ContentItemUI>();
//                 item.icon.sprite = p.Value.icon;
//                 item.text.text = (c >= s ? s : c).ToString();
//                 c -= s;
//                 num++;
//             }
//         }

//         // int columns = 4;
//         // int h = (int)Mathf.Ceil(num / columns) * (cellSize + yspacing);
//         // h = h < height ? height : h;
//         // GridLayoutGroup layout = content.GetComponent<GridLayoutGroup>();
//         // layout.cellSize = new Vector2(cellSize, cellSize);
//         // layout.spacing = new Vector2(xspacing, yspacing);
//         // content.GetComponent<RectTransform>().sizeDelta = new Vector2(width, h);
//         Canvas.ForceUpdateCanvases();
//     }

//     /// <summary> To remove the ui from the screen </summary>
//     private void ClearUI() {
//         if (content != null)
//             foreach (Transform slot in content) ReturnSlotToPool(slot);

//         GameObject toDestroy = inventoryUI.gameObject;
//         inventoryUI = null;
//         scrollview = null;
//         content = null;
//         toDestroy.SetActive(false);
//         Destroy(toDestroy);
//         slotPool = new Queue<Transform>();
//         isActive = false;
//     }

//     /// <summary> To get a slot from the slot pool</summary>
//     /// <returns>A slot from the slot pool</returns>
//     private Transform GetSlotFromPool() {
//         if (slotPool.Count > 0) {
//             Transform slot = slotPool.Dequeue();
//             slot.gameObject.SetActive(true);
//             return slot;
//         }
//         else {
//             return Instantiate(slotPref, content).transform;
//         }
//     }

//     /// <summary> To return a slot back to the pool </summary>
//     /// <params name="slot"> The slot gameobject you want to return </params>
//     private void ReturnSlotToPool(Transform slot) {
//         slot.gameObject.SetActive(false);
//         slotPool.Enqueue(slot);
//     }
// }


// using UnityEngine;
// using TMPro;
// using System.Collections.Generic;
// using UnityEngine.UI;
// using Unity.VisualScripting;
// using System.Linq;

// public class StatsUIManager : MonoBehaviour {
//     public static StatsUIManager Instance;

//     // prefab of the inventoryUI slot
//     [SerializeField] private GameObject slotPref;

//     // To mantain a pool of slots
//     private Queue<Transform> slotPool = new Queue<Transform>();


//     [SerializeField] private GameObject uiPref;
//     [SerializeField] private GameObject fieldPref;
//     // [SerializeField] private GameObject pickupPref;
//     // [SerializeField] private GameObject descPref;

//     // private List<GameObject> pickups;
//     // private List<Transform> descs = new List<Transform>();
//     private Dictionary<string, GameObject> fields = new Dictionary<string, GameObject>();
//     private Queue<Transform> descPool = new Queue<Transform>();
//     private GameObject statsUI;
//     private Character charData;
//     private bool isActive;
//     private int currIndex;
//     // private float separation = 60;

//     private void Awake() {
//         if (StatsUIManager.Instance == null) {
//             // this is to make this object persist across scenes
//             DontDestroyOnLoad(gameObject);
//             Instance = this;

//             currIndex = 0;
//             isActive = false;
//         }
//         else if (StatsUIManager.Instance != this) {
//             // destroy self if an instance is already present to ensure there is only one manager
//             Destroy(gameObject);
//             return;
//         }
//     }

//     private void Update() {
//         if (Input.GetKeyDown(KeyCode.E)) {
//             if (!isActive) {
//                 statsUI = Instantiate(uiPref);
//                 Button prevButton = statsUI.GetComponent<StatsUI>().prevButton;
//                 Button nextButton = statsUI.GetComponent<StatsUI>().nextButton;
//                 nextButton.onClick.AddListener(() => {
//                     // for (int j = 0; j < pickups.Count; j++) pickups[j].SetActive(false);
//                     int length = CharacterManager.Instance.characters.Count;
//                     currIndex = currIndex < length - 1 ? currIndex + 1 : currIndex;
//                     UpdateUI();
//                 });
//                 prevButton.onClick.AddListener(() => {
//                     // for (int j = 0; j < pickups.Count; j++) pickups[j].SetActive(false);
//                     currIndex = currIndex > 0 ? currIndex - 1 : currIndex;
//                     UpdateUI();
//                 });

//                 Button close = statsUI.GetComponent<StatsUI>().closeButton;
//                 close.onClick.RemoveAllListeners();
//                 close.onClick.AddListener(() => ClearUI());

//                 isActive = true;
//                 UpdateUI();
//             }
//             else {
//                 ClearUI();
//             }
//         }
//     }


//     /// <summary> To update the ui </summary>
//     public void UpdateUI() {
//         if (statsUI == null)
//             return;
//         if (fields == null)
//             return;
//         if (!isActive)
//             return;

//         charData = CharacterManager.Instance.characters[currIndex];

//         InitFieldsAndEquips();
//         UpdateEquipments();

//         string charName = $"{charData.characterName} ({charData.characterClass})\nLevel {charData.curLvl}";
//         statsUI.GetComponent<StatsUI>().nameText.text = charName;

//         UpdateField("Max Health", charData.BaseMHP, charData.MHP);
//         UpdateField("Attack", charData.BaseATK, charData.ATK);
//         UpdateField("Magic Attack", charData.BaseMATK, charData.MATK);
//         UpdateField("Defence", charData.BaseDEF, charData.DEF);
//         UpdateField("Magic Defence", charData.BaseMDEF, charData.MDEF);
//         UpdateField("Agility", charData.BaseAGI, charData.AGI);
//         UpdateField("Luck", charData.BaseLUCK, charData.LUCK);


//         Dictionary<string, InventoryItem> dict = InventoryManager.Instance.items;
//         if (dict == null) return;

//         // clear up the content gameObject first
//         foreach (Transform slot in statsUI.GetComponent<StatsUI>().inventoryContent) ReturnSlotToPool(slot);

//         ItemSlot item;
//         int num = 0;

//         foreach (KeyValuePair<string, InventoryItem> p in dict) {
//             int c = p.Value.count;
//             int s = p.Value.maxStack;
//             while (c > 0) {
//                 item = GetSlotFromPool().GetComponent<ItemSlot>();
//                 item.icon.sprite = p.Value.icon;
//                 item.count.text = (c >= s ? s : c).ToString();
//                 c -= s;
//                 num++;
//             }
//         }

//         Canvas.ForceUpdateCanvases();
//     }


/// <summary> To update the equpments ui in the slots</summary>
// private void UpdateEquipments() {
//     List<Transform> slots = new List<Transform>();
//     List<Equipment> equips = charData.equipments.Values.ToList();

//     slots.Add(statsUI.GetComponent<StatsUI>().headSlot);
//     slots.Add(statsUI.GetComponent<StatsUI>().bodySlot);
//     slots.Add(statsUI.GetComponent<StatsUI>().hand2Slot);
//     slots.Add(statsUI.GetComponent<StatsUI>().hand1Slot);
//     slots.Add(statsUI.GetComponent<StatsUI>().bootSlot);
//     slots.Add(statsUI.GetComponent<StatsUI>().AccessorySlot);

//     for (int i = 0; i < equips.Count; i++) {
//         EquimentSlot slot = slots[i].GetComponent<EquimentSlot>();

//         if (equips[i] != null) {
//             slot.icon.sprite = equips[i].icon;
//             slot.icon.color = Color.white;
//         }
//         else {
//             slot.icon.sprite = null;
//             slot.icon.color = new Color().WithAlpha(0);
//         }

//         int index = i;
//         slot.button.onClick.RemoveAllListeners();
//         slot.button.onClick.AddListener(() => {
//             // if (!pickups[index].activeSelf) {
//             //     ShowPickup(index);
//             // }
//             // else {
//             //     foreach (Transform desc in descs) ReturnDescToPool(desc);
//             //     descs = new List<Transform>();
//             //     pickups[index].SetActive(false);
//             // }
//             UpdateUI();
//         });
//     }
// }


/// <summary> To show a pickup window for a slot </summary>
/// <params name="i"> the slot you want to show pickup for (typecated to EquimentSlot inside the function) </params>
// private void ShowPickup(int i) {
//     List<InventoryItem> items = InventoryManager.Instance.items.Values.ToList();
//     List<Equipment> equips = new List<Equipment>();

//     Class charClass = charData.characterClass;
//     Transform content = pickups[i].transform.GetChild(0).GetChild(0).GetChild(0);

//     foreach (InventoryItem item in items) {
//         if (item.itemType == ItemType.Equipment) {
//             Equipment e = Equipment.Create(item.equipment);
//             if (e.slot != (EquipmentSlot)i || e.isEquiped) continue;
//             foreach (Class c in e.validClasses) {
//                 if (charClass == c) {
//                     equips.Add(e);
//                     break;
//                 }
//             }
//         }
//     }
//     equips.Add(null);

//     foreach (Transform desc in descs) ReturnDescToPool(desc);
//     descs = new List<Transform>();
//     foreach (Equipment e in equips) AddChoice(e, i);

//     float height = equips.Count * 68;
//     float width = content.GetComponent<RectTransform>().sizeDelta.x;
//     content.GetComponent<RectTransform>().sizeDelta = new Vector2(width, height);

//     for (int j = 0; j < pickups.Count; j++) pickups[j].SetActive(false);
//     pickups[i].transform.SetAsLastSibling();
//     pickups[i].SetActive(true);
// }


/// <summary> To add a hoice to the pickup </summary>
/// <params name="e">Equipment you want to add</params>
/// <params name="i">index of the pickup (type casted to Equipmentslot inside the function) </params>
// private void AddChoice(Equipment e, int i) {
//     ContentItemUI components = GetDescFromPool().GetComponent<ContentItemUI>();
//     Transform content = pickups[i].transform.GetChild(0).GetChild(0).GetChild(0);
//     components.transform.SetParent(content, false);
//     if (e != null) {
//         components.icon.sprite = e.icon;
//         components.icon.color = Color.white;
//         var statBuffs = new List<(string, int)> {
//             ("hp", e.hpBuff),
//             ("atk", e.atkBuff),
//             ("matk", e.matkBuff),
//             ("def", e.defBuff),
//             ("mdef", e.mdefBuff),
//             ("agi", e.agiBuff),
//             ("luck", e.luckBuff)
//         };
//         components.text.text = e.equipmentName + '\n' + string.Join(" ", statBuffs
//                 .Where(stat => stat.Item2 > 0)
//                 .Select(stat => $"{stat.Item1}<color=green>+{stat.Item2}</color>"));
//     }
//     else {
//         components.icon.sprite = null;
//         components.icon.color = new Color().WithAlpha(0);
//     }

//     components.button.onClick.RemoveAllListeners();
//     components.button.onClick.AddListener(() => {
//         if (e != null) CharacterManager.Instance.characters[currIndex].Equip(e);
//         else CharacterManager.Instance.characters[currIndex].Unequip((EquipmentSlot)i);

//         if (pickups[i] != null) {
//             foreach (Transform desc in descs) ReturnDescToPool(desc);
//             descs = new List<Transform>();
//             pickups[i].SetActive(false);
//         }
//         UpdateUI();
//     });
//     descs.Add(components.transform);
// }


//     /// <summary> To update the value of a field </summary>
//     /// <params name="name">name of the field</params>
//     /// <params name="baseValue">value without equipment buffs</params>
//     /// <params name="value">finnal value after adding equipment buffs</params>
//     private void UpdateField(string name, int baseValue, int value) {
//         if (fields == null)
//             fields = new Dictionary<string, GameObject>();

//         GameObject field = fields[name];
//         TextMeshProUGUI text = field.GetComponent<TextMeshProUGUI>();
//         name = $"<color=white>{name}</color>";
//         string baseVal = $"<color=yellow>{baseValue}</color>";
//         string buffVal = $"<color=green>{value - baseValue}</color>";
//         text.text = $"{name} : {baseVal} + {buffVal}";

//         fields[name] = field;
//     }


//     /// <summary> To set field positions and pickups </summary>
//     private void InitFieldsAndEquips() {
//         if (statsUI == null) {
//             return;
//         }
//         // StatsUI components = statsUI.GetComponent<StatsUI>();
//         // header = header == null ? Instantiate(headerTextPref, components.header) : header;

//         if (fields == null || fields.Count == 0) {
//             fields = new Dictionary<string, GameObject>();
//             fields["Max Health"] = Instantiate(fieldPref, statsUI.GetComponent<StatsUI>().fieldsContent);
//             fields["Attack"] = Instantiate(fieldPref, statsUI.GetComponent<StatsUI>().fieldsContent);
//             fields["Magic Attack"] = Instantiate(fieldPref, statsUI.GetComponent<StatsUI>().fieldsContent);
//             fields["Defence"] = Instantiate(fieldPref, statsUI.GetComponent<StatsUI>().fieldsContent);
//             fields["Magic Defence"] = Instantiate(fieldPref, statsUI.GetComponent<StatsUI>().fieldsContent);
//             fields["Agility"] = Instantiate(fieldPref, statsUI.GetComponent<StatsUI>().fieldsContent);
//             fields["Luck"] = Instantiate(fieldPref, statsUI.GetComponent<StatsUI>().fieldsContent);

//             // Transform start = components.start;
//             // float x = start.position.x, y = start.position.y;

//             // foreach (KeyValuePair<string, GameObject> p in fields) {
//             //     Transform field = p.Value.transform;
//             //     field.position = new Vector3(x, y, field.position.z);
//             //     field.gameObject.SetActive(true);
//             //     y -= separation;
//             // }
//         }

//         // List<Transform> slots = components.equipSlots;
//         // Transform pos;

//         // if (pickups == null || pickups.Count == 0) {
//         //     pickups = new List<GameObject>();
//         //     for (int i = 0; i < slots.Count; i++) {
//         //         pos = slots[i].GetComponent<ContentItemUI>().pos;
//         //         GameObject pickup = Instantiate(pickupPref, statsUI.transform);
//         //         pickup.transform.position = pos.position;
//         //         pickups.Add(pickup);
//         //         pickups[i].SetActive(false);
//         //     }
//         // }
//     }


//     /// <summary> To remove the ui from the screen</summary>
//     private void ClearUI() {
//         GameObject toDestroy = statsUI;
//         statsUI = null;
//         toDestroy.SetActive(false);
//         Destroy(toDestroy);
//         // descPool = new Queue<Transform>();
//         fields = new Dictionary<string, GameObject>();
//         // pickups = new List<GameObject>();
//         slotPool = new Queue<Transform>();
//         isActive = false;
//     }


//     /// <summary> To get a desc from the slot pool</summary>
//     /// <returns>A desc from the desc pool</returns>
//     // private Transform GetDescFromPool() {
//     //     if (descPool.Count > 0) {
//     //         Transform desc = descPool.Dequeue();
//     //         desc.gameObject.SetActive(true);
//     //         return desc;
//     //     }
//     //     else {
//     //         return Instantiate(descPref, statsUI.transform).transform;
//     //     }
//     // }


//     /// <summary> To return a desc back to the pool </summary>
//     /// <params name="desc"> The desc gameobject you want to return </params>
//     // private void ReturnDescToPool(Transform desc) {
//     //     ContentItemUI components = desc.GetComponent<ContentItemUI>();
//     //     components.text.text = "";
//     //     components.button.onClick.RemoveAllListeners();
//     //     components.text.text = "";
//     //     components.icon.sprite = null;
//     //     components.icon.color = new Color().WithAlpha(0);
//     //     components.button.onClick.RemoveAllListeners();
//     //     desc.gameObject.SetActive(false);
//     //     descPool.Enqueue(desc);
//     // }

//     /// <summary> To get a slot from the slot pool</summary>
//     /// <returns>A slot from the slot pool</returns>
//     private Transform GetSlotFromPool() {
//         if (slotPool.Count > 0) {
//             Transform slot = slotPool.Dequeue();
//             slot.gameObject.SetActive(true);
//             return slot;
//         }
//         else {
//             return Instantiate(slotPref, statsUI.GetComponent<StatsUI>().inventoryContent).transform;
//         }
//     }

//     /// <summary> To return a slot back to the pool </summary>
//     /// <params name="slot"> The slot gameobject you want to return </params>
//     private void ReturnSlotToPool(Transform slot) {
//         slot.gameObject.SetActive(false);
//         slotPool.Enqueue(slot);
//     }
// }

