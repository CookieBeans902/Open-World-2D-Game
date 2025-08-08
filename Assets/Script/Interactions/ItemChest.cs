using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class ItemChest : MonoBehaviour, IInteract {
    public bool isOpen;
    [SerializeField] private Sprite open;
    [SerializeField] private SpriteRenderer minimap;
    [SerializeField] private List<ItemSO> items;
    [SerializeField] private List<int> maxCounts;

    private IEnumerator ShowPopups() {
        for (int i = 0; i < items.Count; i++) {
            InventoryManager.Instance.AddItem(InventoryItem.Create(items[i]), maxCounts[i]);
            PopupManager.RequestItemPopup(items[i].name, maxCounts[i], items[i].icon, transform.position, Color.white, 4);
            yield return new WaitForSeconds(0.5f);
        }
    }

    public void OnInteract() {
        if (!isOpen) {
            GetComponent<SpriteRenderer>().sprite = open;
            minimap.sprite = open;
            isOpen = true;
            StartCoroutine(ShowPopups());
        }
    }
}
