using System;

using Game.Utils;

using UnityEngine;

public class PopupManager : MonoBehaviour {
    public static event Action<string, Vector2, float, Color> OnDamagePopupRequest;
    public static event Action<string, int, Sprite, Vector2, Color, float> OnItemPopupRequest;
    [SerializeField] private Transform damagePopupPref;
    [SerializeField] private Transform itemPopupPref;

    private void OnEnable() {
        PopupManager.OnDamagePopupRequest += ShowDamagePopup;
        PopupManager.OnItemPopupRequest += ShowItemPopup;
    }

    private void OnDisable() {
        PopupManager.OnDamagePopupRequest -= ShowDamagePopup;
        PopupManager.OnItemPopupRequest -= ShowItemPopup;
    }

    public static void RequestDamagePopup(string val, Vector2 position, float size, Color color) {
        OnDamagePopupRequest?.Invoke(val, position, size, color);
    }

    public static void RequestItemPopup(string name, int count, Sprite icon, Vector2 position, Color color, float size) {
        OnItemPopupRequest?.Invoke(name, count, icon, position, color, size);
    }

    private void ShowDamagePopup(string val, Vector2 position, float size, Color color) {
        Vector2 offset = new Vector2(0.5f, 0.5f);
        DamagePopup popup = Instantiate(damagePopupPref, position + offset, Quaternion.identity).GetComponent<DamagePopup>();

        popup.text.text = val;
        popup.text.fontSize = size;
        popup.text.color = color;

        popup.canvas.overrideSorting = true;
        popup.canvas.sortingLayerName = "Popup";
        popup.canvas.sortingOrder = 100;
    }

    private void ShowItemPopup(string name, int count, Sprite icon, Vector2 position, Color color, float size) {
        Vector2 offset = new Vector2(0, 0.1f);
        ItemPopup popup = Instantiate(itemPopupPref, position + offset, Quaternion.identity).GetComponent<ItemPopup>();

        popup.text.text = $"{name} x {count}";
        popup.text.fontSize = size;
        popup.text.color = color;
        popup.text.color = color;
        popup.icon.sprite = icon;

        popup.canvas.overrideSorting = true;
        popup.canvas.sortingLayerName = "Popup";
        popup.canvas.sortingOrder = 100;
    }
}
