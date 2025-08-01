using System.Collections;

using TMPro;

using Unity.VisualScripting;

using UnityEngine;
using UnityEngine.UI;

public class HudItemSlot : MonoBehaviour {
    public TextMeshProUGUI key;
    public TextMeshProUGUI count;

    public InventoryItem item;
    public Image icon;
    public Image cooldown;
    public int index;

    private bool canUse = true;

    private void Start() {
        key.text = index.ToString();
        count.text = "";
        item = null;
        // UpdateSlot(item);
    }

    private void Update() {
        if (Input.GetKeyDown(GetKeyCode())) {
            UseItem();
        }
    }

    public void UpdateSlot(InventoryItem newItem) {
        item = newItem;
        if (newItem != null) {
            count.text = newItem.count.ToString();
            icon.sprite = newItem.icon;
            icon.color = Color.white.WithAlpha(1);
        }
        else {
            count.text = "";
            icon.sprite = null;
            icon.color = Color.white.WithAlpha(0);
        }
    }

    public void UseItem() {
        if (item != null && canUse) {
            StartCoroutine(ShowCooldown(item.cooldownTIme));
            canUse = false;
        }
    }

    private IEnumerator ShowCooldown(float cooldownTime) {
        float elapsed = 0;
        cooldown.fillAmount = 1;

        while (elapsed < cooldownTime) {
            float t = elapsed / cooldownTime;
            cooldown.fillAmount = 1 - t;

            elapsed += Time.deltaTime;
            yield return null;
        }

        cooldown.fillAmount = 0;
        RemoveItem();
        canUse = true;
        yield return null;
    }

    private void RemoveItem() {
        if (item == null) return;

        InventoryManager.Instance.RemoveItem(item.itemName, 1);
        if (item.count <= 0) UpdateSlot(null);
    }

    private KeyCode GetKeyCode() {
        switch (index) {
            case 1:
                return KeyCode.Alpha1;
            case 2:
                return KeyCode.Alpha2;
            case 3:
                return KeyCode.Alpha3;
            case 4:
                return KeyCode.Alpha4;
            case 5:
                return KeyCode.Alpha5;
            default:
                return KeyCode.Alpha6;
        }
    }
}
