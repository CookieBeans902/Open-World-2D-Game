using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUIManager : MonoBehaviour {
    public static InventoryUIManager Instance;

    // prefab of the inventoryUI canvas ui
    [SerializeField] private GameObject uiPref;

    // prefab of the inventoryUI slot
    [SerializeField] private GameObject slotPref;

    // To mantain a pool of slots
    private Queue<Transform> slotPool = new Queue<Transform>();
    private InventoryUI inventoryUI;
    private Transform scrollview;
    private Transform content;
    private bool isActive;
    private int height = 480;
    private int width = 480;
    private int cellSize = 100;
    private int xspacing = 10;
    private int yspacing = 10;

    private void Awake() {
        if (InventoryUIManager.Instance == null) {
            // this is to make this object persist across scenes
            DontDestroyOnLoad(gameObject);
            Instance = this;
        }
        else if (InventoryUIManager.Instance != this) {
            // destroy self if an instance is already present to ensure there is only one manager
            Destroy(gameObject);
            return;
        }
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.I)) {
            if (!isActive) {
                if (inventoryUI == null)
                    inventoryUI = Instantiate(uiPref).GetComponent<InventoryUI>();
                inventoryUI.GetComponent<RectTransform>().sizeDelta = new Vector3(width, height);

                scrollview = inventoryUI.scrollView;
                content = inventoryUI.content;
                isActive = true;

                Button close = inventoryUI.transform.GetChild(0).GetChild(2).GetComponent<Button>();
                close.onClick.RemoveAllListeners();
                close.onClick.AddListener(() => ClearUI());
                UpdateUI();
            }
            else {
                ClearUI();
            }
        }
    }


    /// <summary> To update the inventoryUI UI whenever it it changed </summary>
    public void UpdateUI() {
        if (!isActive) return;
        Dictionary<string, InventoryItem> dict = InventoryManager.Instance.items;
        if (dict == null) return;

        // clear up the content gameObject first
        foreach (Transform slot in content) ReturnSlotToPool(slot);

        ContentItemUI item;
        int num = 0;

        foreach (KeyValuePair<string, InventoryItem> p in dict) {
            int c = p.Value.count;
            int s = p.Value.maxStack;
            while (c > 0) {
                item = GetSlotFromPool().GetComponent<ContentItemUI>();
                item.icon.sprite = p.Value.icon;
                item.text.text = (c >= s ? s : c).ToString();
                c -= s;
                num++;
            }
        }

        int columns = 4;
        int h = (int)Mathf.Ceil(num / columns) * (cellSize + yspacing);
        h = h < height ? height : h;
        GridLayoutGroup layout = content.GetComponent<GridLayoutGroup>();
        layout.cellSize = new Vector2(cellSize, cellSize);
        layout.spacing = new Vector2(xspacing, yspacing);
        content.GetComponent<RectTransform>().sizeDelta = new Vector2(width, h);
        Canvas.ForceUpdateCanvases();
    }

    /// <summary> To remove the ui from the screen </summary>
    private void ClearUI() {
        if (content != null)
            foreach (Transform slot in content) ReturnSlotToPool(slot);

        GameObject toDestroy = inventoryUI.gameObject;
        inventoryUI = null;
        scrollview = null;
        content = null;
        toDestroy.SetActive(false);
        Destroy(toDestroy);
        slotPool = new Queue<Transform>();
        isActive = false;
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
            return Instantiate(slotPref, content).transform;
        }
    }

    /// <summary> To return a slot back to the pool </summary>
    /// <params name="slot"> The slot gameobject you want to return </params>
    private void ReturnSlotToPool(Transform slot) {
        slot.gameObject.SetActive(false);
        slotPool.Enqueue(slot);
    }
}
